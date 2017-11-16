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
    class MediPack : Sprite
    {
        int _heal;
        MediPackLib _mediPackLib;
        Player _player;
        public MediPack(Texture2D texture, Vector2 position, SpriteBatch spritebatch, int heal, Player player)
            : base(texture, position, spritebatch)
        {
            _heal = heal;
            _player = player;
            _mediPackLib = new MediPackLib(new System.Numerics.Vector2(position.X, position.Y), texture.Width, texture.Height, player.PlayerLib);
        }
        public int Heal
        {
            get { return _heal; }
        }
        public bool IsFinished { get; set; }
        public void Update()
        {
            if (_mediPackLib.PlayerIntersect())
            {
                _player.Life+=Heal;
                IsFinished = true;
            }
        }

    }
}
