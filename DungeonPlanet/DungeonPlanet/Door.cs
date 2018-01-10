using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using DungeonPlanet.Library;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GeonBit.UI;
using GeonBit.UI.Entities;

namespace DungeonPlanet
{
    class Door: Sprite
    {
        SpriteBatch _spritebatch;
        EnemyLib _lib;
        Header _header;
        DungeonPlanetGame _ctx;
        public Door(Texture2D texture, Vector2 position, SpriteBatch spriteBatch, DungeonPlanetGame ctx)
            : base(texture, position, spriteBatch)
        {
            _lib = new EnemyLib(new System.Numerics.Vector2(position.X, position.Y), texture.Width, texture.Height, 100);
            _spritebatch = spriteBatch;
            _ctx = ctx;
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            _lib.AffectWithGravity();
            _lib.SimulateFriction();
            _lib.MoveAsFarAsPossible((float)gameTime.ElapsedGameTime.TotalMilliseconds / 15);
            _lib.StopMovingIfBlocked();
            position = new Vector2(_lib.Position.X, _lib.Position.Y);
            if (Player.CurrentPlayer.PlayerLib.Bounds.IntersectsWith(_lib.Bounds) && keyboardState.IsKeyDown(Keys.E))
            {
                if (Level.ActualState == Level.State.Hub)
                {
                    _ctx.RestartLevelOne();
                    if (_header != null)
                    {
                        UserInterface.Active.RemoveEntity(_header);
                        _header = null;
                    }
                }
                else if (Level.ActualState == Level.State.LevelOne)
                {
                    _ctx.RestartBossRoom();
                    if (_header != null)
                    {
                        UserInterface.Active.RemoveEntity(_header);
                        _header = null;
                    }
                }
                else if (Level.ActualState == Level.State.BossRoom)
                {
                    _ctx.RestartHub();
                    if (_header != null)
                    {
                        UserInterface.Active.RemoveEntity(_header);
                        _header = null;
                    }
                }
            }
            else if (_header == null && Player.CurrentPlayer.PlayerLib.Bounds.IntersectsWith(_lib.Bounds))
            {
                _header = new Header("Appuyer sur E pour interagir", Anchor.AutoCenter);
                UserInterface.Active.AddEntity(_header);
            }
            else if (!Player.CurrentPlayer.PlayerLib.Bounds.IntersectsWith(_lib.Bounds))
            {
                if (_header != null)
                {
                    UserInterface.Active.RemoveEntity(_header);
                    _header = null;
                }
            }
        }
    }
}
