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
        public Sprite Fire { get; set; }
        int _count;

        Player _player;

        public Boss(Texture2D texture, Vector2 position, SpriteBatch spritebatch, Texture2D fireBossTexture)
            : base(texture, position, spritebatch)
        {
            BossLib = new BossLib(new System.Numerics.Vector2(position.X, position.Y), texture.Width, texture.Height, 200);
            _player = Player.CurrentPlayer;
            PlayerLib = Player.CurrentPlayer.PlayerLib;
            _count = 0;
            Fire = new Sprite(fireBossTexture, new Vector2(BossLib.Position.X, BossLib.Position.Y), spritebatch);
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

            if (BossLib != null && Fire != null) Fire.position = new Vector2(BossLib.Position.X, BossLib.Position.Y);
        }

        private void CheckKeyboardAndUpdateMovement()
        {
            if (BossLib.Vision.IntersectsWith(PlayerLib.Bounds))
            {
                if (BossLib.Bounds.IntersectsWith(PlayerLib.Bounds))
                {
                    
                    _player.PlayerInfo.Life -= 25;
                    BossLib.MakeDamage(PlayerLib);
                }
                if (BossLib.GetDistanceTo(PlayerLib.Position).X < 0.1 && BossLib.State != 2) { BossLib.Left(); }
                else { BossLib.LeftSlim(); }
            }
            else
            {
                if(BossLib.State != 2) BossLib.Right();
                else { BossLib.RightSlim(); }
            }

            if (_count <= 50) _count++;
            else
            {
                _count = 0;
                if (BossLib.State == 1) BossLib.Life -= 10;
            }
        }
        public override void Draw()
        {
            base.Draw();
            if (BossLib.State == 1) Fire.Draw();
        }
    }
}
