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

        public Sprite Fire { get; set; }

        Player _player;
        int _count;
        string _type;
        Animation _animation;

        public Enemy(Texture2D texture, Vector2 position, SpriteBatch spritebatch, string type, Texture2D fireTexture, int frameWidth, int frameHeight,int numberLin, int frameCount, int frameTime, DungeonPlanetGame ctx)
            : base(texture, position, spritebatch)
        {
            EnemyLib = new EnemyLib(new System.Numerics.Vector2(position.X, position.Y), 40, 55, 30);
            _player = Player.CurrentPlayer;
            PlayerLib = Player.CurrentPlayer.PlayerLib;
            _count = 0;
            _ctx = ctx;
            _type = type;
            _spritebatch = spritebatch;
            Fire = new Sprite(fireTexture, new Vector2(EnemyLib.Position.X, EnemyLib.Position.Y), _spritebatch);
            _animation = new Animation();
            _animation.Initialize(texture, position, frameWidth, frameHeight, 0, numberLin, frameCount, frameTime, Color.White, 1, true, true);
        }
        public Enemy(Texture2D texture, Vector2 position, SpriteBatch spritebatch, string type, Texture2D fireTexture, Texture2D textureWeapon, Texture2D textureBullet, DungeonPlanetGame ctx, int frameWidth, int frameHeight, int numberLin, int frameCount, int frameTime)
           : base(texture, position, spritebatch)
        {
            EnemyLib = new EnemyLib(new System.Numerics.Vector2(position.X, position.Y), 22, 49, 30);
            _player = Player.CurrentPlayer;
            PlayerLib = Player.CurrentPlayer.PlayerLib;
            _count = 0;
            _type = type;
            _spritebatch = spritebatch;
            _textureWeapon = textureWeapon;
            _textureBullet = textureBullet;
            _ctx = ctx;
            WeaponA = new Weapon(_textureWeapon, _textureBullet, _ctx, base.position, _spritebatch, EnemyLib);
            Fire = new Sprite(fireTexture, new Vector2(EnemyLib.Position.X, EnemyLib.Position.Y), _spritebatch);
            _animation = new Animation();
            _animation.Initialize(texture, position, frameWidth, frameHeight, 0, numberLin, frameCount, frameTime, Color.White, 1, true, true);
        }

        public void Update(GameTime gameTime)
        {
            _animation.Update(gameTime);
            if (WeaponA == null) _animation.Position = new Vector2(EnemyLib.Position.X + 20, EnemyLib.Position.Y + 25);
            else _animation.Position = new Vector2(EnemyLib.Position.X+15, EnemyLib.Position.Y+25);

            if (_ctx != null) EnemyLib.Update(EnemyLib.Position.X - _ctx.GraphicsDevice.Viewport.Width / 2, EnemyLib.Position.Y - _ctx.GraphicsDevice.Viewport.Height / 2);
            CheckKeyboardAndUpdateMovement(gameTime);
            EnemyLib.AffectWithGravity();
            EnemyLib.SimulateFriction();
            EnemyLib.MoveAsFarAsPossible((float)gameTime.ElapsedGameTime.TotalMilliseconds / 40);
            EnemyLib.StopMovingIfBlocked();
            position = new Vector2(EnemyLib.Position.X, EnemyLib.Position.Y);
            EnemyLib.Life = MathHelper.Clamp(EnemyLib.Life, 0, 100);
            if (WeaponA != null) WeaponA.Update(gameTime);
            if(EnemyLib != null && Fire != null)Fire.position = new Vector2(EnemyLib.Position.X - 5, EnemyLib.Position.Y - 20);
        }

        private void CheckKeyboardAndUpdateMovement(GameTime gameTime)
        {
            if (EnemyLib.Movement.X < -1)
            {
                _animation.Effect = SpriteEffects.None;
            }
            else if (EnemyLib.Movement.X > 1)
            {
                _animation.Effect = SpriteEffects.FlipHorizontally;
            }

            if (EnemyLib.Vision.IntersectsWith(PlayerLib.Bounds))
            {
                if (_type == "CQC")
                {
                    if (EnemyLib.Bounds.IntersectsWith(PlayerLib.Bounds))
                    {
                        if (EnemyLib.State != 2)
                        {
                            _player.PlayerInfo.Life -= 10;
                            EnemyLib.MakeDamage(PlayerLib);
                        }
                        else
                        {
                            _player.PlayerInfo.Life -= 10;
                            EnemyLib.MakeDamageWithSlim(PlayerLib);
                        }
                    }
                    if (EnemyLib.GetDistanceTo(PlayerLib.Position).X < 0.1 && EnemyLib.State != 2) { EnemyLib.Left(); }
                    else {  EnemyLib.LeftSlim(); }
                    if (EnemyLib.GetDistanceTo(PlayerLib.Position).X > 0.1 && EnemyLib.State != 2) { EnemyLib.Right(); }
                    else { EnemyLib.RightSlim(); }
                }
                else if (_type == "DIST")
                {
                    EnemyLib.IsShooting = true;
                    if (EnemyLib.Bounds.IntersectsWith(PlayerLib.Bounds))
                    {
                        _player.PlayerInfo.Life -= 1;
                        EnemyLib.MakeDamage(PlayerLib);
                    }
                }
            }
            else
            {
                EnemyLib.IsShooting = false;
                if (_count <= 100)
                {
                    if (EnemyLib.State != 2)
                    {
                        EnemyLib.Left();
                        _count++;
                    }
                    else
                    {
                        EnemyLib.LeftSlim();
                        _count++;
                    }
                }
                else if (_count <= 200 && _count > 100)
                {
                    if (EnemyLib.State != 2)
                    {
                        EnemyLib.Right();
                        _count++;
                    }
                    else
                    {
                        EnemyLib.RightSlim();
                        _count++;
                    }
                }
                else
                {
                    _count = 0;
                    if (EnemyLib.State == 1) EnemyLib.Life -= 15;
                }
            }
        }
        public override void Draw()
        {
            if (WeaponA != null) WeaponA.Draw();
            if (_ctx.IsOnScreen(new Rectangle(EnemyLib.Bounds.X, EnemyLib.Bounds.Y, EnemyLib.Bounds.Width, EnemyLib.Bounds.Height)))
            {
                if (EnemyLib.State == 1) Fire.Draw();
                _animation.Draw(SpriteBatch);
            }
        }
    }
}