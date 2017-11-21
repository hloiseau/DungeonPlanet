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
        private Random _rnd = new Random();
        public static Board CurrentBoard { get; private set; }
        public BoardLib _boardLib;
        public Sprite _sprite;
      

        public Board(SpriteBatch spritebatch, Texture2D tileTexture, int columns, int rows)
        {

            TileTexture = tileTexture;
            SpriteBatch = spritebatch;

            _boardLib = new BoardLib(columns, rows, tileTexture.Width, tileTexture.Height);
            CreateNewBoard();
            Board.CurrentBoard = this;
        }

        public void CreateNewBoard()
        {
            _boardLib.InitializeAllTilesAndBlockSomeRandomly();
            _boardLib.SetAllBorderTilesBlocked();
            _boardLib.SetTopLeftTileUnblocked();
        }

        public void Draw()
        {
            foreach (var tile in _boardLib.Tiles)
            {
                if(tile.IsBlocked)
                {
                    Sprite sprite = new Sprite(TileTexture, new Vector2(tile.Position.X, tile.Position.Y), SpriteBatch);
                    sprite.Draw();
                }
            }
        }
    }
}
