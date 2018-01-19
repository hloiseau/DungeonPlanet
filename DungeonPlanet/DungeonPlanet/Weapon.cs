using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using DungeonPlanet.Library;

namespace DungeonPlanet
{
    public class Weapon : Sprite
    {
        EnemyLib _enemyLib;
        internal WeaponLib WeaponLib { get; set; }
        Vector2 _origin;
        MouseState _currentMouse;
        public List<Bullet> Bullets { get; }
        public List<Bullet> BulletsEnemy { get; }
        Texture2D _bulletTexture;
        int _time;
        DungeonPlanetGame _ctx;
        List<Boss> _bosses;
        SpriteEffects _effect;

        public Weapon(Texture2D weaponTexture, Texture2D bulletTexture, DungeonPlanetGame ctx, Vector2 position, SpriteBatch spritebatch, List<Boss> bosses)
            : base(weaponTexture, position, spritebatch)
        {
            _origin = new Vector2(-2, 7);
            WeaponLib = new WeaponLib();
            Bullets = new List<Bullet>();
            BulletsEnemy = new List<Bullet>();
            _time = 0;
            _ctx = ctx;
            _bulletTexture = bulletTexture;
            _bosses = bosses;
        }
        public Weapon(Texture2D weaponTexture, Texture2D bulletTexture, DungeonPlanetGame ctx, Vector2 position, SpriteBatch spritebatch, EnemyLib enemyLib)
           : base(weaponTexture, position, spritebatch)
        {
            _enemyLib = enemyLib;
            _origin = new Vector2(-2, 7);
            WeaponLib = new WeaponLib();
            Bullets = new List<Bullet>();
            BulletsEnemy = new List<Bullet>();
            _time = 0;
            _ctx = ctx;
            _bulletTexture = bulletTexture;            
        }

        public void Update(GameTime gameTime)
        {
            CheckMouseAndUpdateMovement();
            BulletUpdate(gameTime);
            if (_enemyLib != null)
            {
                if (_enemyLib.IsShooting) ShootingEnemy();
                position = new Vector2(_enemyLib.Position.X, _enemyLib.Position.Y);
                WeaponLib.Update(PlayerLib.Position.X - position.X, (PlayerLib.Position.Y + 15) - position.Y);
            }
            else
            {
                position = new Vector2(PlayerLib.Position.X + 23 , PlayerLib.Position.Y + 25);
                if (Player.CurrentPlayer.Animation.Effect == SpriteEffects.FlipHorizontally) _effect = SpriteEffects.FlipVertically;
                else _effect = SpriteEffects.None;
                WeaponLib.Update(_currentMouse.X - _ctx.GraphicsDevice.Viewport.Width / 2, _currentMouse.Y - _ctx.GraphicsDevice.Viewport.Height / 2);
            }
        }

        private void CheckMouseAndUpdateMovement()
        {
            _currentMouse = Mouse.GetState();

            Bullet bullet;

            if (_currentMouse.LeftButton == ButtonState.Pressed)
            {
                if (_enemyLib == null)
                {
                    if (_time >= 15)
                    {
                        bullet = new Bullet(_bulletTexture, position, SpriteBatch, WeaponLib, _bosses);
                        Bullets.Add(bullet);
                        _time = 0;
                    }
                    else
                    {
                        _time += 1;
                    }
                }
            }
        }

        public void ShootingEnemy()
        {
            Bullet bulletE;

            if (_time >= 15)
            {
                bulletE = new Bullet(_bulletTexture, position, SpriteBatch, WeaponLib);
                BulletsEnemy.Add(bulletE);
                _time = 0;
            }
            else
            {
                _time += 1;
            }
        }

        private void BulletUpdate(GameTime gameTime)
        {
            for (int i = 0; i < Bullets.Count; i++)
            {
                if (Bullets[i].BulletLib.IsDead() || Bullets[i].HasTouchedEnemy() || Bullets[i].HasTouchedTile() || Bullets[i].HasTouchedBoss(Player.CurrentPlayer.PlayerLib))
                {
                    Bullets.Remove(Bullets[i]);
                }
                else
                {
                    Bullets[i].Update(gameTime);
                }
            }

            for (int i = 0; i < BulletsEnemy.Count; i++)
            {
                if (BulletsEnemy[i].BulletLib.IsDead() || BulletsEnemy[i].HasTouchedPlayer(_enemyLib) || BulletsEnemy[i].HasTouchedTile() || BulletsEnemy[i].HasTouchedShield())
                {
                    BulletsEnemy.Remove(BulletsEnemy[i]);
                }
                else
                {
                    BulletsEnemy[i].Update(gameTime);
                }
            }
        }

        public override void Draw()
        {
            foreach (Bullet bullet in Bullets)
            {
                bullet.Draw();
            }
            foreach (Bullet bulletE in BulletsEnemy)
            {
                bulletE.Draw();
            }
            SpriteBatch.Draw(Texture, position, null, Color.White, WeaponLib.Rotation, _origin, 1, _effect, 0);

        }
    }
}
