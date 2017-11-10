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

        
        public Player(Texture2D texture, Vector2 position, SpriteBatch spritebatch)
            : base(texture, position, spritebatch)
        {
            PlayerLib = new PlayerLib(new System.Numerics.Vector2(position.X,position.Y), texture.Width, texture.Height);
        }
        
        public void Update(GameTime gameTime)
        {
            CheckKeyboardAndUpdateMovement();
            PlayerLib.AffectWithGravity();
            PlayerLib.SimulateFriction();
            PlayerLib.MoveAsFarAsPossible((float)gameTime.ElapsedGameTime.TotalMilliseconds / 15);
            PlayerLib.StopMovingIfBlocked();
            Position = new Vector2(PlayerLib.Position.X, PlayerLib.Position.Y);

        }

        private void CheckKeyboardAndUpdateMovement()
        {
            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Left)) { PlayerLib.Left(); }
            if (keyboardState.IsKeyDown(Keys.Right)) { PlayerLib.Right(); }
            if (keyboardState.IsKeyDown(Keys.Space) && PlayerLib.IsOnFirmGround()) { PlayerLib.Jump(); }
        }
    }
}
