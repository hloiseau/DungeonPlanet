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
        public Sprite _sprite;
        public Level Level { get; private set; }

        public Board(SpriteBatch spritebatch, Texture2D tileTexture, int columns, int rows)
        {

            TileTexture = tileTexture;
            SpriteBatch = spritebatch;
            Level = new Level(2, 2);
            CreateNewBoard();
            Board.CurrentBoard = this;
        }

        public void CreateNewBoard()
        {
            Level.NewLevel();
        }

        public void Draw()
        {
            foreach(Case Case in Level.Cases)
            {
                foreach (var tile in Case.Tiles)
                {
                    if (tile.IsBlocked)
                    {
                        Sprite sprite = new Sprite(TileTexture, new Vector2(tile.Position.X, tile.Position.Y), SpriteBatch);
                        sprite.Draw();
                    }
                }
            }
        }
    }
}
