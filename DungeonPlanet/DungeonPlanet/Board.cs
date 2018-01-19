using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DungeonPlanet.Library;

namespace DungeonPlanet
{
    public class Board
    {

        public Texture2D TileTexture { get; set; }
        private SpriteBatch SpriteBatch { get; set; }
        public static Board CurrentBoard { get; private set; }
        public Sprite _sprite;
        public Level Level { get; private set; }
        public static List<Enemy> Enemys { get; private set; }
        public static List<Boss> Bosses { get; private set; }
        List<TilesLook> _tilesLookList;

        public Board(SpriteBatch spritebatch, Texture2D tileTexture, int columns, int rows)
        {
            TileTexture = tileTexture;
            SpriteBatch = spritebatch;
            Level = new Level();
            _tilesLookList = new List<TilesLook>();
            CreateNewBoard();
            Board.CurrentBoard = this;
        }

        public void CreateNewBoard()
        {
            Level.NewLevel();
            ListInisilisation();
        }
        public void ListInisilisation()
        {
            if (Level.ActualState == Level.State.Hub || Level.ActualState == Level.State.BossRoom)
            {
                foreach (var tile in Level.Hub.Tiles)
                {
                    if (tile.IsBlocked)
                    {
                        TilesLook tileLook = new TilesLook(TileTexture, tile.Position.X, tile.Position.Y, tile.Type, Color.White);
                        _tilesLookList.Add(tileLook);
                    }
                }
            }
            if (Level.ActualState == Level.State.Level)
            {
                foreach (Case Case in Level.Cases)
                {
                    foreach (var tile in Case.Tiles)
                    {
                        if (tile.IsBlocked)
                        {
                            TilesLook tileLook = new TilesLook(TileTexture, tile.Position.X, tile.Position.Y, tile.Type, Color.White);
                            _tilesLookList.Add(tileLook);
                        }
                    }
                }
            }
        }
        public void Draw()
        {
            if (Level.ActualState == Level.State.Hub 
                || Level.ActualState == Level.State.BossRoom
                || Level.ActualState == Level.State.Level )
            {
                foreach (var t in _tilesLookList)
                {
                    t.Draw(SpriteBatch);
                }
            }
        }
    }
}
