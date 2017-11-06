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
    public class Ship : Sprite
    {
        public Bullet Bullet;
        public Ship(Texture2D texture) : base(texture)
        {

        }

        public override void Update(GameTime gameTime, List<Sprite> sprites)
        {
            _previousKey = _currentKey;
            _currentKey = Keyboard.GetState();
            _currentMouse = Mouse.GetState();

             Direction = new Vector2(1, (_currentMouse.Y - Position.Y)/ 500);

            /*
            if (_currentKey.IsKeyDown(Keys.A))
            {
                _rotation -= MathHelper.ToRadians(RotationVelocity);
            }
            if (_currentKey.IsKeyDown(Keys.E))
            {
                _rotation += MathHelper.ToRadians(RotationVelocity);
            }
            */
            if (_currentKey.IsKeyDown(Keys.D) || _currentKey.IsKeyDown(Keys.Right))
            {
                Position.X += LinearVelocity;
            }
            if (_currentKey.IsKeyDown(Keys.Q) || _currentKey.IsKeyDown(Keys.Left))
            {
                Position.X -= LinearVelocity;
            }
            if (_currentKey.IsKeyDown(Keys.S) || _currentKey.IsKeyDown(Keys.Down))
            {
                Position.Y += LinearVelocity;
            }
            if (_currentKey.IsKeyDown(Keys.Z) || _currentKey.IsKeyDown(Keys.Up))
            {
                Position.Y -= LinearVelocity;
            }

            //shooting is here
            if (_currentKey.IsKeyDown(Keys.Space) &&
                _previousKey.IsKeyUp(Keys.Space))
            {
                AddBullet(sprites);
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