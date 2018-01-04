using DungeonPlanet.Library;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonPlanet
{
    class JetPack : Sprite
    {

        JetPackLib _JetPackLib;

        public JetPack(Texture2D texture, Vector2 position, SpriteBatch spritebatch, Player player)
            :base(texture, position, spritebatch)
        {
            _JetPackLib = new JetPackLib(new System.Numerics.Vector2(position.X, position.Y), texture.Width, texture.Height, player.PlayerLib);
        }

        public bool IsEquiped { get; set; }

        public void Update()
        {
            if (_JetPackLib.PlayerIntersect())
            {
                IsEquiped = true;
            }
        }
    }
}
