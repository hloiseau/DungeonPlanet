using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Drawing;

namespace DungeonPlanet.Library
{
    public class PlayerLib
    {
        public Vector2 Movement { get; set; }
        public static Vector2 Position { get; set; }
        public Vector2  OldPosition{ get; set; }

        int _height;
        int _width;

        public PlayerLib(Vector2 position, int width, int height)
        {
            Position = position;
            _height = height;
            _width = width;
        }

        public Rectangle Bounds
        {
            get { return new Rectangle((int)Position.X, (int)Position.Y, _width, _height); }
        }

        public bool IsDead(int life)
        {
            return life <= 0;
        }
        
        public void MoveAsFarAsPossible(float gameTime)
        {
            OldPosition = Position;
            UpdatePositionBasedOnMovement(gameTime);
            Position = Level.CurrentBoard.WhereCanIGetTo(OldPosition, Position, Bounds);
        }

        public void UpdatePositionBasedOnMovement(float gameTime)
        {
            Position += Movement * gameTime;
        }

        public void AffectWithGravity()
        {
            Movement += Vector2.UnitY * .65f;
        }

        public void SimulateFriction()
        {
            if (IsOnFirmGround()) { Movement -= Movement * Vector2.One * .1f; }
            else
            {
                Vector2 movement = Movement;
                movement.X -= Movement.X * Vector2.One.X * .04f;
                movement.Y -= Movement.Y * Vector2.One.Y * .01f;
                Movement = movement;
            }
        }

        public bool IsOnFirmGround()
        {
            Rectangle onePixelLower = Bounds;
            onePixelLower.Offset(0, 1);
            if (Level.CurrentBoard != null)
                return  !Level.CurrentBoard.HasRoomForRectangle(onePixelLower);
            return false;
        }

        public void Left()
        {
            Movement -= Vector2.UnitX;
        }
        public void Right()
        {
            Movement += Vector2.UnitX;
        }
        public void Jump() => Movement = -Vector2.UnitY * 25;
        public void StopMovingIfBlocked()
        {
            Vector2 lastMovement = Position - OldPosition;
            if (lastMovement.X == 0) { Movement *= Vector2.UnitY; }
            if (lastMovement.Y == 0) { Movement *= Vector2.UnitX; }
        }
    }
}

