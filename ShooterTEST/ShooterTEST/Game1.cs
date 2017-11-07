using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ShooterTEST.Sprites;
using System.Collections.Generic;

namespace ShooterTEST
{

    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private List<Sprite> _sprites;

        public float ScreenHeight { get => GraphicsDevice.Viewport.Height; }
        public float ScreenWidth { get => GraphicsDevice.Viewport.Width; }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            var weaponTexture = Content.Load<Texture2D>("caracter_arm");
            var playerTexture = Content.Load<Texture2D>("caracter");
            _sprites = new List<Sprite>()
            {
                new Weapon(weaponTexture, this)
                {
                    Position = new Vector2(100, 100),
                    Bullet = new Bullet(Content.Load<Texture2D>("bullet")),
                },
                new Player(playerTexture, this)
                {
                    Position = new Vector2(100, 100)
                }
            };
        }


        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            foreach (var sprite in _sprites.ToArray())
                sprite.Update(gameTime, _sprites);

            PostUpdate();

            base.Update(gameTime);
        }

        private void PostUpdate()
        {
            for (int i = 0; i < _sprites.Count; i++)
            {
                if (_sprites[i].IsRemooved)
                {
                    _sprites.RemoveAt(i);
                    i--;
                }
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            foreach (var sprite in _sprites)
                sprite.Draw(spriteBatch);
            spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
