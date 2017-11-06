using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Comora;

namespace Platformer
{
    /// <summary>
    /// Simple code for a platformer game
    /// Created in 2013 by Jakob "xnafan" Krarup
    /// http://www.xnafan.net
    /// Distribute and reuse freely, but please leave this comment
    /// </summary>

    public class SimplePlatformerGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D _tileTexture, _jumperTexture;
        private Jumper _jumper;
        private Board _board;
        private Random _rnd = new Random();
        private SpriteFont _debugFont;
        private Camera _camera;

        public SimplePlatformerGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            _graphics.PreferredBackBufferWidth = 960;
            _graphics.PreferredBackBufferHeight = 640;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _tileTexture = Content.Load<Texture2D>("tile");
            _jumperTexture = Content.Load<Texture2D>("jumper");
            _jumper = new Jumper(_jumperTexture, new Vector2(80, 80), _spriteBatch);
            _board = new Board(_spriteBatch, _tileTexture, 15, 10);
            _debugFont = Content.Load<SpriteFont>("DebugFont");
            _camera = new Camera(GraphicsDevice);
            _camera.LoadContent(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            _camera.Update(gameTime);
            _jumper.Update(gameTime);
            _camera.Position = _jumper.Position;
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
            Board.CurrentBoard.CreateNewBoard();
            PutJumperInTopLeftCorner();
        }

        private void PutJumperInTopLeftCorner()
        {
            _jumper.Position = Vector2.One * 80;
            _jumper.Movement = Vector2.Zero;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin(_camera);
            base.Draw(gameTime);
            _board.Draw();
            WriteDebugInformation();
            _jumper.Draw();
            _spriteBatch.End();
            _spriteBatch.Draw(gameTime, _camera.Debug);
        }

        private void WriteDebugInformation()
        {
            string positionInText = string.Format("Position of Jumper: ({0:0.0}, {1:0.0})", _jumper.Position.X, _jumper.Position.Y);
            string movementInText = string.Format("Current movement: ({0:0.0}, {1:0.0})", _jumper.Movement.X, _jumper.Movement.Y);
            string isOnFirmGroundText = string.Format("On firm ground? : {0}", _jumper.IsOnFirmGround());

            DrawWithShadow(positionInText, new Vector2(10, 0));
            DrawWithShadow(movementInText, new Vector2(10, 20));
            DrawWithShadow(isOnFirmGroundText, new Vector2(10, 40));
            DrawWithShadow("F5 for random board", new Vector2(70, 600));
        }

        private void DrawWithShadow(string text, Vector2 position)
        {
            _spriteBatch.DrawString(_debugFont, text, position + Vector2.One, Color.Black);
            _spriteBatch.DrawString(_debugFont, text, position, Color.LightYellow);
        }
    }
}