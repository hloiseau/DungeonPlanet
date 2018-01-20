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
    public class Menu
    {
        Panel _panel;
        Button _newGame;
        Button _loadGame;
        Button _options;
        Button _quit;
        Button _continue;
        DungeonPlanetGame _ctx;
        Level.State _previousState;
        public Menu(DungeonPlanetGame ctx)
        {

            _ctx = ctx;
            _panel = new Panel(new Vector2(1300, 750));
            UserInterface.Active.AddEntity(_panel);
            _panel.AddChild(new Header("Dungeon Planet"));
            _continue = new Button("Reprendre", ButtonSkin.Default, Anchor.AutoCenter, new Vector2(1000, 125));
            _panel.AddChild(_continue);
            _newGame = new Button("Nouvelle Partie", ButtonSkin.Default, Anchor.AutoCenter, new Vector2(1000, 125));
            _panel.AddChild(_newGame);
            _loadGame = new Button("Charger Partie", ButtonSkin.Default, Anchor.AutoCenter, new Vector2(1000, 125));
            _panel.AddChild(_loadGame);
            _options = new Button("Option", ButtonSkin.Default, Anchor.AutoCenter, new Vector2(1000, 125));
            _panel.AddChild(_options);
            _quit = new Button("Quitter", ButtonSkin.Default, Anchor.AutoCenter, new Vector2(1000, 125));
            _panel.AddChild(_quit);

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
