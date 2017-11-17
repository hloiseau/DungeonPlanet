using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using DungeonPlanet.Library;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonPlanet
{
    public class Enemy : Sprite
    {
        public EnemyLib EnemyLib { get; set; }
        public PlayerLib PlayerLib { get; set; }

        Player _player;

        public Enemy(Texture2D texture, Vector2 position, SpriteBatch spritebatch)
            : base(texture, position, spritebatch)
        {
            EnemyLib = new EnemyLib(new System.Numerics.Vector2(position.X, position.Y), texture.Width, texture.Height, 30);
            _player = Player.CurrentPlayer;
            PlayerLib = Player.CurrentPlayer.PlayerLib;
        }

        public void Update(GameTime gameTime)
        {
            CheckKeyboardAndUpdateMovement();
            EnemyLib.AffectWithGravity();
            EnemyLib.SimulateFriction();
            EnemyLib.MoveAsFarAsPossible((float)gameTime.ElapsedGameTime.TotalMilliseconds / 40);
            EnemyLib.StopMovingIfBlocked();
            Position = new Vector2(EnemyLib.Position.X, EnemyLib.Position.Y);
            EnemyLib.Life = MathHelper.Clamp(EnemyLib.Life, 0, 100);
        }

        private void CheckKeyboardAndUpdateMovement()
        {
            if (EnemyLib.Vision.IntersectsWith(PlayerLib.Bounds))
            {
                if (EnemyLib.Bounds.IntersectsWith(PlayerLib.Bounds))
                {
                    _player.Life -= 10;
                    EnemyLib.MakeDamage(PlayerLib);
                }
                if (EnemyLib.GetDistanceTo(PlayerLib.Position).X < 0.1) { EnemyLib.Left(); }
                if (EnemyLib.GetDistanceTo(PlayerLib.Position).X > 0.1) { EnemyLib.Right(); }
                /*if (EnemyLib.GetDistanceTo(PlayerLib.Position).X == 0) { EnemyLib.Position = PlayerLib.Position; };*/

            }
        }
    }
}