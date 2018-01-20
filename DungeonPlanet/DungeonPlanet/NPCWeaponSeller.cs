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
    public class NPCWeaponSeller : Sprite
    {
        SpriteBatch _spritebatch;
        EnemyLib _lib;
        Header _header;
        Header _headerMessage;
        Player _player;
        Image _shotgun;
        Image _launcher;
        Animation _animation;
        bool _alreadyBuy { get; set; }

        Panel NPCPanel { get; set; }

        public NPCWeaponSeller(Texture2D texture, Texture2D shotgun, Texture2D launcher, Vector2 position, SpriteBatch spriteBatch)
            : base(texture, position, spriteBatch)
        {
            _lib = new EnemyLib(new System.Numerics.Vector2(position.X, position.Y), texture.Width, texture.Height, 100);
            _spritebatch = spriteBatch;
            _animation = new Animation();
            _animation.Initialize(texture, position, 55, 64, 0, 0, 5, 150, Color.White, 0.9f, true, true);
            _player = Player.CurrentPlayer;
            _shotgun = new Image(shotgun, new Vector2(100, 50));
            _launcher = new Image(launcher, new Vector2(100, 50));
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
            NPCPanel.AddChild(new Header("Prototype", Anchor.AutoCenter));
           /* _shotgun = new Image(_shotgunTexture, new Vector2(100, 50));
            _launcher = new Image(_launcherTexture, new Vector2(100, 50));*/

            HorizontalLine hz1 = new HorizontalLine();

            Paragraph redText = new Paragraph("Transforme votre arme en un puissant Lance Roquette");
            Paragraph blueText = new Paragraph("transforme votre arme en Shotgun");
            if((Player.CurrentPlayer.PlayerInfo.Unlocked & PlayerInfo.WeaponState.Shotgun) == 0)
            {
                NPCPanel.AddChild(new Label(" 500 $", Anchor.Auto));
                NPCPanel.AddChild(_shotgun);
                NPCPanel.AddChild(blueText);
                NPCPanel.AddChild(hz1);
            }
            if ((Player.CurrentPlayer.PlayerInfo.Unlocked & PlayerInfo.WeaponState.Launcher) == 0)
            {
                NPCPanel.AddChild(new Label(" 1000 $", Anchor.Auto));
                NPCPanel.AddChild(_launcher);
                NPCPanel.AddChild(redText);
            }
                    
        }

        public void Update(GameTime gameTime)
        {
            _animation.Update(gameTime);
            _animation.Position = new Vector2(_lib.Position.X, _lib.Position.Y + 36);
            KeyboardState keyboardState = Keyboard.GetState();

            MouseState mouseState = Mouse.GetState();

            _lib.AffectWithGravity();
            _lib.SimulateFriction();
            _lib.MoveAsFarAsPossible((float)gameTime.ElapsedGameTime.TotalMilliseconds / 15);
            _lib.StopMovingIfBlocked();
            position = new Vector2(_lib.Position.X, _lib.Position.Y);

            if (_launcher != null)
            {
                _launcher.OnClick = (Entity btn) =>
                {
                    if(_player.PlayerInfo.Money - 1000 >= 0)
                    {
                        _player.PlayerInfo.Money -= 1000;
                        Player.CurrentPlayer.PlayerInfo.Unlocked |= PlayerInfo.WeaponState.Launcher;
                    }
                   
                };
            }
            if (_shotgun != null)
            {
                _shotgun.OnClick = (Entity btn) =>
                {
                    if (_player.PlayerInfo.Money - 500 >= 0)
                    {
                        _player.PlayerInfo.Money -= 500;
                        Player.CurrentPlayer.PlayerInfo.Unlocked |= PlayerInfo.WeaponState.Shotgun;
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
                    NPCPanel = null;
                    _headerMessage = null;

                }
            }
        }
        public override void Draw()
        {
            _animation.Draw(SpriteBatch);
        }
    }
}

