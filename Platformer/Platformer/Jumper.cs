using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Platformer
{
    public class Jumper : Sprite
    {
        public Vector2 Movement { get; set; }
        private Vector2 oldPosition;
        public Jumper(Texture2D texture, Vector2 position, SpriteBatch spritebatch)
            : base(texture, position, spritebatch)
        { }

        public void Update(GameTime gameTime)
        {
            CheckKeyboardAndUpdateMovement();
            AffectWithGravity();
            SimulateFriction();
            MoveAsFarAsPossible(gameTime);
            StopMovingIfBlocked();
        }

        private void CheckKeyboardAndUpdateMovement()
        {
            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Left)) { Movement -= Vector2.UnitX * .5f; }
            if (keyboardState.IsKeyDown(Keys.Right)) { Movement += Vector2.UnitX; }
            if (keyboardState.IsKeyDown(Keys.Space) && IsOnFirmGround()) { Movement = -Vector2.UnitY * 20; }
        }

        private void AffectWithGravity()
        {
            Movement += Vector2.UnitY * .65f;
        }

        private void SimulateFriction()
        {
            if (IsOnFirmGround()) { Movement -= Movement * Vector2.One * .1f; }
            else { Movement -= Movement * Vector2.One * .02f; }
        }

        private void MoveAsFarAsPossible(GameTime gameTime)
        {
            oldPosition = Position;
            UpdatePositionBasedOnMovement(gameTime);
            Position = Board.CurrentBoard.WhereCanIGetTo(oldPosition, Position, Bounds);
        }

        private void UpdatePositionBasedOnMovement(GameTime gameTime)
        {
            Position += Movement * (float)gameTime.ElapsedGameTime.TotalMilliseconds / 15;
        }

        public bool IsOnFirmGround()
        {
            Rectangle onePixelLower = Bounds;
            onePixelLower.Offset(0, 1);
            return !Board.CurrentBoard.HasRoomForRectangle(onePixelLower);
        }

        private void StopMovingIfBlocked()
        {
            Vector2 lastMovement = Position - oldPosition;
            if (lastMovement.X == 0) { Movement *= Vector2.UnitY; }
            if (lastMovement.Y == 0) { Movement *= Vector2.UnitX; }
        }
    }
}