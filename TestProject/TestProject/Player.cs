using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace TestProject
{
    class Player
    {
        public Vector2 _position;
        bool _active;
        int _health;
        Animation _playerAnimation;

        public void Initialize(Animation animation, Vector2 position)
        {
            _playerAnimation = animation;
            // Set the starting position of the player around the middle of the screen and to the back
            _position = position;
            // Set the player to be active
            _active = true;
            // Set the player health
            _health = 100;
        }
        public void Update(GameTime gameTime)
        {
            _playerAnimation.Position = _position;
            _playerAnimation.Update(gameTime);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            _playerAnimation.Draw(spriteBatch);
        }

        public int Width
        {
            get { return _playerAnimation.FrameWidth; }
        }
        public int Height
        {
            get { return _playerAnimation.FrameHeight; }
        }
        internal Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }
    }
}
