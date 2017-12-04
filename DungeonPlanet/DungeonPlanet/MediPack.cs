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
    class MediPack : Item
    {
        int _heal;

        public MediPack(Texture2D texture, Vector2 position, SpriteBatch spritebatch, int heal, Player player)
            : base(texture, position, spritebatch, player)
        {
            _heal = heal;
        }
        public int Heal
        {
            get { return _heal; }
        }
        public override void Update(GameTime gameTime)
        {
            CheckStatusAndUpdateMovement();
            base.Update(gameTime);
        }

        private void CheckStatusAndUpdateMovement()
        {
            if (_itemLib.PlayerIntersect())
            {
                Player.Life += Heal;
                IsFinished = true;
            }
        }
    }
}
