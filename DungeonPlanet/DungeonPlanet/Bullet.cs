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
    public class Bullet : Sprite
    {
        float _rotation;
        Vector2 _origin;
        List<Enemy> _enemys = DungeonPlanetGame.Enemys;
        List<Boss> _bosses;
        public BulletLib BulletLib { get; set; }

        public Bullet(Texture2D texture, Vector2 position, SpriteBatch spritebatch, WeaponLib ctx, List<Boss> bosses)
            : base(texture, position, spritebatch)
        {
            _origin = new Vector2(1, 12);
            _rotation = ctx.Rotation;
            base.position = new Vector2(base.position.X, base.position.Y);
            BulletLib = new BulletLib(ctx, new System.Numerics.Vector2(base.position.X, base.position.Y), texture.Height, texture.Width);
            _bosses = bosses;
        }
        public Bullet(Texture2D texture, Vector2 position, SpriteBatch spritebatch, WeaponLib ctx)
           : base(texture, position, spritebatch)
        {
            _origin = new Vector2(1, 12);
            _rotation = ctx.Rotation;
            base.position = new Vector2(base.position.X, base.position.Y);
            BulletLib = new BulletLib(ctx, new System.Numerics.Vector2(base.position.X, base.position.Y), texture.Height, texture.Width);
        }

        public void Update(GameTime gameTime)
        {
            BulletLib.Timer((float)gameTime.ElapsedGameTime.TotalSeconds);
            position += new Vector2(BulletLib.PositionUpdate().X,BulletLib.PositionUpdate().Y);
        }
        public bool HasTouchedEnemy()
        {
            foreach (Enemy enemy in _enemys)
            {
                if (new System.Drawing.Rectangle((int)position.X, (int)position.Y, Texture.Width, Texture.Height).IntersectsWith(enemy.EnemyLib.Bounds))
                {
                    if (PlayerInfo.ActualWeapon == PlayerInfo.WeaponState.None)
                    {
                        enemy.EnemyLib.State = 0;
                        enemy.EnemyLib.GotDamage();
                        enemy.EnemyLib.Life -= 10;
                    }
                    else if (PlayerInfo.ActualWeapon == PlayerInfo.WeaponState.Fire)
                    {
                        enemy.EnemyLib.State = 1;
                        enemy.EnemyLib.GotDamage();
                        enemy.EnemyLib.Life -= 10;
                    }
                    else if (PlayerInfo.ActualWeapon == PlayerInfo.WeaponState.Slime)
                    {
                        enemy.EnemyLib.State = 2;
                        enemy.EnemyLib.GotDammageWithSlim();
                        enemy.EnemyLib.Life -= 10;
                    }
                    return true;
                }
            }
            return false;
        }

        public bool HasTouchedBoss(PlayerLib playerLib)
        {
            foreach (Boss boss in _bosses)
            {
                if (new System.Drawing.Rectangle((int)position.X, (int)position.Y, Texture.Width, Texture.Height).IntersectsWith(boss.BossLib.Bounds))
                {
                    

                    if (PlayerInfo.ActualWeapon == PlayerInfo.WeaponState.None)
                    {
                        boss.BossLib.Life -= 10;
                        boss.BossLib.ReceiveDamage(playerLib);
                        boss.BossLib.State = 0;
                    }
                    else if (PlayerInfo.ActualWeapon == PlayerInfo.WeaponState.Fire)
                    {
                        boss.BossLib.Life -= 10;
                        boss.BossLib.ReceiveDamage(playerLib);
                        boss.BossLib.State = 1;
                    }
                    else if (PlayerInfo.ActualWeapon == PlayerInfo.WeaponState.Slime)
                    {
                        boss.BossLib.Life -= 10;
                        boss.BossLib.ReceiveDamage(playerLib);
                        boss.BossLib.State = 2;
                    }
                    return true;
                }
            }
            return false;
        }

        public bool HasTouchedPlayer(EnemyLib enemylib)
        {
            if (new System.Drawing.Rectangle((int)position.X, (int)position.Y, Texture.Width, Texture.Height).IntersectsWith(Player.CurrentPlayer.PlayerLib.Bounds))
            {
                enemylib.MakeDamage(Player.CurrentPlayer.PlayerLib);
                Player.CurrentPlayer.PlayerInfo.Life -= 10;
                return true;
            }
            return false;
        }

        public bool HasTouchedShield()
        {
            if (Player.CurrentPlayer.Shield.IsActive)
            {
                if(new System.Drawing.Rectangle((int)position.X, (int)position.Y, Texture.Width, Texture.Height).IntersectsWith(Player.CurrentPlayer.Shield.ShieldLib.Bounds))
                {
                    return true;
                }
            }
            return false;
        }

        public bool HasTouchedTile()
        {
            if (Level.ActualState == Level.State.Hub || Level.ActualState == Level.State.BossRoom)
            {
                foreach (var tile in Level.CurrentBoard.Hub.Tiles)
                {
                    if (tile.IsBlocked)
                    {
                        if (new System.Drawing.Rectangle((int)position.X, (int)position.Y, Texture.Width, Texture.Height).IntersectsWith(tile.Bounds))
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
            else if (Level.ActualState == Level.State.Level)
            {
                foreach (Case Case in Level.CurrentBoard.Cases)
                {
                    foreach (Tile tile in Case.Tiles)
                    {
                        if (tile.IsBlocked)
                        {
                            if (new System.Drawing.Rectangle((int)position.X, (int)position.Y, Texture.Width, Texture.Height).IntersectsWith(tile.Bounds))
                            {
                                return true;
                            }
                        }
                    }
                }
                return false;
            }
            return false;
        }
        public override void Draw()
        {
            SpriteBatch.Draw(Texture, position, null, Color.White, _rotation, _origin, 1, SpriteEffects.None, 0);
        }
    }
}
