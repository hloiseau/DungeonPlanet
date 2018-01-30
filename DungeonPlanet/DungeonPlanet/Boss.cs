using DungeonPlanet.Library;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Comora;

namespace DungeonPlanet
{
    public class Boss : Sprite
    {
        public BossLib BossLib { get; set; }
        public PlayerLib PlayerLib { get; set; }
        public Sprite Fire { get; set; }
        int _count;
        List<Bullet> _bullets;
        Animation _animation;
        Animation _animationFireWave;
        Player _player;
        Texture2D _textureBullet;
        Texture2D _textureFireWave;
        int _elapsedTime;
        

        public Boss(Texture2D textureBullet, Texture2D textureFireWave, Texture2D textureBoss, Vector2 position, SpriteBatch spritebatch, Texture2D fireBossTexture)
            : base(textureBoss, position, spritebatch)
        {
            BossLib = new BossLib(new System.Numerics.Vector2(position.X, position.Y), 257, 100, 1000);
            _player = Player.CurrentPlayer;
            _animation = new Animation();
            _animation.Initialize(textureBoss, new Vector2(position.X - 75, position.Y - 10), 257, 100, 1, 0, 4, 75, Color.White, 2, true, false);
            _textureFireWave = textureFireWave;
            _textureBullet = textureBullet;
            _bullets = new List<Bullet>();
            PlayerLib = Player.CurrentPlayer.PlayerLib;
            _count = 0;
            Fire = new Sprite(fireBossTexture, new Vector2(BossLib.Position.X, BossLib.Position.Y), spritebatch);
        }

        public void Update(GameTime gameTime)
        {
            _animation.Update(gameTime);
            _animation.Position = new Vector2(BossLib.Position.X, BossLib.Position.Y+7);
            _elapsedTime += (int)gameTime.ElapsedGameTime.Milliseconds;
            if (_elapsedTime >= 5000)
            {
                _animation.CurrentFrameLin = 1;
                _animation.CurrentFrameCol = 0;
                _animation.FrameCount = 5;
                _animation.FrameWidth = 255;
                _animation.FrameTime = 150;
                _animation.Looping = false;
                //x=-128.5
                //y=-23
                Bullet bullet = new Bullet(_textureBullet, _textureFireWave,new Vector2(position.X-75,position.Y-10), SpriteBatch, new Vector2(PlayerLib.Position.X - position.X, (PlayerLib.Position.Y + 15) - position.Y));
                _bullets.Add(bullet);
                _elapsedTime = 0;
            }
            if (!_animation.IsActive)
            {
                _animation.IsActive = true;
                _animation.Looping = true;
                _animation.CurrentFrameLin = 0;
                _animation.CurrentFrameCol = 1;
                _animation.FrameWidth = 257;
                _animation.FrameTime = 75;


            }

            BulletUpdate(gameTime);
            CheckKeyboardAndUpdateMovement();
            BossLib.AffectWithGravity();
            BossLib.SimulateFriction();
            BossLib.MoveAsFarAsPossible((float)gameTime.ElapsedGameTime.TotalMilliseconds / 40);
            BossLib.StopMovingIfBlocked();
            BossLib.IsDead();
            position = new Vector2(BossLib.Position.X, BossLib.Position.Y);
            BossLib.Life = MathHelper.Clamp(BossLib.Life, 0, 200);

            if (BossLib != null && Fire != null) Fire.position = new Vector2(BossLib.Position.X, BossLib.Position.Y);
        }

        private void BulletUpdate(GameTime gameTime)
        {
            for (int i = 0; i < _bullets.Count; i++)
            {
                if (_bullets[i].BulletLib.IsDead() || _bullets[i].HasTouchedEnemy() || _bullets[i].HasTouchedTile() || _bullets[i].HasTouchedPlayer(BossLib)) 
                {
                    _bullets.Remove(_bullets[i]);
                }
                else
                {
                    _bullets[i].Update(gameTime);
                }
            }
        }

        private void CheckKeyboardAndUpdateMovement()
        {
            if (BossLib.Vision.IntersectsWith(PlayerLib.Bounds))
            {
                if (BossLib.Bounds.IntersectsWith(PlayerLib.Bounds))
                {
                    
                    _player.PlayerInfo.Life -= 25;
                    BossLib.MakeDamage(PlayerLib);
                }
                if (BossLib.GetDistanceTo(PlayerLib.Position).X < 0.1 && BossLib.State != 2) { BossLib.Left(); }
                else { BossLib.LeftSlim(); }
            }
            else
            {
                if(BossLib.State != 2) BossLib.Right();
                else { BossLib.RightSlim(); }
            }

            if (_count <= 50) _count++;
            else
            {
                _count = 0;
                if (BossLib.State == 1) BossLib.Life -= 10;
            }
        }

        public override void Draw()
        {
            _animation.Draw(SpriteBatch);
            for(int x =0; x < _bullets.Count; x++)
            {
                _bullets[x].Draw();
            }
            if (BossLib.State == 1) Fire.Draw();
        }
    }
}
