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
        Animation _animationTank;
        Animation _animationFireWaveTank;
        Animation _animationBullet;
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
        public Bullet(Texture2D texture, Vector2 position, SpriteBatch spritebatch, float rotation, List<Boss> bosses)
            : base(texture, position, spritebatch)
        {
            _origin = new Vector2(1, 12);
            _rotation = rotation;
            System.Numerics.Vector2 direction = new System.Numerics.Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
            base.position = new Vector2(base.position.X, base.position.Y);
            BulletLib = new BulletLib(direction, new System.Numerics.Vector2(base.position.X, base.position.Y), texture.Height, texture.Width);
            _bosses = bosses;
        }
        public Bullet(Texture2D texture, Texture2D textureFireWave, Vector2 position, SpriteBatch spritebatch, float rotation, List<Boss> bosses)
            : base(texture, position, spritebatch)
        {
            _animationBullet = new Animation();
            _animationBullet.Initialize(texture, position, 85, 12, 0, 0, 5, 75, Color.White, 1, true, false);
            _animationBullet.Effect = SpriteEffects.FlipHorizontally;
            _animationFireWaveTank = new Animation();
            _animationFireWaveTank.Initialize(textureFireWave, position, 37, 120, 0, 0, 6, 25, Color.White, 1, false, false);
            _origin = new Vector2(1, 12);
            _rotation = rotation;
            System.Numerics.Vector2 direction = new System.Numerics.Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
            base.position = new Vector2(base.position.X, base.position.Y);
            BulletLib = new BulletLib(direction, new System.Numerics.Vector2(base.position.X, base.position.Y), 8, 30);
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
        public Bullet(Texture2D texture, Texture2D textureFireWave, Vector2 position, SpriteBatch spritebatch, Vector2 distance)
          : base(texture, position, spritebatch)
        {
            _animationTank = new Animation();
            _animationTank.Initialize(texture, position, 85, 12, 0, 0, 5, 75, Color.White, 2, true, false);
            _animationTank.Effect = SpriteEffects.FlipHorizontally;
            _origin = new Vector2(1, 12);
            _rotation = (float)Math.Atan2(distance.Y, distance.X);
            System.Numerics.Vector2 direction = new System.Numerics.Vector2((float)Math.Cos(_rotation), (float)Math.Sin(_rotation));
            base.position = new Vector2(base.position.X, base.position.Y);
            _animationFireWaveTank = new Animation();
            _animationFireWaveTank.Initialize(textureFireWave, position, 37, 120, 0, 0, 6, 25, Color.White, 2, false, false);
            BulletLib = new BulletLib(direction, new System.Numerics.Vector2(base.position.X, base.position.Y), 12, 85);
        }
        

        public void Update(GameTime gameTime)
        {
            BulletLib.Timer((float)gameTime.ElapsedGameTime.TotalSeconds);
            position += new Vector2(BulletLib.PositionUpdate().X,BulletLib.PositionUpdate().Y);
            if (_animationTank != null)
            {
                _animationTank.Update(gameTime);
                _animationTank.Position = new Vector2(position.X +200, position.Y +30); ;
                _animationFireWaveTank.Update(gameTime);
                _animationFireWaveTank.Position = new Vector2(position.X, position.Y-60);
            }
            if(_animationBullet != null)
            {
                _animationBullet.Update(gameTime);
                _animationBullet.Position = new Vector2(position.X + 125, position.Y+15); ;
                _animationFireWaveTank.Update(gameTime);
                _animationFireWaveTank.Position = new Vector2(position.X, position.Y -10);
            }
        }
        public bool HasTouchedEnemy()
        {
            foreach (var enemy in _enemys)
            {
                if (new System.Drawing.Rectangle((int)position.X, (int)position.Y, Texture.Width, Texture.Height).IntersectsWith(enemy.EnemyLib.Bounds))
                {
                    if (_animationBullet != null)
                    {
                        enemy.EnemyLib.Life -= 40;
                    }
                    if (PlayerInfo.ActualBullet == PlayerInfo.BulletState.None)
                    {
                        enemy.EnemyLib.State = 0;
                        enemy.EnemyLib.GotDamage();
                        enemy.EnemyLib.Life -= 10;
                    }
                    else if (PlayerInfo.ActualBullet == PlayerInfo.BulletState.Fire)
                    {
                        enemy.EnemyLib.State = 1;
                        enemy.EnemyLib.GotDamage();
                        enemy.EnemyLib.Life -= 10;
                    }
                    else if (PlayerInfo.ActualBullet == PlayerInfo.BulletState.Slime)
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

                    if (_animationBullet != null)
                    {
                        boss.BossLib.Life -= 40;
                    }
                    if (PlayerInfo.ActualBullet == PlayerInfo.BulletState.None)
                    {
                        boss.BossLib.Life -= 10;
                        boss.BossLib.ReceiveDamage(playerLib);
                        boss.BossLib.State = 0;
                    }
                    else if (PlayerInfo.ActualBullet == PlayerInfo.BulletState.Fire)
                    {
                        boss.BossLib.Life -= 10;
                        boss.BossLib.ReceiveDamage(playerLib);
                        boss.BossLib.State = 1;
                    }
                    else if (PlayerInfo.ActualBullet == PlayerInfo.BulletState.Slime)
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

        public bool HasTouchedPlayer(BossLib bossLib)
        {
            if (new System.Drawing.Rectangle((int)position.X, (int)position.Y, Texture.Width, Texture.Height).IntersectsWith(Player.CurrentPlayer.PlayerLib.Bounds))
            {
                bossLib.MakeDamage(Player.CurrentPlayer.PlayerLib);
                Player.CurrentPlayer.PlayerInfo.Life -= 20;
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
                        if(_animationTank != null || _animationBullet != null)
                        {
                            if (new System.Drawing.Rectangle((int)position.X, (int)position.Y, 85, 12).IntersectsWith(tile.Bounds))
                            {
                                return true;
                            }
                        }
                        else
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
            if(_animationTank!= null)
            {
                _animationTank.Draw(SpriteBatch, _rotation);
                _animationFireWaveTank.Draw(SpriteBatch);
            }
            else if(_animationBullet != null)
            {
                _animationBullet.Draw(SpriteBatch, _rotation);
                _animationFireWaveTank.Draw(SpriteBatch);
            }
            else
            {
                SpriteBatch.Draw(Texture, position, null, Color.White, _rotation, _origin, 1, SpriteEffects.None, 0);
            }
        }
    }
}
