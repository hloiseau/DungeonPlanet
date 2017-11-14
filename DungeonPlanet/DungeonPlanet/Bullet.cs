using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DungeonPlanet.Library;

namespace DungeonPlanet
{
    public class Bullet : Sprite
    {
        float _rotation;
        Vector2 _origin;

        public BulletLib BulletLib { get; set; }
        public Bullet(Texture2D texture, Vector2 position, SpriteBatch spritebatch, WeaponLib ctx)
            : base(texture, position, spritebatch)
        {
            _origin = new Vector2(1, 12);
            _rotation = ctx.Rotation;
            BulletLib = new BulletLib(ctx, new System.Numerics.Vector2(Position.X, Position.Y),texture.Height,texture.Width);
        }
        public void Update(GameTime gameTime)
        {
            BulletLib.Timer((float)gameTime.ElapsedGameTime.TotalSeconds);
            BulletLib.IsDead();
            Position += new Vector2(BulletLib.PositionUpdate().X,BulletLib.PositionUpdate().Y);
        }
        public override void Draw()
        {
            SpriteBatch.Draw(Texture, Position, null, Color.White, _rotation, _origin, 1, SpriteEffects.None, 0);
        }
    }
}
