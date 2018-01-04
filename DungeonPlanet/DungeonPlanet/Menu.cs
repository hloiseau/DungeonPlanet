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

        DungeonPlanetGame _ctx;
        public Menu(DungeonPlanetGame ctx)
        {
            _ctx = ctx;
            _panel = new Panel(new Vector2(1280, 720));
            UserInterface.Active.AddEntity(_panel);
            _panel.AddChild(new Header("Dungeon Planet"));
            _newGame = new Button("Nouveau Jeu", ButtonSkin.Default, Anchor.AutoCenter, new Vector2(1000, 150));
            _panel.AddChild(_newGame);
            _loadGame = new Button("Reprendre Jeu", ButtonSkin.Default, Anchor.AutoCenter, new Vector2(1000, 150));
            _panel.AddChild(_loadGame);
            _options = new Button("Option", ButtonSkin.Default, Anchor.AutoCenter, new Vector2(1000, 150));
            _panel.AddChild(_options);
            _quit = new Button("Quitter", ButtonSkin.Default, Anchor.AutoCenter, new Vector2(1000, 150));
            _panel.AddChild(_quit);

        }

        public void Update()
        {
            if (_newGame.IsMouseDown)
            {
                Level.ActualState = Level.State.Hub;
                Level.CurrentBoard.NewLevel();
                if (_panel != null) UserInterface.Active.RemoveEntity(_panel);
                _panel = null;
            }

            if (_quit.IsMouseDown)
            {
                _ctx.Exit();
            }
        }
    }
}
