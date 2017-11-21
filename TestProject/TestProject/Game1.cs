using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TestProject
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;
        Player _player;
        KeyboardState _currentKeyboardState;
        KeyboardState _previousKeyboardState;
        float _playerMoveSpeed;
        // Image used to display the static background
        Texture2D _mainBackground;
        Rectangle _rectBackground;
        ParallaxingBackground _bgLayer1;
        ParallaxingBackground _bgLayer2;
        Animation _playerAnimation;

        bool _jumping; //Is the character jumping?
        float _startY; 
        float _jumpspeed; //startY to tell us //where it lands, jumpspeed to see how fast it jumps
        

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            _player = new Player();
            //Background
            _bgLayer1 = new ParallaxingBackground();
            _bgLayer2 = new ParallaxingBackground();
            _playerMoveSpeed = 8.0f;
            
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            // Load the player resources

            _playerAnimation = new Animation();
            Texture2D playerTexture = Content.Load<Texture2D>("Bear");
            _playerAnimation.Initialize(playerTexture, Vector2.Zero, 32, 32, 0, 1, 4, 100, Color.White, 2f, true);
            Vector2 playerPosition = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X, GraphicsDevice.Viewport.TitleSafeArea.Y + GraphicsDevice.Viewport.TitleSafeArea.Height / 2);
            _player.Initialize(_playerAnimation, playerPosition);
            // Load the parallaxing background
            _bgLayer1.Initialize(Content, "Area4Capsules4", GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, -1);
            _bgLayer2.Initialize(Content, "Area4PurpleBushColumn", GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, -2);

            _mainBackground = Content.Load<Texture2D>("Area4PurpleBushColumn");

            _startY = GraphicsDevice.Viewport.Height;//Starting position
            _jumping = false;//Init jumping to false
            _jumpspeed = 0;//Default no speed
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        private void PlayerUpdate(GameTime gameTime)
        {
            int ori = 0;
            _player.Update(gameTime);
            if (_currentKeyboardState.IsKeyDown(Keys.Left) || _currentKeyboardState.IsKeyDown(Keys.Q))
            {
                _player._position.X -= _playerMoveSpeed / 2;
                ori = 3;
            }
            if (_currentKeyboardState.IsKeyDown(Keys.Right) || _currentKeyboardState.IsKeyDown(Keys.D))
            {
                _player._position.X += _playerMoveSpeed / 2;
                ori = 1;
            }
            if (_currentKeyboardState.IsKeyDown(Keys.Up) || _currentKeyboardState.IsKeyDown(Keys.Z))
            {
                _player._position.Y -= _playerMoveSpeed;
            }

            if (_jumping)
            {
                _player._position.Y += _jumpspeed;//Making it go up
                _jumpspeed += 1;//Some math (explained later)
                if (_player._position.Y >= _startY)
                //If it's farther than ground
                {
                    _player._position.Y = _startY;//Then set it on
                    _jumping = false;
                }
            }
            else
            {
                if (_currentKeyboardState.IsKeyDown(Keys.Space))
                {
                    _jumping = true;
                    _jumpspeed = -25;//Give it upward thrust
                }
            }

            _playerAnimation.CurrentFrameLin = ori;

            _player._position.Y += _playerMoveSpeed / 2;

            _player._position.X = MathHelper.Clamp(_player._position.X, _player.Width, GraphicsDevice.Viewport.Width - _player.Width);

            _player._position.Y = MathHelper.Clamp(_player._position.Y, _player.Height, GraphicsDevice.Viewport.Height - _player.Height);
        }
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Save the previous state of the keyboard  so we can determine single key presses
            _previousKeyboardState = _currentKeyboardState;

            // Read the current state of the keyboard and gamepad and store it
            _currentKeyboardState = Keyboard.GetState();

            //Update the player
            PlayerUpdate(gameTime);

            // Update the parallaxing background
            _bgLayer1.Update(gameTime);
            _bgLayer2.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            _bgLayer2.Draw(_spriteBatch);
            _bgLayer1.Draw(_spriteBatch);

            _player.Draw(_spriteBatch);

            //Draw the Main Background Texture
            _spriteBatch.Draw(_mainBackground, _rectBackground, Color.White);

            // Draw the moving background


            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
