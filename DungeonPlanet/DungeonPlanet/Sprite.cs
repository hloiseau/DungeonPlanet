using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonPlanet
{
    public class Sprite
    {
        public Vector2 position { get; set; }
        public Texture2D Texture { set; get; }
        public SpriteBatch SpriteBatch { get; set; }

        public Sprite(Texture2D texture, Vector2 position, SpriteBatch batch)
        {
            Texture = texture;
            this.position = position;
            SpriteBatch = batch;
        }

        public virtual void Draw()
        {
            SpriteBatch.Draw(Texture, position, Color.White);
        }
    }
}