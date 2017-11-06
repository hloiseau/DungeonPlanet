using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ennemi.sprites
{
    public class Hero : Sprite
    {
        internal Hero(Texture2D texture)
            : base(texture) 
        {
        }

        public override void Update(GameTime gametime, List<Sprite> sprites)
        {
            _previousKey = _currentKey;
            _currentKey = Keyboard.GetState();

            if (_currentKey.IsKeyDown(Keys.Right)) PositionX += LinearVelocity;
            if (_currentKey.IsKeyDown(Keys.Left)) PositionX -= LinearVelocity;
            if (_currentKey.IsKeyDown(Keys.Up)) PositionY -= LinearVelocity;
            if (_currentKey.IsKeyDown(Keys.Down)) PositionY += LinearVelocity;
        }

        
    }
}
