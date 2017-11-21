using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace ennemi.sprites
{
    class Ennemi : Sprite
    {
        float _distance;
        float _oldDistance;
        Vector2 _velocity;
        Game1 _ctx;

        bool right;

        internal Ennemi(Texture2D texture, float NewDistance, Game1 ctx)
            : base(texture)
        {
            _ctx = ctx;
            _distance = NewDistance;

            _oldDistance = _distance;
        }

        public override void Update(GameTime gametime, List<Sprite> sprites)
        {
           /* Vector2 destination = FindHero(sprites);
            if (Position == FindHero(sprites))
            {*/
                Position += _velocity;
                if (_distance == 0)
                {
                    right = true;
                    _velocity.X = 1f;
                }
                else if (_distance >= _oldDistance)
                {
                    right = false;
                    _velocity.X = -1f;
                }

                if (right) _distance += 1;
                else _distance -= 1;
            //}
        }

        Vector2 FindHero(List<Sprite> sprites)
        {
            foreach (Hero hero in sprites)
            {
                float distance = GetDistanceTo(hero.Position);
                Vector2 direction = new Vector2(hero.Position.X, hero.Position.Y);
                return Position = direction * _velocity;
            }
            return Position;
        }

        float GetDistanceTo(Vector2 heroPosition)
        {
            double temp = Math.Sqrt(Math.Pow(heroPosition.X - Position.X, 2) + Math.Pow(0 - 0, 2));
            float distance = (float)temp;
            return distance;
        }
    }
}
