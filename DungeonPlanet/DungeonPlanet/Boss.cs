using DungeonPlanet.Library;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Comora;

namespace DungeonPlanet
{
    public class Boss : Sprite
    {
        public BossLib BossLib { get; set; }
        public PlayerLib PlayerLib { get; set; }

        Player _player;

        public Boss(Texture2D texture, Vector2 position, SpriteBatch spritebatch)
            : base(texture, position, spritebatch)
        {
            BossLib = new BossLib(new System.Numerics.Vector2(position.X, position.Y), texture.Width, texture.Height, 200);
            _player = Player.CurrentPlayer;
            PlayerLib = Player.CurrentPlayer.PlayerLib;
        }

        public void Update(GameTime gameTime)
        {
            CheckKeyboardAndUpdateMovement();
            BossLib.AffectWithGravity();
            BossLib.SimulateFriction();
            BossLib.MoveAsFarAsPossible((float)gameTime.ElapsedGameTime.TotalMilliseconds / 40);
            BossLib.StopMovingIfBlocked();
            BossLib.IsDead();
            position = new Vector2(BossLib.Position.X, BossLib.Position.Y);
            BossLib.Life = MathHelper.Clamp(BossLib.Life, 0, 200);
        }

        private void CheckKeyboardAndUpdateMovement()
        {
            if (BossLib.Vision.IntersectsWith(PlayerLib.Bounds))
            {
                if (BossLib.Bounds.IntersectsWith(PlayerLib.Bounds))
                {
                    _player.Life -= 15;
                    BossLib.MakeDamage(PlayerLib);
                }
                if (BossLib.GetDistanceTo(PlayerLib.Position).X < 0.1) { BossLib.Left(); }
            }
            else
            {
                BossLib.Right();
            }
        }
    }
}
