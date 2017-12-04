using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonPlanet
{
    public class Shield : Item
    {
        int Life { get; set; }
        public bool Activate { get; set; }

        public Shield(Texture2D texture, Vector2 position, SpriteBatch spritebatch, Player player)
            : base(texture, position, spritebatch, player)
        {
            Life = 60;
        }

        public override void Update(GameTime gameTime)
        {
            CheckStatusAndUpdateMovement();
            base.Update(gameTime);
            position = new Vector2(Player.position.X - 15, Player.position.Y - 20);
        }

        private void CheckStatusAndUpdateMovement()
        {
        }
    }
}