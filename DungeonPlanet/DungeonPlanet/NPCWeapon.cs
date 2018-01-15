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
    public class NPCWeapon : Sprite
    {
        SpriteBatch _spritebatch;
        EnemyLib _lib;
        Header _header;
        Header _headerMessage;
        Player _player;
        Icon _redBullet;
        Icon _blueBullet;
        Icon _greenBullet;
        bool _alreadyBuy { get; set; }

        Panel NPCPanel { get; set; }

        public NPCWeapon(Texture2D texture, Vector2 position, SpriteBatch spriteBatch)
            : base(texture, position, spriteBatch)
        {
            _lib = new EnemyLib(new System.Numerics.Vector2(position.X, position.Y), texture.Width, texture.Height, 100);
            _spritebatch = spriteBatch;
            _player = Player.CurrentPlayer;
            _redBullet = new Icon(IconType.OrbRed, Anchor.Auto);
            _blueBullet = new Icon(IconType.OrbBlue, Anchor.Auto);
            _greenBullet = new Icon(IconType.OrbGreen, Anchor.Auto);
            _alreadyBuy = false;
        }

        public void ShowMessage()
        {
            _header = new Header("Appuyer sur E pour voir la boutique", Anchor.AutoCenter);
            UserInterface.Active.AddEntity(_header);
        }
        public void ShowMenu()
        {
            _headerMessage = new Header("R pour quitter", Anchor.AutoCenter);
            UserInterface.Active.AddEntity(_headerMessage);
            NPCPanel = new Panel(size: new Vector2(500, 500), skin: PanelSkin.Golden, anchor: Anchor.Center, offset: new Vector2(10, 10));
            UserInterface.Active.AddEntity(NPCPanel);
            NPCPanel.AddChild(new Header("Magasin", Anchor.AutoCenter));
            _redBullet = new Icon(IconType.OrbRed, Anchor.Auto);
            _blueBullet = new Icon(IconType.OrbBlue, Anchor.Auto);
            _greenBullet = new Icon(IconType.OrbGreen, Anchor.Auto);

            HorizontalLine hz1 = new HorizontalLine();
            HorizontalLine hz2 = new HorizontalLine();

            Paragraph redText = new Paragraph("Emflamme l'ennemi");
            Paragraph blueText = new Paragraph("Des munitions basiques");
            Paragraph greenText = new Paragraph("Ralenti l'ennemi");

            NPCPanel.AddChild(new Label(" 10 $", Anchor.Auto));
            NPCPanel.AddChild(_blueBullet);
            NPCPanel.AddChild(blueText);
            NPCPanel.AddChild(hz1);
            NPCPanel.AddChild(new Label(" 250 $", Anchor.Auto));
            NPCPanel.AddChild(_redBullet);
            NPCPanel.AddChild(redText);
            NPCPanel.AddChild(hz2);
            NPCPanel.AddChild(new Label(" 100 $", Anchor.Auto));
            NPCPanel.AddChild(_greenBullet);
            NPCPanel.AddChild(greenText);
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
            if (_blueBullet != null && _blueBullet.IsMouseDown && _player.PlayerInfo.Money - 10 >= 0)
            {
                _player.PlayerInfo.Money -= 10;
                PlayerInfo.ActualWeapon = PlayerInfo.WeaponState.None;
            }
            if (_redBullet != null && _redBullet.IsMouseDown && _player.PlayerInfo.Money - 250 >= 0)
            {
                _player.PlayerInfo.Money -= 250;
                PlayerInfo.ActualWeapon = PlayerInfo.WeaponState.Fire;
            }
            if (_greenBullet != null && _greenBullet.IsMouseDown && _player.PlayerInfo.Money - 100 >= 0)
            {
                _player.PlayerInfo.Money -= 100;
                PlayerInfo.ActualWeapon = PlayerInfo.WeaponState.Slime;
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
                    NPCPanel = null;
                    _headerMessage = null;
                    _redBullet = null;
                    _blueBullet = null;
                    _greenBullet = null;
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
                    _redBullet = null;
                    _blueBullet = null;
                    _greenBullet = null;
                }
            }
        }
    }
}
