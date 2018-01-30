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
    class Bomb : Item
    {

        int _count;
        List<Enemy> _enemy;
        Animation _sfx;
        bool _boom;
        public Bomb(Texture2D texture, Texture2D sfx, Vector2 position, SpriteBatch spritebatch, int heal, Player player, List<Enemy> enemys)
            : base(texture, position, spritebatch, player)
        {
            _enemy = enemys;
            _count = 0;
            _sfx = new Animation();
            _sfx.Initialize(sfx, position, 95, 100, 0, 0, 22, 50, Color.White, 1, false, false);
        }

        public override void Update(GameTime gameTime)
        {
            CheckStatusAndMovement();
            if (_boom)
            {
                _sfx.Position = position;
                _sfx.Update(gameTime);
            }

            base.Update(gameTime);
            ItemLib.SimulateFriction();
        }

        private void CheckStatusAndMovement()
        {
            if (_count <= 150)
            {
                _count++;
            }
            else if (_count <= 151 && _count > 150)
            {
                if (ItemLib.BombRadius.IntersectsWith(Player.PlayerLib.Bounds)) { Player.PlayerInfo.Life -= 20; }
                foreach (Enemy enemy in _enemy)
                {
                    if (ItemLib.BombRadius.IntersectsWith(enemy.EnemyLib.Bounds)) { enemy.EnemyLib.Life -= 20; }
                }
                _count++;
                _boom = true;
            }
            else
            {
                if (!_sfx.IsActive)
                {
                    IsFinished = true;
                }
            }
        }

        public override void Draw()
        {
            if (_boom)
            {
                _sfx.Draw(SpriteBatch);
            }
            else
            base.Draw();
        }
    }
}
