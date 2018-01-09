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
    public class NPC : Sprite
    {
        SpriteBatch _spritebatch;
        EnemyLib _lib;
        Header _header;
        Player _player;
        Icon _button;

        Panel NPCPanel { get; set; }
        public NPC(Texture2D texture, Vector2 position, SpriteBatch spriteBatch)
            : base(texture, position, spriteBatch)
        {
            _lib = new EnemyLib(new System.Numerics.Vector2(position.X, position.Y), texture.Width, texture.Height, 100);
            _spritebatch = spriteBatch;
            _player = Player.CurrentPlayer;
            _button = new Icon(IconType.PotionRed, Anchor.Auto);

        }

        public void ShowMessage()
        {
            _header = new Header("Appuyer sur E pour interagir", Anchor.AutoCenter);
            UserInterface.Active.AddEntity(_header);
        }

        public void ShowMenu()
        {
            NPCPanel = new Panel(size: new Vector2(500, 500), skin: PanelSkin.Golden, anchor: Anchor.Center, offset: new Vector2(10, 10));
            UserInterface.Active.AddEntity(NPCPanel);
            NPCPanel.AddChild(new Header("Magasin", Anchor.AutoCenter));
            NPCPanel.AddChild(new HorizontalLine());
            _button = new Icon(IconType.PotionRed, Anchor.Auto);
            /*
q                NPCPanel.AddChild(new Label(" 10 $", Anchor.Auto));
            */
            NPCPanel.AddChild(new Paragraph("Vie"));
            NPCPanel.AddChild(_button);
        }



        public void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            _lib.AffectWithGravity();
            _lib.SimulateFriction();
            _lib.MoveAsFarAsPossible((float)gameTime.ElapsedGameTime.TotalMilliseconds / 15);
            _lib.StopMovingIfBlocked();
            position = new Vector2(_lib.Position.X, _lib.Position.Y);
            if (_button != null &&_button.IsMouseDown)
            {
                _player.PlayerInfo.Money -= 10;
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
                    UserInterface.Active.RemoveEntity(NPCPanel);
                    NPCPanel = null;
                    _button = null;
                }
                if (_header != null)
                {
                    UserInterface.Active.RemoveEntity(_header);
                    _header = null;
                }
            }
        }
    }
}
