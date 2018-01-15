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
    public class NPCNarrator : Sprite
    {
        SpriteBatch _spritebatch;
        Header _header;
        Header _headerMessage;
        Player _player;
        EnemyLib ReferenceLib { get; set; }
        NPCDialogLib Lib { get; set; }
        string Sentence { get; set; }
        Panel NPCPanel { get; set; }

        public NPCNarrator(Texture2D texture, Vector2 position, SpriteBatch spriteBatch)
            : base(texture, position, spriteBatch)
        {
            ReferenceLib = new EnemyLib(new System.Numerics.Vector2(position.X, position.Y), texture.Width, texture.Height, 100);
            _spritebatch = spriteBatch;
            _player = Player.CurrentPlayer;
            Lib = new NPCDialogLib();
        }

        public void ShowMessage()
        {
            _header = new Header("Appuyer sur E pour parler", Anchor.AutoCenter);
            UserInterface.Active.AddEntity(_header);
        }

        public void ShowMenu()
        {
            _headerMessage = new Header("R pour quitter", Anchor.AutoCenter);
            UserInterface.Active.AddEntity(_headerMessage);

            NPCPanel = new Panel(size: new Vector2(1200, 200), skin: PanelSkin.Golden, anchor: Anchor.AutoCenter, offset: new Vector2(10, 10));
            UserInterface.Active.AddEntity(NPCPanel);
            NPCPanel.AddChild(new Header("Le chroniqueur", Anchor.TopLeft));
            NPCPanel.AddChild(new HorizontalLine());
            Sentence = Lib.ChooseSentenceForChronicler();

            Paragraph paragraph = new Paragraph(Sentence);
            NPCPanel.AddChild(paragraph);
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            MouseState mouseState = Mouse.GetState();

            ReferenceLib.AffectWithGravity();
            ReferenceLib.SimulateFriction();
            ReferenceLib.MoveAsFarAsPossible((float)gameTime.ElapsedGameTime.TotalMilliseconds / 15);
            ReferenceLib.StopMovingIfBlocked();
            position = new Vector2(ReferenceLib.Position.X, ReferenceLib.Position.Y);

            if (NPCPanel == null && Player.CurrentPlayer.PlayerLib.Bounds.IntersectsWith(ReferenceLib.Bounds) && keyboardState.IsKeyDown(Keys.E))
            {
                ShowMenu();
                if (_header != null)
                {
                    UserInterface.Active.RemoveEntity(_header);
                    _header = null;
                }
            }
            else if (NPCPanel == null && _header == null && Player.CurrentPlayer.PlayerLib.Bounds.IntersectsWith(ReferenceLib.Bounds))
            {
                ShowMessage();
            }
            else if (!Player.CurrentPlayer.PlayerLib.Bounds.IntersectsWith(ReferenceLib.Bounds))
            {
                if (NPCPanel != null)
                {
                    UserInterface.Active.RemoveEntity(NPCPanel);
                    UserInterface.Active.RemoveEntity(_headerMessage);
                    NPCPanel = null;
                    _headerMessage = null;
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
                    _headerMessage = null;
                    NPCPanel = null;
                }
            }
        }
    }
}
