using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace DungeonPlanet.Library
{
    public class EnemyLib
    {
        public Vector2 Movement { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 OldPosition { get; set; }

        int _height;
        int _width;

        public Rectangle Bounds
        {
            get { return new Rectangle((int)Position.X, (int)Position.Y, _width, _height); }
        }

        public Rectangle Vision
        {
            get { return new Rectangle((int)Position.X - 375, (int)Position.Y, _width * 20, _height); }
        }

        public EnemyLib(Vector2 position, int width, int height)
        {
            Position = position;
            _height = height;
            _width = width;
        }

        public void MoveAsFarAsPossible(float gameTime)
        {
            OldPosition = Position;
            UpdatePositionBasedOnMovement(gameTime);
            Position = BoardLib.CurrentBoard.WhereCanIGetTo(OldPosition, Position, Bounds);
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
            else { Movement -= Movement * Vector2.One * .02f; }
        }

        public bool IsOnFirmGround()
        {
            Rectangle onePixelLower = Bounds;
            onePixelLower.Offset(0, 1);
            return !BoardLib.CurrentBoard.HasRoomForRectangle(onePixelLower);
        }

        public void Left()
        {
            Movement -= Vector2.UnitX * .65f;
        }
        public void Right()
        {
            Movement += Vector2.UnitX * .65f;
        }

        public void StopMovingIfBlocked()
        {
            Vector2 lastMovement = Position - OldPosition;
            if (lastMovement.X == 0) { Movement *= Vector2.UnitY; }
            if (lastMovement.Y == 0) { Movement *= Vector2.UnitX; }
        }

        public void MovementPatterns()
        {
           
        }

        public void followingTheHero()
        {

        }
        public Vector2 GetDistanceTo(Vector2 destination)
        {
            return new Vector2(destination.X - Position.X, destination.Y - Position.Y);
        }
    }
}
