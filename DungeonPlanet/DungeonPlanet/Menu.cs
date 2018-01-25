using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using DungeonPlanet.Library;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using GeonBit.UI;
using GeonBit.UI.Entities;

namespace DungeonPlanet
{
    public class Menu
    {
        Panel _panel;
        Button _newGame;
        Button _loadGame;
        Button _options;
        Button _quit;
        Button _continue;
        Panel _pOption;
        CheckBox _fullscreen;
        Slider _music;
        Slider _effect;
        Button _return;
        DungeonPlanetGame _ctx;
        Level.State _previousState;
        public Menu(DungeonPlanetGame ctx)
        {

            _ctx = ctx;
            _panel = new Panel(new Vector2(1300, 750));
            UserInterface.Active.AddEntity(_panel);
            _panel.AddChild(new Header("Dungeon Planet"));
            _panel.AddChild(new LineSpace());
            _panel.AddChild(new HorizontalLine());
            _panel.AddChild(new LineSpace());
            _continue = new Button("Reprendre", ButtonSkin.Default, Anchor.AutoCenter, new Vector2(1000, 120));
            _panel.AddChild(_continue);
            _newGame = new Button("Nouvelle Partie", ButtonSkin.Default, Anchor.AutoCenter, new Vector2(1000, 120));
            _panel.AddChild(_newGame);
            _loadGame = new Button("Charger Partie", ButtonSkin.Default, Anchor.AutoCenter, new Vector2(1000, 120));
            _panel.AddChild(_loadGame);
            _options = new Button("Option", ButtonSkin.Default, Anchor.AutoCenter, new Vector2(1000, 120));
            _panel.AddChild(_options);
            _quit = new Button("Quitter", ButtonSkin.Default, Anchor.AutoCenter, new Vector2(1000, 120));
            _panel.AddChild(_quit);

            _pOption = new Panel(new Vector2(1300, 750));
            UserInterface.Active.AddEntity(_pOption);
            _pOption.AddChild(new Header("Options"));
            _fullscreen = new CheckBox("Pleine Ecran");
            _pOption.AddChild(new LineSpace());
            _pOption.AddChild(new HorizontalLine());
            _pOption.AddChild(new LineSpace());
            _pOption.AddChild(_fullscreen);
            _fullscreen.Checked = _ctx.Graphics.IsFullScreen;
            _pOption.AddChild(new LineSpace());
            _pOption.AddChild(new HorizontalLine());
            _pOption.AddChild(new LineSpace());
            _pOption.AddChild(new Paragraph("Volume de la musique"));
            _music = new Slider(0, 100);
            _pOption.AddChild(_music);
            _music.Value = (int)(MediaPlayer.Volume * 100);
            _pOption.AddChild(new HorizontalLine());
            _pOption.AddChild(new Paragraph("Volume des effets sonores"));
            _effect = new Slider(0, 100);
            _pOption.AddChild(_effect);
            _effect.Value = (int)(SoundEffect.MasterVolume * 100);
            _pOption.AddChild(new LineSpace());
            _pOption.AddChild(new HorizontalLine());
            _pOption.AddChild(new LineSpace());
            _return = new Button("Retour");
            _pOption.AddChild(_return);
            _pOption.Visible = false;

        }
        internal Level.State PreviousState
        {
            get { return _previousState; }
            set { _previousState = value; }
        }
        public void Update()
        {
            if (_panel != null)
            {
                if (_previousState == Level.State.Menu)
                {
                    _continue.Visible = false;
                }
                else
                {
                    if (!_continue.Visible) _continue.Visible = true;
                    _continue.OnClick = (Entity btn) =>
                    {
                        Level.ActualState = _previousState;
                    };
                }
                if (Level.ActualState != Level.State.Menu)
                {
                    _panel.Visible = false;
                    _pOption.Visible = false;
                    _continue.Visible = false;
                }
                else
                {
                    _newGame.OnClick = (Entity btn) =>
                    {
                        Level.ActualState = Level.State.Hub;
                        _ctx.Reload();
                        _panel.Visible = false;
                        _continue.Visible = false;
                    };
                    _loadGame.OnClick = (Entity btn) =>
                    {
                        Level.ActualState = Level.State.Hub;
                        _ctx.Reload();
                        Player.CurrentPlayer.PlayerInfo = PlayerInfo.LoadFrom(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Documents\\SaveDP.sav");
                        _panel.Visible = false;
                        _continue.Visible = false;
                    };
                    _options.OnClick = (Entity btn) =>
                    {
                        _panel.Visible = false;
                        _pOption.Visible = true;
                    };
                    if (_pOption.Visible)
                    {
                        _return.OnClick = (Entity btn) =>
                        {
                            _panel.Visible = true;
                            _pOption.Visible = false;
                        };
                        _fullscreen.OnValueChange = (Entity btn) =>
                        {
                            _ctx.Graphics.ToggleFullScreen();
                        };
                        SoundEffect.MasterVolume = (float)_effect.Value / 100;
                        MediaPlayer.Volume = (float)_music.Value / 100;
                    }
                    _quit.OnClick = (Entity btn) =>
                    {
                        _ctx.Exit();
                    };
                    _panel.Visible = true;
                }
            }
        }
    }
}
