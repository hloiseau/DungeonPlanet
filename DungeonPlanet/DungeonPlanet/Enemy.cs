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

        public Weapon WeaponA { get; set; }
        List<Bullet> Bullets { get; }
        public Texture2D _textureBullet;
        public Bullet bullet;
        public Texture2D _textureWeapon;
        public SpriteBatch _spritebatch;
        public DungeonPlanetGame _ctx;
        
        Player _player;
        int _count;
        string _type;

        public Enemy(Texture2D texture, Vector2 position, SpriteBatch spritebatch, string type)
            : base(texture, position, spritebatch)
        {
            EnemyLib = new EnemyLib(new System.Numerics.Vector2(position.X, position.Y), texture.Width, texture.Height, 30);
            _player = Player.CurrentPlayer;
            PlayerLib = Player.CurrentPlayer.PlayerLib;
            _count = 0;
            _type = type;
            _spritebatch = spritebatch;
        }
        public Enemy(Texture2D texture, Vector2 position, SpriteBatch spritebatch, string type, Texture2D textureWeapon, Texture2D textureBullet, DungeonPlanetGame ctx, Shield shield)
           : base(texture, position, spritebatch)
        {
            EnemyLib = new EnemyLib(new System.Numerics.Vector2(position.X, position.Y), texture.Width, texture.Height, 30);
            _player = Player.CurrentPlayer;
            PlayerLib = Player.CurrentPlayer.PlayerLib;
            _count = 0;
            _type = type;
            _spritebatch = spritebatch;
            _textureWeapon = textureWeapon;
            _textureBullet = textureBullet;
            _ctx = ctx;
            WeaponA = new Weapon(_textureWeapon, _textureBullet, _ctx, base.position, _spritebatch, EnemyLib, shield);
        }

        public void Update(GameTime gameTime)
        {
            if (_ctx != null) EnemyLib.Update(EnemyLib.Position.X - _ctx.GraphicsDevice.Viewport.Width / 2, EnemyLib.Position.Y - _ctx.GraphicsDevice.Viewport.Height / 2);
            CheckKeyboardAndUpdateMovement(gameTime);
            EnemyLib.AffectWithGravity();
            EnemyLib.SimulateFriction();
            EnemyLib.MoveAsFarAsPossible((float)gameTime.ElapsedGameTime.TotalMilliseconds / 40);
            EnemyLib.StopMovingIfBlocked();
            position = new Vector2(EnemyLib.Position.X, EnemyLib.Position.Y);
            EnemyLib.Life = MathHelper.Clamp(EnemyLib.Life, 0, 100);
            if (WeaponA != null) WeaponA.Update(gameTime);
        }

        private void CheckKeyboardAndUpdateMovement(GameTime gameTime)
        {
            if (EnemyLib.Vision.IntersectsWith(PlayerLib.Bounds))
            {
                if (_type == "CQC")
                {
                    /*if (EnemyLib.Bounds.IntersectsWith(_shield._itemLib.ShieldRadius))
                    {
                        EnemyLib.GotDamage();
                    }*/
                    if (EnemyLib.Bounds.IntersectsWith(PlayerLib.Bounds))
                    {
                        _player.Life -= 10;
                        EnemyLib.MakeDamage(PlayerLib);
                    }
                    if (EnemyLib.GetDistanceTo(PlayerLib.Position).X < 0.1) { EnemyLib.Left(); }
                    if (EnemyLib.GetDistanceTo(PlayerLib.Position).X > 0.1) { EnemyLib.Right(); }
                }
                else if (_type == "DIST")
                {
                    EnemyLib.IsShooting = true;
                    if (EnemyLib.Bounds.IntersectsWith(PlayerLib.Bounds))
                    {
                        _player.Life -= 1;
                        EnemyLib.MakeDamage(PlayerLib);
                    }
                }
            }
            else
            {
                EnemyLib.IsShooting = false;
                if (_count <= 100)
                {
                    EnemyLib.Left();
                    _count++;
                }
                else if (_count <= 200 && _count > 100)
                {
                    EnemyLib.Right();
                    _count++;
                }
                else
                {
                    _count = 0;
                }
            }
        }
        public override void Draw()
        {
            base.Draw();
            if (WeaponA != null) WeaponA.Draw();
        }
    }
}