using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DungeonPlanet.Library;

namespace DungeonPlanet
{
    public class Item : Sprite
    {

        protected ItemLib _itemLib;
        public bool IsFinished { get; set; }
        protected Player Player { get; set; }

        public Item(Texture2D texture, Vector2 position, SpriteBatch spritebatch, Player player)
            : base(texture, position, spritebatch)
        {
            Player = player;
            _itemLib = new ItemLib(new System.Numerics.Vector2(position.X, position.Y), texture.Width, texture.Height, player.PlayerLib);
        }

        public virtual void Update(GameTime gameTime)
        {
            _itemLib.AffectWithGravity();
            _itemLib.MoveAsFarAsPossible((float)gameTime.ElapsedGameTime.TotalMilliseconds / 15);
            _itemLib.StopMovingIfBlocked();
            position = new Vector2(_itemLib.Position.X, _itemLib.Position.Y);
        }
    }
}
