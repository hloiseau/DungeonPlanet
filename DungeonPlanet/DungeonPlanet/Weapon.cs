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
        PlayerLib _playerLib;
        internal WeaponLib WeaponLib { get; set; }
        Vector2 _origin;
        MouseState _currentMouse;
        List<Bullet> Bullets { get; }
        Texture2D _bulletTexture;
        int _time;
        DungeonPlanetGame _ctx;
        List<Enemy> _enemys;

        public Weapon(Texture2D weaponTexture, Texture2D bulletTexture, DungeonPlanetGame ctx,Vector2 position, SpriteBatch spritebatch, PlayerLib playerLib, List<Enemy> enemys)
            : base(weaponTexture, position, spritebatch)
        {
            _playerLib = playerLib;
            _origin = new Vector2(5, 20);
            Position = new Vector2(_playerLib.Position.X + 28, _playerLib.Position.Y + 28);
            WeaponLib = new WeaponLib();
            Bullets = new List<Bullet>();
            _time = 0;
            _ctx = ctx;
            _bulletTexture = bulletTexture;
            _enemys = enemys;
        }

        public void Update(GameTime gameTime)
        {
            CheckMouseAndUpdateMovement();
            BulletUpdate(gameTime);
            Position = new Vector2(_playerLib.Position.X + 28, _playerLib.Position.Y + 28);

            WeaponLib.Update(_currentMouse.X - _ctx.GraphicsDevice.Viewport.Width/2, _currentMouse.Y - _ctx.GraphicsDevice.Viewport.Height / 2);

        }

        private void CheckMouseAndUpdateMovement()
        {
            _currentMouse = Mouse.GetState();
            
            Bullet bullet;


            if (_currentMouse.LeftButton == ButtonState.Pressed)
            {
                if(_time >= 15)
                {
                    bullet = new Bullet(_bulletTexture, Position, SpriteBatch, WeaponLib, _enemys);
                    Bullets.Add(bullet);
                    _time = 0;
                }
                else
                {
                    _time += 1;
                }
            }
        }
        private void BulletUpdate(GameTime gameTime)
        {
            for (int i = 0; i < Bullets.Count; i++)
            {
                if (Bullets[i].BulletLib.IsDead() || Bullets[i].HasTouchedEnemy() || Bullets[i].HasTouchedTile())
                {
                    Bullets.Remove(Bullets[i]);
                }
                else
                {
                    Bullets[i].Update(gameTime);
                }
            }
        }

        public override void Draw()
        {
            SpriteBatch.Draw(Texture, Position, null, Color.White, WeaponLib.Rotation, _origin, 1, SpriteEffects.None, 0);
            foreach(Bullet bullet in Bullets)
            {
                bullet.Draw();
            }
        }
    }
}
