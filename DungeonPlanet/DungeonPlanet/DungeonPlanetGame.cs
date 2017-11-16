using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Comora;

namespace DungeonPlanet
{
    /// <summary>
    /// Simple code for a platformer game
    /// Created in 2013 by Jakob "xnafan" Krarup
    /// http://www.xnafan.net
    /// Distribute and reuse freely, but please leave this comment
    /// </summary>

    public class DungeonPlanetGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D _tileTexture, _playerTexture, _weaponTexture, _bulletTexture, _mediTexture;
        private Player _player;
        private Board _board;
        private Random _rnd = new Random();
        private MediPack _mediPack;
        private SpriteFont _debugFont;
        private Camera _camera;

        public DungeonPlanetGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
            IsMouseVisible = true;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _tileTexture = Content.Load<Texture2D>("tile");
            _playerTexture = Content.Load<Texture2D>("player");
            _weaponTexture = Content.Load<Texture2D>("player_arm");
            _bulletTexture = Content.Load<Texture2D>("bullet");
            _mediTexture = Content.Load<Texture2D>("Medipack");
            _player = new Player(_playerTexture, _weaponTexture, _bulletTexture, this, new Vector2(80, 80), _spriteBatch);
            _mediPack = new MediPack(_mediTexture, new Vector2(300, 300), _spriteBatch, 45, _player);
            _board = new Board(_spriteBatch, _tileTexture, 15, 10);
            _debugFont = Content.Load<SpriteFont>("DebugFont");
            _camera = new Camera(GraphicsDevice);
            _camera.LoadContent(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            _camera.Update(gameTime);
            _player.Update(gameTime);
            if(_mediPack != null)
            {
                _mediPack.Update();
                if (_mediPack.IsFinished)
                {
                    _mediPack = null;
                }
            }
            if (_player.PlayerLib.IsDead(_player.Life)) RestartGame();
            _camera.Position = _player.Position;
            CheckKeyboardAndReact();
        }

        private void CheckKeyboardAndReact()
        {
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.F5)) { RestartGame(); }
            if (state.IsKeyDown(Keys.Escape)) { Exit(); }
            _camera.Debug.IsVisible = Keyboard.GetState().IsKeyDown(Keys.F1);

        }

        private void RestartGame()
        {
            /*Board.CurrentBoard.CreateNewBoard();
            PutJumperInTopLeftCorner();*/
            LoadContent();


        }

        private void PutJumperInTopLeftCorner()
        {
            _player.PlayerLib.Position = System.Numerics.Vector2.One * 80;
            _player.PlayerLib.Movement = System.Numerics.Vector2.Zero;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin(_camera);
            base.Draw(gameTime);
            _board.Draw();
            if (_mediPack != null) _mediPack.Draw();
            WriteDebugInformation();
            _player.Draw();
            _spriteBatch.End();
            _spriteBatch.Draw(gameTime, _camera.Debug);
        }

        private void WriteDebugInformation()
        {
            string positionInText = string.Format("Position of Jumper: ({0:0.0}, {1:0.0})", _player.PlayerLib.Position.X, _player.Position.Y);
            string movementInText = string.Format("Current movement: ({0:0.0}, {1:0.0})", _player.PlayerLib.Movement.X, _player.PlayerLib.Movement.Y);
            string isOnFirmGroundText = string.Format("On firm ground?: {0}", _player.PlayerLib.IsOnFirmGround());
            string playerLifeText = string.Format("Life: {0}/100", _player.Life);

            DrawWithShadow(positionInText, new Vector2(10, 0));
            DrawWithShadow(movementInText, new Vector2(10, 20));
            DrawWithShadow(isOnFirmGroundText, new Vector2(10, 40));
            DrawWithShadow("F5 for random board", new Vector2(70, 600));
            DrawWithShadow(playerLifeText, new Vector2(70, 620));


        }

        private void DrawWithShadow(string text, Vector2 position)
        {
            _spriteBatch.DrawString(_debugFont, text, position + Vector2.One, Color.Black);
            _spriteBatch.DrawString(_debugFont, text, position, Color.LightYellow);
        }
    }
}