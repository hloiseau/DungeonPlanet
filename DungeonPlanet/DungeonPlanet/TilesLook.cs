using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonPlanet.Library
{
    class TilesLook
    {
        // The image representing the collection of images used for animation
        Texture2D _spriteSheet;

        // The scale used to display the sprite strip
        float _scale;

        // The number of frames that the animation contains
        int _frameCount;

        // The index of the current frame colone we are displaying
        public int _currentFrameLin;

        // The index of the current frame  we are displaying
        int _currentFrameCol;

        // The color of the frame we will be displaying
        Color _color;

        // The area of the image strip we want to display
        Rectangle _sourceRect = new Rectangle();

        // The area where we want to display the image strip in the game
        Rectangle _destinationRect = new Rectangle();

        // Width of a given frame
        int _frameWidth;

        // Height of a given frame
        int _frameHeight;


        public Vector2 _position;
        public TilesLook(Texture2D texture, float X, float Y, Tile.TypeSet tileType, Color color)
        {
            // Keep a local copy of the values passed in
            _color = color;
            _frameWidth = 64;
            _frameHeight = 64;
            _scale = 1;
            _currentFrameCol = 0;
            _currentFrameLin = 0;

            _position = new Vector2(X,Y);
            _spriteSheet = texture;

            // Grab the correct frame in the image strip by multiplying the currentFrame index by the Frame width
            
            if (tileType == Tile.TypeSet.External)
            {
                _currentFrameCol = 0;
                _currentFrameLin = 0;
            }
            if (tileType == Tile.TypeSet.Invisible)
            {
                _currentFrameCol = 1;
                _currentFrameLin = 0;
            }

            if (tileType == Tile.TypeSet.Wall)
            {
                _currentFrameCol = 0;
                _currentFrameLin = 1;
            }
            if (tileType == Tile.TypeSet.Platform)
            {
                int i = Level.CurrentBoard.GetNext(0,6);
                if (i == 0)
                {
                    _currentFrameCol = 1;
                    _currentFrameLin = 1;
                }
                if (i == 1)
                {
                    _currentFrameCol = 2;
                    _currentFrameLin = 1;
                }
                if (i >= 2 && i <= 6)
                {
                    _currentFrameCol = 3;
                    _currentFrameLin = 1;
                }
            }
            if (tileType == Tile.TypeSet.Background)
            {
                _currentFrameCol = 4;
                _currentFrameLin = 1;
            }

            _sourceRect = new Rectangle(_currentFrameCol * _frameWidth, _currentFrameLin * _frameHeight, _frameWidth, _frameHeight);

            // Grab the correct frame in the image strip by multiplying the currentFrame index by the frame width
            _destinationRect = new Rectangle((int)_position.X /*- (int)(_frameWidth * _scale) / 2*/,
            (int)_position.Y /*- (int)(_frameHeight * _scale) / 2*/,
            (int)(_frameWidth * _scale),
            (int)(_frameHeight * _scale));
        }
            
        // Draw the Animation Strip
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_spriteSheet, destinationRectangle: _destinationRect, sourceRectangle: _sourceRect, color: _color, effects: Effect);
        }

        /*
        public void Draw(SpriteBatch spriteBatch, float rotation)
        {
            spriteBatch.Draw(_spriteSheet, destinationRectangle: _destinationRect, sourceRectangle: _sourceRect, color: _color, rotation: rotation, effects: Effect);
        }
        */
        internal SpriteEffects Effect { get; set; }

        internal int FrameHeight
        {
            get { return _frameHeight; }
            set { _frameHeight = value; }
        }
        internal int FrameWidth
        {
            get { return _frameWidth; }
            set { _frameWidth = value; }
        }
        internal int FrameCount
        {
            get { return _frameCount; }
            set { _frameCount = value; }
        }
        internal Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }
        internal int CurrentFrameLin
        {
            get { return _currentFrameLin; }
            set { _currentFrameLin = value; }
        }
        internal int CurrentFrameCol
        {
            get { return _currentFrameCol; }
            set { _currentFrameCol = value; }
        }
    }
}
