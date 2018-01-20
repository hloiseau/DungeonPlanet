using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DungeonPlanet.Library;
using GeonBit.UI.Entities;
using GeonBit.UI;

namespace DungeonPlanet
{
    public class SpriteObject : Sprite
    {
        SpriteBatch _spritebatch;
        Animation _animation;
        Header _header;
        EnemyLib ReferenceLib { get; set; }

        public SpriteObject(Texture2D texture, Vector2 position, SpriteBatch batch, int frameWidth, int frameHeight, int frameCount, int frameTime)
            : base(texture, position, batch)
        {
            ReferenceLib = new EnemyLib(new System.Numerics.Vector2(position.X, position.Y), texture.Width, texture.Height, 100);
            _animation = new Animation();
            _animation.Initialize(texture, position, frameWidth, frameHeight, 0, 0, frameCount, frameTime, Color.White, 1, true, true);
            _spritebatch = batch;
        }

        public void Update(GameTime gameTime)
        {
            _animation.Update(gameTime);
            _animation.Position = new Vector2(ReferenceLib.Position.X, ReferenceLib.Position.Y+10);

            ReferenceLib.AffectWithGravity();
            ReferenceLib.SimulateFriction();
            ReferenceLib.MoveAsFarAsPossible((float)gameTime.ElapsedGameTime.TotalMilliseconds / 15);
            ReferenceLib.StopMovingIfBlocked();
            position = new Vector2(ReferenceLib.Position.X, ReferenceLib.Position.Y);
        }
    
        public override void Draw()
        {
            _animation.Draw(SpriteBatch);
        }
    }
}
