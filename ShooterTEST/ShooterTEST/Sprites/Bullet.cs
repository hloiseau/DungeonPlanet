using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShooterTEST.Sprites
{
    public class Bullet : Sprite
    {
        Weapon _context;
        private float _timer;
        public Bullet(Texture2D Texture) : base(Texture)
        {
            Origin = new Vector2(1, 12);
        }

        public Bullet(Texture2D Texture, Weapon context) : base(Texture)
        {
            _context = context;
            Origin = new Vector2(1, 12);
        }
        public override void Update(GameTime gameTime, List<Sprite> sprites)
        {
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_timer > LifeSpan) IsRemooved = true;

            Position += Direction * LinearVelocity;
        }
        //public BulletRotation{ _context.rotation; }
    }
}
