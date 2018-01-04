﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using Comora;
using DungeonPlanet.Library;
using GeonBit.UI;
using GeonBit.UI.Entities;

namespace DungeonPlanet
{

    public class DungeonPlanetGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D _tileTexture, _playerTexture, _enemyTexture, _enemyTexture2, _bossTexture, _weaponTexture, _bulletTexture, _bulletETexture;
        private Texture2D _mediTexture, _bombTexture, _shieldTexture;
        private Player _player;
        private NPC _NPC;
        private Enemy _enemy;
        private Enemy _enemy2;
        private Boss _boss;
        private Board _board;
        private Random _rnd = new Random();
        private MediPack _mediPack;
        private Shield _shield;
        private Bomb _bomb;
        private SpriteFont _debugFont;
        private Camera _camera;
        private ProgressBar _healthBar;
        private ProgressBar _energyBar;
        private Door _door;
        private Door _door2;
        private Menu _menu;
        public static List<Enemy> Enemys { get; private set; }
        public static List<Boss> Bosses { get; private set; }
        public List<Item> Items { get; set; }

        public DungeonPlanetGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            //IsMouseVisible = true;
        }
        protected override void Initialize()
        {
            UserInterface.Initialize(Content, BuiltinThemes.editor);
            //change the cursor to a custom or built-in
            UserInterface.Active.SetCursor(CursorType.Default);
            // create a panel at the top-left corner of with 10x10 offset from it, with 'Golden' panel skin.
            // to see more skins check out the PanelSkin enum options or look at the panel examples in the example project.
            /*Panel panel = new Panel(size: new Vector2(500, 500), skin: PanelSkin.Golden, anchor: Anchor.TopLeft, offset: new Vector2(10, 10));
            UserInterface.Active.AddEntity(panel);*/
            //life bar test
            _healthBar = new ProgressBar(0, 100, size: new Vector2(400, 40), offset: new Vector2(10, 10));
            _healthBar.ProgressFill.FillColor = Color.Red;
            _healthBar.Locked = true;
            _energyBar = new ProgressBar(0, 100, size: new Vector2(400, 40), offset: new Vector2(10, 10));
            _energyBar.ProgressFill.FillColor = Color.LightSteelBlue;
            _energyBar.Locked = true;
            UserInterface.Active.AddEntity(_healthBar);
            UserInterface.Active.AddEntity(_energyBar);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            Enemys = new List<Enemy>();
            Bosses = new List<Boss>();
            Items = new List<Item>();
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _tileTexture = Content.Load<Texture2D>("tile");
            _playerTexture = Content.Load<Texture2D>("player");
            _enemyTexture = Content.Load<Texture2D>("enemy");
            _enemyTexture2 = Content.Load<Texture2D>("enemy2");
            _bossTexture = Content.Load<Texture2D>("boss");
            _weaponTexture = Content.Load<Texture2D>("player_arm");
            _bulletTexture = Content.Load<Texture2D>("bullet");
            _bulletETexture = Content.Load<Texture2D>("bulletE");
            _mediTexture = Content.Load<Texture2D>("Medipack");
            _bombTexture = Content.Load<Texture2D>("bomb");
            _shieldTexture = Content.Load<Texture2D>("shield");
            _board = new Board(_spriteBatch, _tileTexture, 2, 2);
            _player = new Player(_playerTexture, _weaponTexture, _bulletTexture, this, new Vector2(80, 80), _spriteBatch, Enemys, Bosses);
            _shield = new Shield(_shieldTexture, new Vector2(_player.position.X, _player.position.Y), _spriteBatch, _player, Enemys);
            _player.Shield = _shield;
            _enemy = new Enemy(_enemyTexture, new Vector2(500, 200), _spriteBatch, "CQC");
            _enemy2 = new Enemy(_enemyTexture2, new Vector2(400, 100), _spriteBatch, "DIST", _weaponTexture, _bulletETexture, this);
            _boss = new Boss(_bossTexture, new Vector2(1360, 200), _spriteBatch);
            _mediPack = new MediPack(_mediTexture, new Vector2(300, 300), _spriteBatch, 45, _player);
            _NPC = new NPC(_playerTexture, new Vector2(500, 200), _spriteBatch);
            _door = new Door(Content.Load<Texture2D>("door"), new Vector2(1000, 200), _spriteBatch, this);
            _door2 = new Door(Content.Load<Texture2D>("door"), new Vector2(100, 200), _spriteBatch, this);
            _bomb = new Bomb(_bombTexture, new Vector2(200, 300), _spriteBatch, 45, _player, Enemys);
            _debugFont = Content.Load<SpriteFont>("DebugFont");
            _camera = new Camera(GraphicsDevice);
            _menu = new Menu(this);
            _camera.LoadContent();

            if (Level.ActualState == Level.State.LevelOne)
            {
                for (int i = 0; i < Level.CurrentBoard.GetNext(20, 30 + 1); i++)
                {
                    Enemy enemy;
                    Tile tile = Level.CurrentBoard.Emptytile();

                    if (Level.CurrentBoard.GetNext(0, 2) == 0)
                    {
                        enemy = new Enemy(_enemyTexture, new Vector2(tile.Position.X, tile.Position.Y), _spriteBatch, "CQC");

                    }
                    else
                    {
                        enemy = new Enemy(_enemyTexture2, new Vector2(tile.Position.X, tile.Position.Y), _spriteBatch, "DIST", _weaponTexture, _bulletETexture, this);
                    }

                    Enemys.Add(enemy);
                }

                for (int i = 0; i < 2/*Level.CurrentBoard.GetNext(5, 10 + 1)*/; i++)
                {
                    Tile tile = Level.CurrentBoard.Emptytile();
                    MediPack mediPack = new MediPack(_mediTexture, new Vector2(tile.Position.X, tile.Position.Y), _spriteBatch, 50, _player);
                    Items.Add(mediPack);
                }

                _player.Shield = _shield;
                Bosses.Add(_boss);
            }
        }

        protected override void Update(GameTime gameTime)
        {
            
            base.Update(gameTime);
            UserInterface.Active.Update(gameTime);
            _camera.Update(gameTime);
            _player.Update(gameTime);
            _shield.Update(gameTime);
            if (Level.ActualState == Level.State.Menu)
            {
                _menu.Update();
            }
            if (Level.ActualState == Level.State.Hub)
            {
                _NPC.Update(gameTime);
                _door.Update(gameTime);
            }
            if (Level.ActualState == Level.State.LevelOne)
            {
                _door2.Update(gameTime);
            }

            for (int i = 0; i < Enemys.Count; i++)
            {
                if (Enemys[i].EnemyLib.Life <= 0)
                {
                    _player.Money += 20;
                    Enemys.Remove(Enemys[i]);
                }
                else
                {
                    if(IsOnScreen(Enemys[i].position))
                    Enemys[i].Update(gameTime);
                }
            }

            for (int i = 0; i < Bosses.Count; i++)
            {
                if (Bosses[i].BossLib.Life <= 0)
                {
                    _player.Money += 50;
                    Bosses.Remove(Bosses[i]);
                }
                else
                {
                    if (IsOnScreen(Bosses[i].position))
                        Bosses[i].Update(gameTime);
                }
            }

            for (int i = 0; i < Items.Count; i++)
            {
                if (IsOnScreen(Items[i].position))
                    Items[i].Update(gameTime);
                if (Items[i].IsFinished && !(Items[i] is Shield))
                {
                    Items.Remove(Items[i]);
                }
            }

            if (_player.PlayerLib.IsDead(_player.Life)) RestartHub();
            _camera.Position = _player.position;
            _healthBar.Value = _player.Life;
            _energyBar.Value = _player.Energy;
            
            CheckKeyboardAndReact();
            
        }

        private bool IsOnScreen(Vector2 position)
        {
            _camera.ToScreen(ref position, out Vector2 screenPosition);
            if(_camera.Position.X -_camera.Width/2 > screenPosition.X || screenPosition.X <_camera.Position.X + _camera.Width / 2 || _camera.Position.Y - _camera.Height / 2 > screenPosition.Y || screenPosition.Y < _camera.Position.Y + _camera.Height / 2)
            {
                return true;
            }
            return false;
        }

        private void CheckKeyboardAndReact()
        {
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.F5)) { RestartHub(); }
            if (state.IsKeyDown(Keys.Escape)) { Exit(); }
            if (state.IsKeyDown(Keys.F6)) { RestartLevelOne(); }
            _camera.Debug.IsVisible = Keyboard.GetState().IsKeyDown(Keys.F1);

        }

        private void RestartHub()
        { 
            Level.ActualState = Level.State.Hub;
            LoadContent();
        }

        internal void RestartLevelOne()
        {
            Level.ActualState = Level.State.LevelOne;
            LoadContent();
        }

        private void PutJumperInTopLeftCorner()
        {
            PlayerLib.Position = System.Numerics.Vector2.One * 80;
            _player.PlayerLib.Movement = System.Numerics.Vector2.Zero;
        }

        protected override void Draw(GameTime gameTime)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin(_camera);
            base.Draw(gameTime);
            if (Level.ActualState == Level.State.Hub)
            {
                _NPC.Draw();
                _door.Draw();
            }
            if (Level.ActualState == Level.State.LevelOne)
            {
                _door2.Draw();
            }
            _board.Draw();
            WriteDebugInformation();
            _player.Draw();
            foreach (Enemy enemy in Enemys) enemy.Draw();
            foreach (Boss boss in Bosses) boss.Draw();
            foreach (Item item in Items)
            {
                if (item != null) item.Draw();
            }
            _spriteBatch.End();
            _spriteBatch.Draw(_camera.Debug);
            UserInterface.Active.Draw(_spriteBatch);
            stopwatch.Stop();
            Console.WriteLine(stopwatch.Elapsed);
        }

        private void WriteDebugInformation()
        {
            string enemyLifeText;
            string positionInText = string.Format("Position of Jumper: ({0:0.0}, {1:0.0})", PlayerLib.Position.X, _player.position.Y);
            string movementInText = string.Format("Current movement: ({0:0.0}, {1:0.0})", _player.PlayerLib.Movement.X, _player.PlayerLib.Movement.Y);
            string isOnFirmGroundText = string.Format("On firm ground?: {0}", _player.PlayerLib.IsOnFirmGround());
            string playerLifeText = string.Format("PLife: {0}/100", _player.Life);
            string playerMoney = string.Format("Coins : {0}", _player.Money);
            if(_enemy != null)
            {
                enemyLifeText = string.Format("ELife: {0}/100", _enemy.EnemyLib.Life);
                DrawWithShadow(enemyLifeText, new Vector2(70, 620));
            }
            if (_enemy2 != null)
            {
                enemyLifeText = string.Format("ELife2: {0}/100", _enemy2.EnemyLib.Life);
                DrawWithShadow(enemyLifeText, new Vector2(70, 640));
            }
            string bossLifeText = string.Format("BLife: {0}/200", _boss.BossLib.Life);

            DrawWithShadowMoney(playerMoney, new Vector2(PlayerLib.Position.X - 940, PlayerLib.Position.Y - 400));
            DrawWithShadow(positionInText, new Vector2(10, 0));
            DrawWithShadow(movementInText, new Vector2(10, 20));
            DrawWithShadow(isOnFirmGroundText, new Vector2(10, 40));
            DrawWithShadow("F6 for random board", new Vector2(70, 580));
            DrawWithShadow(playerLifeText, new Vector2(70, 600));
        }

        private void DrawWithShadow(string text, Vector2 position)
        { 
            
            _spriteBatch.DrawString(_debugFont, text, position + Vector2.One, Color.Black);
            _spriteBatch.DrawString(_debugFont, text, position, Color.LightYellow);
        }

        private void DrawWithShadowMoney(string text, Vector2 position)
        {
            _spriteBatch.DrawString(_debugFont, text, position + Vector2.One, Color.Black, 0, new Vector2(0, 0), 2, SpriteEffects.None, 0);
            _spriteBatch.DrawString(_debugFont, text, position, Color.LightYellow, 0, new Vector2(0, 0), 2, SpriteEffects.None, 0);
        }
    }
}