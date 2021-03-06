﻿using Comora;
using DungeonPlanet.Library;
using GeonBit.UI;
using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

namespace DungeonPlanet
{

    public class DungeonPlanetGame : Game
    {
        private GraphicsDeviceManager _graphics;
        public GraphicsDeviceManager Graphics
        {
            get => _graphics;
            set => _graphics = value;
        }
        private SpriteBatch _spriteBatch;

        int _elapsedTime;

        private Texture2D _hubBackground001;
        private Texture2D _hubBackground002;
        private Texture2D _hubBackground003;

        private Texture2D _levelBackground01;

        private SpriteObject _cat;
        private SpriteObject _nugget;
        private Sprite _setOfBullet;
        private Sprite _setOfFood;

        private Texture2D _tileTexture, _playerTexture, _enemyTexture, _enemyTexture2, _bossTexture, _weaponTexture, _bulletTexture, _bulletETexture, _enemyWeaponTexture;
        private Texture2D _mediTexture, _bombTexture, _shieldTexture;
        private Texture2D _fireTexture, _fireBossTexture;

        internal Texture2D TankFirewave { get; private set; }
        internal Texture2D TankBullet { get; private set; }
        Texture2D _end;

        private Song backgroundHubSong;
        private Song backgroundBossSong;
        private Song backgroundLevel1Song;
        private Song backgroundLevel2Song;
        private Song backgroundLevel3Song;
        private Song backgroundLevel4Song;
        private Song backgroundLevel5Song;

        private SoundEffect GunSoundEfect;
        private SoundEffect GrandpaSingingJhonny;
        private SoundEffect GrandpaSingingGanon;

        private Player _player;
        private NPCMarchand _NPCMarchand;
        private NPCWeapon _NPCWeapon;
        private NPCWeaponSeller _NPCWeaponSeller;
        private NPCTheWise _NPCWise;
        private NPCNarrator _NPCNarrator;
        private Enemy _enemy;
        private Enemy _enemy2;
        private Boss _boss;
        private Board _board;
        private Random _rnd = new Random();
        private MediPack _mediPack;
        private Shield _shield;
        private SpriteFont _debugFont;
        internal Camera _camera;
        private ProgressBar _healthBar;
        private ProgressBar _energyBar;
        private Door[] _door;
        private Door _door2;
        private Menu _menu;
        private Paragraph _money;
        private KeyboardState _previousState;
        public static List<Enemy> Enemys { get; private set; }
        public static List<Boss> Bosses { get; private set; }
        public List<Item> Items { get; set; }
        private int oldcount;
        private string oldsing;
        int _elpasedtime;
        int Singingtime;
        Random _random = new Random();
        public bool DebugZoom { get; private set; }

        public PlayerInfo PlayerInfo
        {
            get { return _player.PlayerInfo; }
            private set { _player.PlayerInfo = value; }
        }

        public DungeonPlanetGame()
        {
            _graphics = new GraphicsDeviceManager(this)
            {
                IsFullScreen = true,
                PreferredBackBufferWidth = 1920,
                PreferredBackBufferHeight = 1080
        };
            Content.RootDirectory = "Content";
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
            _money = new Paragraph("", offset: new Vector2(10, 10));
            _money.Scale = 1;
            UserInterface.Active.AddEntity(_healthBar);
            UserInterface.Active.AddEntity(_energyBar);
            UserInterface.Active.AddEntity(_money);
            _menu = new Menu(this);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            Enemys = new List<Enemy>();
            Bosses = new List<Boss>();
            Items = new List<Item>();
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _tileTexture = Content.Load<Texture2D>("sprite texture LV1");
            _playerTexture = Content.Load<Texture2D>("PlayerSprite");
            _hubBackground001 = Content.Load<Texture2D>("hub001");
            _hubBackground002 = Content.Load<Texture2D>("hub002");
            _hubBackground003 = Content.Load<Texture2D>("hub003");

            _levelBackground01 = Content.Load<Texture2D>("level02");

            Texture2D theWise = Content.Load<Texture2D>("NPCTheWise");
            Texture2D weapon = Content.Load<Texture2D>("NPCWeapon");
            TankBullet = Content.Load<Texture2D>("TankBullet");
            TankFirewave = Content.Load<Texture2D>("TankFirewave");
            Texture2D shotgun = Content.Load<Texture2D>("Shotgun");
            Texture2D launcher = Content.Load<Texture2D>("Lance Roquette");
            Texture2D narrator = Content.Load<Texture2D>("NPCNarrator");
            Texture2D marchand = Content.Load<Texture2D>("NPCMerchand");
            Texture2D weaponSeller = Content.Load<Texture2D>("WeaponSeller");
            Texture2D cat = Content.Load<Texture2D>("Cat");
            Texture2D nugget = Content.Load<Texture2D>("Nugget");
            Texture2D bullet = Content.Load<Texture2D>("ItemSetBullet");
            Texture2D food = Content.Load<Texture2D>("ItemSetFood");
            Texture2D boom = Content.Load<Texture2D>("Boom");
            _end = Content.Load<Texture2D>("end1");
            _enemyTexture = Content.Load<Texture2D>("soldier");
            _enemyTexture2 = Content.Load<Texture2D>("skeleton");
            _bossTexture = Content.Load<Texture2D>("Tank");
            _weaponTexture = Content.Load<Texture2D>("playerGun");
            _enemyWeaponTexture = Content.Load<Texture2D>("enemyGun");
            _bulletTexture = Content.Load<Texture2D>("bullet");
            _bulletETexture = Content.Load<Texture2D>("bulletE");
            _mediTexture = Content.Load<Texture2D>("Medipack");
            _bombTexture = Content.Load<Texture2D>("bomb");
            _shieldTexture = Content.Load<Texture2D>("shield");
            _fireTexture = Content.Load<Texture2D>("fire");
            _fireBossTexture = Content.Load<Texture2D>("fireBoss");

            _board = new Board(_spriteBatch, _tileTexture, 2, 2, this);
            _player = new Player(_playerTexture, _weaponTexture, _bombTexture, boom, _bulletTexture, this, new Vector2(80, 80), _spriteBatch, Enemys, Bosses);
            _shield = new Shield(_shieldTexture, new Vector2(_player.position.X, _player.position.Y), _spriteBatch, _player, Enemys);
            _player.Shield = _shield;
            _boss = new Boss(TankBullet, TankFirewave, _bossTexture, new Vector2(1360, 200), _spriteBatch, _fireBossTexture);
            _mediPack = new MediPack(_mediTexture, new Vector2(300, 300), _spriteBatch, 45, _player);

            _NPCMarchand = new NPCMarchand(marchand, new Vector2(500, 200), _spriteBatch);
            _NPCWeapon = new NPCWeapon(weapon, new Vector2(300, 200), _spriteBatch);
            _NPCWeaponSeller = new NPCWeaponSeller(weaponSeller, shotgun, launcher, new Vector2(1100, 200), _spriteBatch);
            _NPCWise = new NPCTheWise(theWise, new Vector2(700, 200), _spriteBatch);
            _NPCNarrator = new NPCNarrator(narrator, new Vector2(900, 200), _spriteBatch);

            _cat = new SpriteObject(cat, new Vector2(920, 200), _spriteBatch, 28, 16, 6, 210);
            _nugget = new SpriteObject(nugget, new Vector2(872, 200), _spriteBatch, 18, 16, 10, 150);
            _setOfFood = new Sprite(food, new Vector2(450, 535), _spriteBatch);
            _setOfBullet = new Sprite(bullet, new Vector2(300, 545), _spriteBatch);

            _door = new Door[5];
            for (int x = 0; x < _door.Length; x++)
            {
                if (Level.ActualState == Level.State.Hub || (Level.ActualState == Level.State.BossRoom && x == 0))
                    _door[x] = new Door(Content.Load<Texture2D>("door"), new Vector2(1625 + x * 180, 200), _spriteBatch, this, (Level.LevelID)x + 1);
            }
            _door2 = new Door(Content.Load<Texture2D>("door"), new Vector2(Case._dorX, Case._dorY), _spriteBatch, this);
            //_bomb = new Bomb(_bombTexture, new Vector2(200, 300), _spriteBatch, 45, _player, Enemys);
            _debugFont = Content.Load<SpriteFont>("DebugFont");
            _camera = new Camera(GraphicsDevice)
            {
                Zoom = 1.35f
            };
            DebugZoom = false;
            _camera.LoadContent();

            GunSoundEfect = Content.Load<SoundEffect>("GunSoundEfect");
            GrandpaSingingJhonny = Content.Load<SoundEffect>("GrandpaSingingJhonny");
            GrandpaSingingGanon = Content.Load<SoundEffect>("Ganon");

            backgroundBossSong = Content.Load<Song>("backgroundBossSong");
            backgroundHubSong = Content.Load<Song>("backgroundHubSong");
            backgroundLevel1Song = Content.Load<Song>("backgroundLevel1Song");
            backgroundLevel2Song = Content.Load<Song>("backgroundLevel2Song");
            backgroundLevel3Song = Content.Load<Song>("backgroundLevel3Song");
            backgroundLevel4Song = Content.Load<Song>("backgroundLevel4Song");
            backgroundLevel5Song = Content.Load<Song>("backgroundLevel5Song");

            if (Level.ActualState == Level.State.BossRoom)
            {
                Bosses.Add(_boss);
                MediaPlayer.Play(backgroundBossSong);
            }

            if (Level.ActualState == Level.State.Hub)
            {
                MediaPlayer.Play(backgroundHubSong);
            }

            if (Level.ActualState == Level.State.Level)
            {
                for (int i = 0; i < Level.CurrentBoard.GetNext(20, 30 + 1); i++)
                {
                    int colorSwapEnemy = _random.Next(0, 4);
                    if (colorSwapEnemy == 4) colorSwapEnemy = 3;

                    Enemy enemy;
                    Tile tile = Level.CurrentBoard.Emptytile();

                    if (Level.CurrentBoard.GetNext(0, 2) == 0)
                    {
                        enemy = new Enemy(_enemyTexture, new Vector2(tile.Position.X, tile.Position.Y), _spriteBatch, "CQC", _fireTexture, 41, 55, colorSwapEnemy, 8, 150, this);

                    }
                    else
                    {
                        enemy = new Enemy(_enemyTexture2, new Vector2(tile.Position.X, tile.Position.Y), _spriteBatch, "DIST", _fireTexture, _weaponTexture, _bulletETexture, this, 25, 50, colorSwapEnemy, 7, 150);
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

                if (Level.ID == Level.LevelID.One)
                {
                    MediaPlayer.Play(backgroundLevel1Song);
                }
                else if (Level.ID == Level.LevelID.Two)
                {
                    MediaPlayer.Play(backgroundLevel2Song);
                }
                else if (Level.ID == Level.LevelID.Three)
                {
                    MediaPlayer.Play(backgroundLevel3Song);
                }
                else if (Level.ID == Level.LevelID.Four)
                {
                    MediaPlayer.Play(backgroundLevel4Song);
                }
                else if (Level.ID == Level.LevelID.Five)
                {
                    MediaPlayer.Play(backgroundLevel5Song);
                }
            }
        }
        public void Reload() => LoadContent();

        protected override void Update(GameTime gameTime)
        {
            _camera.Update(gameTime);
            MediaPlayer.IsRepeating = true;
            base.Update(gameTime);
            UserInterface.Active.Update(gameTime);
            _menu.Update();
            if (Level.ActualState == Level.State.End)
            {
                _elapsedTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (_elapsedTime > 10000)
                {
                    PlayerInfo.Money += 10000;
                    PlayerInfo.Life = 100;
                    Level.ActualState = Level.State.Hub;
                }
            }

            if (Level.ActualState == Level.State.Hub)
            {
                _player.Update(gameTime);
                _shield.Update(gameTime);
                for (int x = 0; x < (int)PlayerInfo.Progress; x++)
                {
                    if (_door[x] != null) _door[x].Update(gameTime);
                }
                _NPCMarchand.Update(gameTime);
                _NPCWeapon.Update(gameTime);
                _NPCWeaponSeller.Update(gameTime);
                _NPCWise.Update(gameTime);
                _nugget.Update(gameTime);
                _NPCNarrator.Update(gameTime);
                _cat.Update(gameTime);

                if (NPCTheWise.SingingLine != oldsing && NPCTheWise.SingingLine == "Allumer le feux !!! hey, tu sais tu peux acheter des munitions enflammees chez l armurier.")
                {
                    MediaPlayer.Pause();
                    GrandpaSingingJhonny.Play();
                    _elpasedtime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                    Singingtime = _elpasedtime + 2500;
                    oldsing = NPCTheWise.SingingLine;
                }
                if (NPCTheWise.SingingLine != oldsing && NPCTheWise.SingingLine == "SEUL LINK PEUT VAINCRE GANON !! ... Au fait c'est qui ganon ?")
                {
                    MediaPlayer.Pause();
                    GrandpaSingingGanon.Play();
                    _elpasedtime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                    Singingtime = _elpasedtime + 3000;
                    oldsing = NPCTheWise.SingingLine;
                }
                else
                {
                    oldsing = NPCTheWise.SingingLine;
                }

                _elpasedtime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (Singingtime != null)
                {
                    if (_elpasedtime > Singingtime)
                    {
                        MediaPlayer.Resume();
                    }
                }
            }
            if (Level.ActualState == Level.State.Level)
            {
                _player.Update(gameTime);
                _shield.Update(gameTime);
                _door2.Update(gameTime);
                if (oldcount == null)
                {
                    oldcount = Player.CurrentPlayer.Weapon.Bullets.Count;
                }
                if (Player.CurrentPlayer.Weapon.Bullets.Count > oldcount)
                {
                    GunSoundEfect.Play();
                }
                oldcount = Player.CurrentPlayer.Weapon.Bullets.Count;
            }
            if (Level.ActualState == Level.State.BossRoom)
            {
                _player.Update(gameTime);
                _shield.Update(gameTime);
                for (int i = 0; i < Bosses.Count; i++)
                {
                    if (Bosses[i].BossLib.Life <= 0)
                    {
                        _player.PlayerInfo.Money += 50;
                        Bosses.Remove(Bosses[i]);
                    }
                    else
                    {

                        Bosses[i].Update(gameTime);
                    }
                }
                if (Bosses.Count == 0) _door[0].Update(gameTime);
                if (oldcount == null)
                {
                    oldcount = Player.CurrentPlayer.Weapon.Bullets.Count;
                }
                if (Player.CurrentPlayer.Weapon.Bullets.Count > oldcount)
                {
                    GunSoundEfect.Play();
                }
                oldcount = Player.CurrentPlayer.Weapon.Bullets.Count;

            }

            for (int i = 0; i < Enemys.Count; i++)
            {
                if (Enemys[i].EnemyLib.Life <= 0)
                {
                    _player.PlayerInfo.Money += 20;
                    Enemys.Remove(Enemys[i]);
                }
                else
                {
                    Enemys[i].Update(gameTime);
                }
            }

            for (int i = 0; i < Items.Count; i++)
            {

                Items[i].Update(gameTime);
                if (Items[i].IsFinished && !(Items[i] is Shield) && IsOnScreen(new Rectangle(Items[i].ItemLib.Bounds.X, Items[i].ItemLib.Bounds.Y, Items[i].ItemLib.Bounds.Width, Items[i].ItemLib.Bounds.Height)))
                {
                    Items.Remove(Items[i]);
                }
            }

            if (_player.PlayerLib.IsDead(_player.PlayerInfo.Life))
            {
                RestartHub();
                _player.PlayerInfo.Life = 100;
            }
            _camera.Position = _player.position;
            _healthBar.Value = _player.PlayerInfo.Life;
            _energyBar.Value = _player.PlayerInfo.Energy;
            _money.Text = string.Format("Coins : {0}", _player.PlayerInfo.Money);
            CheckKeyboardAndReact();

        }

        internal bool IsOnScreen(Rectangle position)
        {
            Rectangle rectange = new Rectangle((int)_camera.GetBounds().X,
                        (int)_camera.GetBounds().Y + 325,
                        (int)(_camera.GetBounds().Width),
                        (int)(_camera.GetBounds().Height));
            if (rectange.Intersects(position))
            {
                return true;
            }
            return false;
        }

        private void CheckKeyboardAndReact()
        {
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.F5)) RestartHub();
            if (state.IsKeyDown(Keys.Escape) && !_previousState.IsKeyDown(Keys.Escape)) OpenMenu();
            if (state.IsKeyDown(Keys.F6)) RestartLevelOne();
            if (state.IsKeyDown(Keys.F7)) RestartBossRoom();
            if (state.IsKeyDown(Keys.F2) && !_previousState.IsKeyDown(Keys.F2))
            {
                DebugZoom = DebugZoom == false ? true :  false;
                _camera.Zoom = DebugZoom ? 0.2f : 1.35f;
            }

            if (state.IsKeyDown(Keys.F3))
            {
                PlayerInfo.Money = 5000;
                PlayerInfo.Progress = Level.LevelID.Five;
            }

            if (state.IsKeyDown(Keys.F8))
            {
                Level.ActualState = Level.State.End;
            }
            _camera.Debug.IsVisible = Keyboard.GetState().IsKeyDown(Keys.F1);
            _previousState = state;
        }

        internal void RestartHub()
        {
            _player.PlayerInfo.Save(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Documents\\SaveDP.sav");
            Level.ActualState = Level.State.Hub;
            LoadContent();
            _player.PlayerInfo = PlayerInfo.LoadFrom(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Documents\\SaveDP.sav");
        }
        internal void RestartLevelOne()
        {
            UserInterface.Active.Clear();
            _healthBar = new ProgressBar(0, 100, size: new Vector2(400, 40), offset: new Vector2(10, 10));
            _healthBar.ProgressFill.FillColor = Color.Red;
            _healthBar.Locked = true;
            _energyBar = new ProgressBar(0, 100, size: new Vector2(400, 40), offset: new Vector2(10, 10));
            _energyBar.ProgressFill.FillColor = Color.LightSteelBlue;
            _energyBar.Locked = true;
            _money = new Paragraph("", offset: new Vector2(10, 10))
            {
                Scale = 1
            };
            UserInterface.Active.AddEntity(_healthBar);
            UserInterface.Active.AddEntity(_energyBar);
            UserInterface.Active.AddEntity(_money);
            _menu = new Menu(this);
            _player.PlayerInfo.Save(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Documents\\SaveDP.sav");
            Level.ActualState = Level.State.Level;
            LoadContent();
            _player.PlayerInfo = PlayerInfo.LoadFrom(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Documents\\SaveDP.sav");
        }
        internal void RestartBossRoom()
        {
            _player.PlayerInfo.Save(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Documents\\SaveDP.sav");
            Level.ActualState = Level.State.BossRoom;
            LoadContent();
            _player.PlayerInfo = PlayerInfo.LoadFrom(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Documents\\SaveDP.sav");
        }
        internal void OpenMenu()
        {
            if (Level.ActualState != Level.State.Menu)
            {
                _player.PlayerInfo.Save(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Documents\\SaveDP.sav");
                _menu.PreviousState = Level.ActualState;
                Level.ActualState = Level.State.Menu;
                _player.PlayerInfo = PlayerInfo.LoadFrom(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Documents\\SaveDP.sav");
            }
            else
            {
                Level.ActualState = _menu.PreviousState;
            }

        }

        private void PutJumperInTopLeftCorner()
        {
            PlayerLib.Position = System.Numerics.Vector2.One * 80;
            _player.PlayerLib.Movement = System.Numerics.Vector2.Zero;
        }

        int test2048 = 0;

        void DrawLevelBackground()
        {
            if (Level.ActualState == Level.State.Level)
            {
                Vector2 _spawnPoint = new Vector2(-2048, -2048);
                for (int i = 0; i <= (Level._levelRows * 3); i++)
                {
                    for (int j = 0; j <= (Level._levelColumns * 4); j++)
                    {
                        _spriteBatch.Draw(_levelBackground01, new Vector2((_levelBackground01.Height * j) + _spawnPoint.X, _spawnPoint.Y + (_levelBackground01.Width * i)), Color.White);
                    }
                }
            }
            else if (Level.ActualState == Level.State.Hub || Level.ActualState == Level.State.BossRoom)
            {
                Vector2 _spawnPoint = new Vector2(-2048, -2048);
                for (int i = 0; i <= 5; i++)
                {
                    for (int j = 0; j <= 20; j++)
                    {
                        _spriteBatch.Draw(_levelBackground01, new Vector2((_levelBackground01.Height * j) + _spawnPoint.X, _spawnPoint.Y + (_levelBackground01.Width * i)), Color.White);
                    }
                }
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin(_camera);

            base.Draw(gameTime);
            if (Level.ActualState == Level.State.Hub)
            {
                DrawLevelBackground();
                _spriteBatch.Draw(_hubBackground001, new Vector2(-1000, -575), Color.White);
                _spriteBatch.Draw(_hubBackground002, new Vector2(766, -575), Color.White);
                _spriteBatch.Draw(_hubBackground003, new Vector2(2461, -575), Color.White);
                _NPCWise.Draw();
                _setOfBullet.Draw();
                _NPCWeapon.Draw();
                _NPCWeaponSeller.Draw();
                _setOfFood.Draw();
                _NPCMarchand.Draw();
                _nugget.Draw();
                _NPCNarrator.Draw();
                _cat.Draw();

                for (int x = 0; x < (int)PlayerInfo.Progress; x++)
                {
                    _door[x].Draw();
                }
            }
            if (Level.ActualState == Level.State.Level)
            {
                DrawLevelBackground();
                _door2.Draw();
            }
            if (Level.ActualState == Level.State.BossRoom)
            {
                DrawLevelBackground();
                foreach (Boss boss in Bosses) boss.Draw();
                if (Bosses.Count == 0) _door[0].Draw();
            }
            _board.Draw();
            //WriteDebugInformation();
            _player.Draw();
            foreach (Enemy enemy in Enemys) enemy.Draw();
            foreach (Item item in Items)
            {
                if (item != null && IsOnScreen(new Rectangle(item.ItemLib.Bounds.X, item.ItemLib.Bounds.Y, item.ItemLib.Bounds.Width, item.ItemLib.Bounds.Height))) item.Draw();
            }

            _spriteBatch.End();
            UserInterface.Active.Draw(_spriteBatch);
            if (Level.ActualState == Level.State.End)
            {
                _spriteBatch.Begin(_camera);
                Rectangle rectange = new Rectangle((int)_camera.GetBounds().X,
             (int)_camera.GetBounds().Y + 325,
             (int)(_camera.GetBounds().Width),
             (int)(_camera.GetBounds().Height));
                _spriteBatch.Draw(_end, rectange, Color.White);

                _spriteBatch.End();
            }

            _spriteBatch.Draw(_camera.Debug);
        }

        private void WriteDebugInformation()
        {
            string enemyLifeText;
            string positionInText = string.Format("Position of Jumper: ({0:0.0}, {1:0.0})", PlayerLib.Position.X, _player.position.Y);
            string movementInText = string.Format("Current movement: ({0:0.0}, {1:0.0})", _player.PlayerLib.Movement.X, _player.PlayerLib.Movement.Y);
            string isOnFirmGroundText = string.Format("On firm ground?: {0}", _player.PlayerLib.IsOnFirmGround());
            string playerLifeText = string.Format("PLife: {0}/100", _player.PlayerInfo.Life);
            if (_enemy != null)
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