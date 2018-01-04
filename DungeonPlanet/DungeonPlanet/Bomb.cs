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

        public Bomb(Texture2D texture, Vector2 position, SpriteBatch spritebatch, int heal, Player player, List<Enemy> enemys)
            : base(texture, position, spritebatch, player)
        {
            _enemy = enemys;
            _count = 0;
        }

        public override void Update(GameTime gameTime)
        {
            CheckStatusAndMovement();
            base.Update(gameTime);
            ItemLib.SimulateFriction();
        }

        private void CheckStatusAndMovement()
        {
            if (_count <= 300)
            {
                _count++;
            }
            else if (_count <= 301 && _count > 300)
            {
                if (ItemLib.BombRadius.IntersectsWith(Player.PlayerLib.Bounds)) { Player.Life -= 20; }
                foreach (Enemy enemy in _enemy)
                {
                    if (ItemLib.BombRadius.IntersectsWith(enemy.EnemyLib.Bounds)) { enemy.EnemyLib.Life -= 20; }
                }
                _count++;
            }
            else
            {
                IsFinished = true;
            }
        }
    }
}
