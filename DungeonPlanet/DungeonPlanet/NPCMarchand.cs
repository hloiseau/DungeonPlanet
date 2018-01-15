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
    public class NPCMarchand : Sprite
    {
        SpriteBatch _spritebatch;
        EnemyLib _lib;
        Header _header;
        Header _headerMessage;
        Player _player;
        Icon _button;
        Icon _button2;

        Panel NPCPanel { get; set; }
        public NPCMarchand(Texture2D texture, Vector2 position, SpriteBatch spriteBatch)
            : base(texture, position, spriteBatch)
        {
            _lib = new EnemyLib(new System.Numerics.Vector2(position.X, position.Y), texture.Width, texture.Height, 100);
            _spritebatch = spriteBatch;
            _player = Player.CurrentPlayer;
            _button = new Icon(IconType.PotionRed, Anchor.Auto);
            _button2 = new Icon(IconType.Apple, Anchor.Auto);

        }

        public void ShowMessage()
        {
            _header = new Header("Appuyer sur E voir le marche", Anchor.AutoCenter);
            UserInterface.Active.AddEntity(_header);
        }

        public void ShowMenu()
        {
            _headerMessage = new Header("R pour quitter", Anchor.AutoCenter);
            UserInterface.Active.AddEntity(_headerMessage);
            NPCPanel = new Panel(size: new Vector2(500, 500), skin: PanelSkin.Golden, anchor: Anchor.Center, offset: new Vector2(10, 10));
            UserInterface.Active.AddEntity(NPCPanel);
            NPCPanel.AddChild(new Header("Magasin", Anchor.AutoCenter));
            NPCPanel.AddChild(new HorizontalLine());
            _button = new Icon(IconType.PotionRed, Anchor.Auto);
            _button2 = new Icon(IconType.Apple, Anchor.Auto);

            NPCPanel.AddChild(new Label(" 30 $", Anchor.Auto));
            NPCPanel.AddChild(_button);

            NPCPanel.AddChild(new Label(" 5 $", Anchor.Auto));
            NPCPanel.AddChild(_button2);
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            MouseState mouseState = Mouse.GetState();

            _lib.AffectWithGravity();
            _lib.SimulateFriction();
            _lib.MoveAsFarAsPossible((float)gameTime.ElapsedGameTime.TotalMilliseconds / 15);
            _lib.StopMovingIfBlocked();
            position = new Vector2(_lib.Position.X, _lib.Position.Y);

            if (_button != null)
            {
                _button.OnClick = (Entity btn) =>
                {
                    if (_player.PlayerInfo.Money - 30 >= 0)
                    {
                        _player.PlayerInfo.Money -= 30;
                        _player.PlayerInfo.Life += 40;
                    }
                };
            }

            if (_button2 != null)
            {
                _button2.OnClick = (Entity btn) =>
                {
                    if (_player.PlayerInfo.Money - 5 >= 0)
                    {
                        _player.PlayerInfo.Money -= 5;
                        _player.PlayerInfo.Life += 10;
                    }
                };
            }


            if (NPCPanel == null && Player.CurrentPlayer.PlayerLib.Bounds.IntersectsWith(_lib.Bounds) && keyboardState.IsKeyDown(Keys.E))
            {
                ShowMenu();
                if (_header != null)
                {
                    UserInterface.Active.RemoveEntity(_header);
                    _header = null;
                }
            }
            else if (NPCPanel == null && _header == null && Player.CurrentPlayer.PlayerLib.Bounds.IntersectsWith(_lib.Bounds))
            {
                ShowMessage();
            }
            else if (!Player.CurrentPlayer.PlayerLib.Bounds.IntersectsWith(_lib.Bounds))
            {
                if (NPCPanel != null)
                {
                    UserInterface.Active.RemoveEntity(_headerMessage);
                    UserInterface.Active.RemoveEntity(NPCPanel);
                    _headerMessage = null;
                    NPCPanel = null;
                    _button = null;
                }
                if (_header != null)
                {
                    UserInterface.Active.RemoveEntity(_header);
                    _header = null;
                }
            }
            else if (NPCPanel != null && keyboardState.IsKeyDown(Keys.R))
            {
                if (NPCPanel != null)
                {
                    UserInterface.Active.RemoveEntity(_headerMessage);
                    UserInterface.Active.RemoveEntity(NPCPanel);
                    NPCPanel = null;
                    _headerMessage = null;
                    _button = null;
                }
            }
        }
    }
}
