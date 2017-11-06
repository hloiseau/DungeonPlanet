using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ennemi.sprites
{
    public class Sprite
    {
        readonly Texture2D _texture;
        protected float _rotation;
        protected KeyboardState _currentKey;
        protected KeyboardState _previousKey;

        Vector2 _position;
        Vector2 _origin;

        readonly float _rotationVelocity = 3f;
        readonly float _linearVelocity = 4f;


        internal Sprite(Texture2D texture)
        {
            _texture = texture;
            _origin = new Vector2(_texture.Width / 2, _texture.Height / 2);
        }

        public virtual void Update(GameTime gametime, List<Sprite> sprites)
        {
        }

        internal virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, null, Color.White, _rotation, _origin, 1, SpriteEffects.None, 0);
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
        public float RotationVelocity
        {
            get { return _rotationVelocity; }
        }

        public float LinearVelocity
        {
            get { return _linearVelocity; }
        }

        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public float PositionX
        {
            get { return _position.X; }
            protected set { _position.X = value; }
        }

        public float PositionY
        {
            get { return _position.Y; }
            protected set { _position.Y = value; }
        }

        public Vector2 Origin
        {
            get { return _origin; }
        }
    }
}
