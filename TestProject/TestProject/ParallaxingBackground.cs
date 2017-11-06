using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TestProject
{
    class ParallaxingBackground
    {
        // The image representing the parallaxing background
        Texture2D _texture;
        // An array of positions of the parallaxing background
        Vector2[] _positions;
        // The speed which the background is moving
        int _speed;
        int _bgHeight;
        int _bgWidth;
        public void Initialize(ContentManager content, String texturePath, int screenWidth, int screenHeight, int speed)
        {
             _bgHeight = screenHeight;
             _bgWidth = screenWidth;

            // Load the background texture we will be using
            _texture = content.Load<Texture2D>(texturePath);
            // Set the speed of the background
            this._speed = speed;
            // If we divide the screen with the texture width then we can determine the number of tiles need.
            // We add 1 to it so that we won't have a gap in the tiling
            _positions = new Vector2[screenWidth / _texture.Width + 1];
            // Set the initial positions of the parallaxing background
            for (int i = 0; i < _positions.Length; i++)
            {
                // We need the tiles to be side by side to create a tiling effect
                _positions[i] = new Vector2(i * _texture.Width, 0);
            }
        }

        public void Update(GameTime gametime)

        {
            // Update the positions of the background

            for (int i = 0; i < _positions.Length; i++)
            {
                // Update the position of the screen by adding the speed
                _positions[i].X += _speed;
                // If the speed has the background moving to the left
                if (_speed <= 0)
                {
                    // Check the texture is out of view then put that texture at the end of the screen
                    if (_positions[i].X <= -_texture.Width)
                    {
                        _positions[i].X = _texture.Width * (_positions.Length - 1);
                    }
                }
                // If the speed has the background moving to the right
                else
                {
                    // Check if the texture is out of view then position it to the start of the screen
                    if (_positions[i].X >= _texture.Width * (_positions.Length - 1))
                    {
                        _positions[i].X = -_texture.Width;
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < _positions.Length; i++)
            {
                Rectangle rectBg = new Rectangle((int)_positions[i].X, (int)_positions[i].Y, _bgWidth, _bgHeight);
                spriteBatch.Draw(_texture, rectBg, Color.White);
            }
        }
    }
}
