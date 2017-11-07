using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ShooterTEST.Sprites
{
    public class Weapon : Sprite
    {
        public Bullet Bullet;
        bool _jumping;
        float _jumpVelocity;
        Game1 _game;
        float _shootingTime = 0;


        public Weapon(Texture2D texture, Game1 game) : base(texture)
        {
            _game = game;
            Origin = new Vector2(6, 30);
        }

        public override void Update(GameTime gameTime, List<Sprite> sprites)
        {
            _previousKey = _currentKey;
            _currentKey = Keyboard.GetState();
            _currentMouse = Mouse.GetState();

            distance.X = _currentMouse.X - Position.X;
            distance.Y = _currentMouse.Y - Position.Y;

            Direction = new Vector2((float)Math.Cos(_rotation), (float)Math.Sin(_rotation));

            _rotation = (float)Math.Atan2(distance.Y, distance.X);

            if (_currentKey.IsKeyDown(Keys.D) || _currentKey.IsKeyDown(Keys.Right))
            {
                Position.X += LinearVelocity;
            }
            if (_currentKey.IsKeyDown(Keys.Q) || _currentKey.IsKeyDown(Keys.Left))
            {
                Position.X -= LinearVelocity;
            }

            if (_jumping)
            {
                Position.Y += _jumpVelocity; //Making it go up
                _jumpVelocity += 1;
                if (Position.Y >= _game.ScreenHeight)
                {
                    Position.Y = _game.ScreenHeight; //Then set it on the ground
                    _jumping = false;
                }
            }
            else
            {
                if (_currentKey.IsKeyDown(Keys.Space))
                {
                    _jumping = true;
                    _jumpVelocity = -25; //Give it upward thrust
                }
            }

            Position.X = MathHelper.Clamp(Position.X, 28, _game.ScreenWidth - 28);

            Position.Y = MathHelper.Clamp(Position.Y, 63, _game.ScreenHeight - 63);

            //shooting is here
            if (_currentMouse.LeftButton == ButtonState.Pressed)
            {
                if (_shootingTime >= 15)
                {
                    AddBullet(sprites);
                    _shootingTime = 0;
                }
                else
                {
                    _shootingTime += 1;
                }
            }
        }
        private void AddBullet(List<Sprite> sprites)
        {
            var bullet = Bullet.Clone() as Bullet;
            bullet.Direction = this.Direction;
            bullet.Position = this.Position;
            bullet.LinearVelocity = this.LinearVelocity * 2;
            bullet.LifeSpan = 2f;
            bullet.Parent = this;

            sprites.Add(bullet);
        }
    }
}