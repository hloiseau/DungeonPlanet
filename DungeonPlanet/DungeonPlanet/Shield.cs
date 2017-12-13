using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DungeonPlanet.Library;

namespace DungeonPlanet
{
    public class Shield : Item
    {
        int Life { get; set; }
        public bool Activate { get; set; }
        List<Enemy> _enemys;
        ItemLib ShieldLib { get; set; }

        public Shield(Texture2D texture, Vector2 position, SpriteBatch spritebatch, Player player, List<Enemy> enemys)
            : base(texture, position, spritebatch, player)
        {
            _enemys = enemys;
            ShieldLib = new ItemLib(new System.Numerics.Vector2(player.position.X - 20, player.position.Y - 20), texture.Width, texture.Height, player.PlayerLib);
        }

        public override void Update(GameTime gameTime)
        {
            position = new Vector2(Player.position.X - 20, Player.position.Y - 20);
            ShieldLib.Position = new System.Numerics.Vector2(position.X, position.Y);
            CheckStatusAndProtect();
        }

        private void CheckStatusAndProtect()
        {
            foreach (Enemy enemy in _enemys)
            {
                if (Player.Shield.Activate)
                {
                    if (ShieldLib.EnemyIntersect(enemy.EnemyLib))
                    {
                        enemy.EnemyLib.MakeDamage(Player.PlayerLib);
                    }
                }
            }
        }
    }
}