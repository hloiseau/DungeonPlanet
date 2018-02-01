using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Drawing;

namespace DungeonPlanet.Library
{
    public class Hub
    {
        int _rows;
        int _columns;
        private Random _rnd = new Random();
        public Tile[,] Tiles { get; private set; }

        public Hub(int rows, int columns)
        {
            _rows = rows;
            _columns = columns;
            Tiles = new Tile[columns, rows];
        }

        public void SetTopLeftTileUnblocked()
        {
            Tiles[1, 1].IsBlocked = false;
            Tiles[1, 2].IsBlocked = false;
        }

        public void InitializeAllTilesAndBlockSomeRandomly()
        {
            for (int x = 0; x < _columns; x++)
            {
                for (int y = 0; y < _rows; y++)
                {
                    Vector2 tilePosition = new Vector2(x * 64, y * 64);
                    Rectangle tileBounds = new Rectangle((int)tilePosition.X, (int)tilePosition.Y, 64, 64);
                    Tile tile = new Tile(tilePosition, tileBounds, /*_rnd.Next(5) == 0*/ false, Tile.TypeSet.Platform);
                    Tiles[x, y] = tile;
                }
            }
        }

        public void SetAllBorderTilesBlocked()
        {
            if (Level.ActualState == Level.State.Hub)
            {
                for (int x = 0; x < _columns; x++)
                {
                    for (int y = 0; y < _rows; y++)
                    {
                        if (x == 0 || x == _columns - 1 || y == 0 || y == _rows - 1)
                        {
                            Tiles[x, y].IsBlocked = true;
                            Tiles[x, y].Type = Tile.TypeSet.Invisible;
                        }
                    }

                    Tiles[x, 9].Type = Tile.TypeSet.Wall;
                }
            }
            else
            {
                for (int x = 0; x < _columns; x++)
                {
                    for (int y = 0; y < _rows; y++)
                    {
                        if (x == 0 || x == _columns - 1 || y == 0 || y == _rows - 1)
                        {
                            Tiles[x, y].IsBlocked = true;
                            Tiles[x, y].Type = Tile.TypeSet.Platform;
                        }
                    }
                }
            }
        }
    }
}
