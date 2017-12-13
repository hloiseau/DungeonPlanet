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

        public Board(SpriteBatch spritebatch, Texture2D tileTexture, int columns, int rows)
        {

            TileTexture = tileTexture;
            SpriteBatch = spritebatch;
            Level = new Level(5, 5);
            CreateNewBoard();
            Board.CurrentBoard = this;
        }

        public void CreateNewBoard()
        {
            Level.NewLevel();
        }

        public void Draw()
        {
            if (Level.ActualState == Level.State.Hub)
            {
                foreach (var tile in Level.Hub.Tiles)
                {
                    if (tile.IsBlocked)
                    {
                        Sprite sprite = new Sprite(TileTexture, new Vector2(tile.Position.X, tile.Position.Y), SpriteBatch);
                        sprite.Draw();
                    }
                }
            }
            else if (Level.ActualState == Level.State.LevelOne)
            {
                foreach (Case Case in Level.Cases)
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
}
