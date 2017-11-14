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
        public  Weapon Weapon { get; set; }


        public Player(Texture2D texturePlayer, Texture2D textureWeapon, Texture2D textureBullet, DungeonPlanetGame ctx, Vector2 position, SpriteBatch spritebatch)
            : base(texturePlayer, position, spritebatch)
        {
            PlayerLib = new PlayerLib(new System.Numerics.Vector2(position.X,position.Y), texturePlayer.Width, texturePlayer.Height);
            Weapon = new Weapon(textureWeapon, textureBullet, ctx, position, spritebatch, PlayerLib);
        }
        
        public void Update(GameTime gameTime)
        {
            CheckKeyboardAndUpdateMovement();
            PlayerLib.AffectWithGravity();
            PlayerLib.SimulateFriction();
            PlayerLib.MoveAsFarAsPossible((float)gameTime.ElapsedGameTime.TotalMilliseconds / 15);
            PlayerLib.StopMovingIfBlocked();
            Position = new Vector2(PlayerLib.Position.X, PlayerLib.Position.Y);
            Weapon.Update(gameTime);
        }

        private void CheckKeyboardAndUpdateMovement()
        {
            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Q)) { PlayerLib.Left(); }
            if (keyboardState.IsKeyDown(Keys.D)) { PlayerLib.Right(); }
            if (keyboardState.IsKeyDown(Keys.Z) && PlayerLib.IsOnFirmGround()) { PlayerLib.Jump(); }
        }

        public override void Draw()
        {
            base.Draw();
            Weapon.Draw();
        }

    }
}
