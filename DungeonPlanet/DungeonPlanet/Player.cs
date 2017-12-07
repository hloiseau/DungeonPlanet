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
    public class Player : Sprite
    {
        public PlayerLib PlayerLib { get; set; }
        public Weapon Weapon { get; set; }
        public int Life { get; set; }
        public int Energy { get; set; }
        public static Player CurrentPlayer { get; private set; }
        public Shield Shield { get; set; }
        Texture2D _texturebomb;
        SpriteBatch _spritebatch;
        List<Enemy> _enemys;
        List<Bomb> _bombs;
        KeyboardState _previousKey;

        public Player(Texture2D texturePlayer, Texture2D textureWeapon, Texture2D textureBullet, DungeonPlanetGame ctx, Vector2 position, SpriteBatch spritebatch, List<Enemy> enemys, List<Boss> bosses)
            : base(texturePlayer, position, spritebatch)
        {
            PlayerLib = new PlayerLib(new System.Numerics.Vector2(position.X, position.Y), texturePlayer.Width, texturePlayer.Height);
            Weapon = new Weapon(textureWeapon, textureBullet, ctx, position, spritebatch, bosses);
            _bombs = new List<Bomb>();
            _texturebomb = textureWeapon;
            _spritebatch = spritebatch;
            _enemys = enemys;
            Life = 70;
            Energy = 0;
            CurrentPlayer = this;
        }

        public void Update(GameTime gameTime)
        {
            CheckKeyboardAndUpdateMovement();
            PlayerLib.AffectWithGravity();
            PlayerLib.SimulateFriction();
            PlayerLib.MoveAsFarAsPossible((float)gameTime.ElapsedGameTime.TotalMilliseconds / 15);
            PlayerLib.StopMovingIfBlocked();
            PlayerLib.IsDead(Life);
            position = new Vector2(PlayerLib.Position.X, PlayerLib.Position.Y);
            Weapon.Update(gameTime);
            Life = MathHelper.Clamp(Life, 0, 100);
            if (PlayerLib.IsOnFirmGround()) Energy++;
            Energy = MathHelper.Clamp(Energy, 0, 100);
            for(int i = 0; i< _bombs.Count; i++) 
            {
                _bombs[i].Update(gameTime);
                if (_bombs[i].IsFinished)
                {
                    _bombs.Remove(_bombs[i]);
                }

            }
        }

        private void CheckKeyboardAndUpdateMovement()
        {
            KeyboardState keyboardState = Keyboard.GetState();


            if (keyboardState.IsKeyDown(Keys.Q)) { PlayerLib.Left(); }
            if (keyboardState.IsKeyDown(Keys.D)) { PlayerLib.Right(); }
            if (keyboardState.IsKeyDown(Keys.Z) && PlayerLib.IsOnFirmGround()) { PlayerLib.Jump(); }
            if (keyboardState.IsKeyDown(Keys.B) && !_previousKey.IsKeyDown(Keys.B) && Energy >= 50)
            {
                Bomb bomb = new Bomb(_texturebomb, position, _spritebatch, 0, this, _enemys);
                bomb.ItemLib.Movement = System.Numerics.Vector2.UnitX * 20;
                _bombs.Add(bomb);
                Energy -= 50;
            }
            _previousKey = keyboardState;
        }

        public override void Draw()
        {
            KeyboardState keyboardState = Keyboard.GetState();
            base.Draw();
            Weapon.Draw();
            for (int i = 0; i < _bombs.Count; i++)
            {
                _bombs[i].Draw();
            }
            if (keyboardState.IsKeyDown(Keys.A) && !(Shield.Activate) && Level.ActualState == Level.State.LevelOne)
            {
                Shield.Activate = true;
                Shield.Draw();
            }
            else if (keyboardState.IsKeyUp(Keys.A) && Shield.Activate == true && Level.ActualState == Level.State.LevelOne) { Shield.Draw(); }
            else if (keyboardState.IsKeyDown(Keys.A) && Shield.Activate == true && Level.ActualState == Level.State.LevelOne)
            {
                Shield.Activate = false;
            }
        }

    }
}
