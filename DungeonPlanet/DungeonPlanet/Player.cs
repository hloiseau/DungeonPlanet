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
    [Serializable]
    public class Player : Sprite
    {
        public PlayerLib PlayerLib { get; set; }
        public PlayerInfo PlayerInfo { get; set; }
        public Weapon Weapon { get; set; }
        public static Player CurrentPlayer { get; private set; }
        public Shield Shield { get; set; }
        Texture2D _texturebomb;
        SpriteBatch _spritebatch;
        List<Enemy> _enemys;
        List<Bomb> _bombs;
        KeyboardState _previousKey;
        Animation _animation;
        int _animeState = 0;

        public Player(Texture2D texturePlayer, Texture2D textureWeapon, Texture2D textureBomb, Texture2D textureBullet, DungeonPlanetGame ctx, Vector2 position, SpriteBatch spritebatch, List<Enemy> enemys, List<Boss> bosses)
            : base(texturePlayer, position, spritebatch)
        {
            PlayerLib = new PlayerLib(new System.Numerics.Vector2(position.X, position.Y), 40, 64);
            PlayerInfo = new PlayerInfo();
            _animation = new Animation();
            _animation.Initialize(texturePlayer, position, 40, 64, 0, 0, 2, 150, Color.White, 1, true, false);
            
            Weapon = new Weapon(textureWeapon, textureBullet, ctx, position, spritebatch, bosses);
            _bombs = new List<Bomb>();
            _texturebomb = textureBomb;
            _spritebatch = spritebatch;
            _enemys = enemys;
            CurrentPlayer = this;
        }

        public void Update(GameTime gameTime)
        {
            
            CheckKeyboardAndUpdateMovement();
            PlayerLib.AffectWithGravity();
            PlayerLib.SimulateFriction();
            PlayerLib.MoveAsFarAsPossible((float)gameTime.ElapsedGameTime.TotalMilliseconds / 15);
            PlayerLib.StopMovingIfBlocked();
            PlayerLib.IsDead(PlayerInfo.Life);

            position = new Vector2(PlayerLib.Position.X, PlayerLib.Position.Y);
            _animation.Position = new Vector2(PlayerLib.Position.X, PlayerLib.Position.Y + 35);
            _animation.Update(gameTime);
            Weapon.Update(gameTime);
            CheckMovementAndUpdateAnimation();
            PlayerInfo.Life = MathHelper.Clamp(PlayerInfo.Life, 0, 100);
            if (PlayerLib.IsOnFirmGround()) PlayerInfo.Energy++;
            PlayerInfo.Energy = MathHelper.Clamp(PlayerInfo.Energy, 0, 100);

            for (int i = 0; i< _bombs.Count; i++) 
            {
                _bombs[i].Update(gameTime);
                if (_bombs[i].IsFinished)
                {
                    _bombs.Remove(_bombs[i]);
                }
            }
        }

        private void CheckMovementAndUpdateAnimation()
        {
            if (PlayerLib.IsOnFirmGround())
            {
                if (PlayerLib.Movement.X < -1)
                {
                    if (_animeState != 0)
                    {
                        _animation.CurrentFrameCol = 0;
                        _animeState = 0;
                    }
                    _animation.FrameWidth = 47;
                    _animation.FrameCount = 5;
                    _animation.Effect = SpriteEffects.FlipHorizontally;
                    _animation.CurrentFrameLin = 2;
                    _animation.FrameTime = 150;
                }
                else if (PlayerLib.Movement.X > 1)
                {
                    if (_animeState != 1)
                    {
                        _animation.CurrentFrameCol = 0;
                        _animeState = 1;
                    }
                    _animation.FrameWidth = 47;
                    _animation.FrameCount = 5;
                    _animation.Effect = SpriteEffects.None;
                    _animation.CurrentFrameLin = 2;
                    _animation.FrameTime = 150;
                }
                else
                {
                    if (_animeState != 2)
                    {
                        _animation.CurrentFrameCol = 0;
                        _animeState = 2;
                    }
                    _animation.FrameTime = 300;
                    _animation.FrameWidth = 40;
                    _animation.FrameCount = 2;
                    _animation.CurrentFrameLin = 0;
                }
            }
            else
            {
                if (PlayerLib.Movement.Y < -3)
                {
                    _animation.CurrentFrameCol = 2;
                    _animation.FrameWidth = 40;
                    _animation.FrameCount = 0;
                    _animation.CurrentFrameLin = 1;
                    _animation.FrameTime = 1000;
                }
                else if (PlayerLib.Movement.Y > 3)
                {
                    _animation.CurrentFrameCol = 0;

                    _animation.FrameWidth = 40;
                    _animation.FrameCount = 0;
                    _animation.CurrentFrameLin = 1;
                    _animation.FrameTime = 1000;
                }
                else
                {
                    _animation.CurrentFrameCol = 1;

                    _animation.FrameTime = 1000;
                    _animation.FrameWidth = 40;
                    _animation.FrameCount = 1;
                    _animation.CurrentFrameLin = 1;
                }
            }
            

        }

        private void CheckKeyboardAndUpdateMovement()
        {
            KeyboardState keyboardState = Keyboard.GetState();


            if (keyboardState.IsKeyDown(Keys.Q)) { PlayerLib.Left(); }
            if (keyboardState.IsKeyDown(Keys.D)) { PlayerLib.Right(); }
            if (keyboardState.IsKeyDown(Keys.Z) && PlayerLib.IsOnFirmGround()) { PlayerLib.Jump(); }
            if (keyboardState.IsKeyDown(Keys.B) && !_previousKey.IsKeyDown(Keys.B) && PlayerInfo.Energy >= 50 )
            {
                Bomb bomb = new Bomb(_texturebomb, position, _spritebatch, 0, this, _enemys);
                bomb.ItemLib.Movement = System.Numerics.Vector2.UnitX * 20;
                _bombs.Add(bomb);
                PlayerInfo.Energy -= 50;
            }
            if (keyboardState.IsKeyDown(Keys.A) && !_previousKey.IsKeyDown(Keys.A) && !Shield.IsActive)
            {
                Shield.IsActive = true;
            } 
            else if(keyboardState.IsKeyDown(Keys.A) && !_previousKey.IsKeyDown(Keys.A) && Shield.IsActive)
            {
                Shield.IsActive = false;
            }
            if (Shield.IsActive)
            {
                PlayerInfo.Energy -= 2;
            }
            if (PlayerInfo.Energy <= 0) Shield.IsActive = false;
            _previousKey = keyboardState;
        }

        public override void Draw()
        {
            KeyboardState keyboardState = Keyboard.GetState();
            _animation.Draw(SpriteBatch);
            Weapon.Draw();
            for (int i = 0; i < _bombs.Count; i++)
            {
                _bombs[i].Draw();
            }
            if  (Shield.IsActive && Level.ActualState == Level.State.Level) { Shield.Draw(); }
        }

      
    }
}
