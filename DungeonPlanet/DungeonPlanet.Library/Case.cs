using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Drawing;

namespace DungeonPlanet.Library
{
    public class Case
    {
        int _rows;
        int _columns;
        Level _ctx;
        Path.Direction _direction;
        public Tile[,] Tiles { get; private set; }
        int _xTilesOffset;
        int _yTilesOffset;
        int _xLevel;
        int _yLevel;
        string _oldMove;
        public static int _dorX;
        public static int _dorY;


        public Case(int rows, int columns, Path.Direction direction, Level ctx, int xOffset, int yOffset)
        {
            _rows = rows;
            _columns = columns;
            _ctx = ctx;
            _xTilesOffset = xOffset * 20;
            _yTilesOffset = yOffset * 14;
            _xLevel = xOffset;
            _yLevel = yOffset;
            _direction = direction;
            Tiles = new Tile[columns, rows];
        }

        public static int Clamp(int value, int min, int max)
        {
            return (value < min) ? min : (value > max) ? max : value;
        }

        public void InitializeAllTiles()
        {
            for (int x = 0; x < _columns; x++)
            {
                for (int y = 0; y < _rows; y++)
                {
                    Vector2 tilePosition = new Vector2((_xTilesOffset + x) * 64, (_yTilesOffset + y) * 64);
                    Rectangle tileBounds = new Rectangle((int)tilePosition.X, (int)tilePosition.Y, 64, 64);
                    Tiles[x, y] = new Tile(tilePosition, tileBounds, false);
                }
            }
        }

        public void SetBorder()
        {
            for (int x = 0; x < _columns; x++)
            {
                for (int y = 0; y < _rows; y++)
                {
                    if ((x == 0 && (_direction & Path.Direction.Left) == Path.Direction.None) || (x == _columns - 1 && (_direction & Path.Direction.Right) == Path.Direction.None))
                    {
                        Tiles[x, y].IsBlocked = true;
                        Tiles[x, y].Type = Tile.TypeSet.Platform;
                    }

                    if ((y == 0 && (_direction & Path.Direction.Top) == Path.Direction.None) || (y == _rows - 1 && (_direction & Path.Direction.Bottom) == Path.Direction.None))
                    {
                        Tiles[x, y].IsBlocked = true;
                        Tiles[x, y].Type = Tile.TypeSet.Platform;
                    }
                }
            }
        }

        public void StructuresCreation()
        {
            for (int x = 0; x < _columns; x += _columns / 3)
            {
                for (int y = 0; y < _rows; y += _rows / 2)
                {
                    PartsAnalysis();
                }
            }
        }

        public void PartsAnalysis()
        {
            for (int a = 0; a < _rows; a += _rows / 2) // rows = 12 and columns = 18 : so a + 6
            {
                for (int b = 1; b < _columns / 1.1; b += _columns / 3) // so b + 6
                {
                    if ((_direction & Path.Direction.Last) == Path.Direction.None) //if this is Not a "last" direction
                    {
                        GenerateStuf(a,b);
                    }
                    else
                    {
                        GenerateDor(a, b);
                    }
                }
            }
            Tiles[1, 1].IsBlocked = false;
            Tiles[1, 2].IsBlocked = false;
            Tiles[2, 1].IsBlocked = false;
            Tiles[2, 2].IsBlocked = false;
        }

        private void GenerateDor(int a, int b)
        {
            if (a == 7 && b == 7)
            {
                if (_xLevel != 0)
                {
                    _dorX = (((_xLevel + 1) * (64 * 20)) - (64 * 11));
                }
                else
                {
                    _dorX = _xLevel + (64 * 10);
                }

                _dorY = (((_yLevel + 1) * (64 * 14)) - (64 * 4));

            }
            else
            {
                GenerateStuf(a, b);
            }
            
        }

        private void GenerateStuf(int a, int b)
        {
            if (!(a >= _columns / 3 && a <= 2 * (_columns / 3))) //so diff 6 >= x <= 12 (far left or far right case)
            {
                if (_ctx.GetNext(0, 2) == 0) // if random = 0
                {
                    GeneratePlatform(a, b);
                    _oldMove = "plat";
                }
                else
                {
                    if (((_direction & Path.Direction.Left) == Path.Direction.None && a < _columns / 3) || ((_direction & Path.Direction.Right) == Path.Direction.None && a >= 2 * (_columns / 3))) // if  this is not left direction and x < 6 ||  if  this is not right direction and x >= 12
                    {
                        GenerateStairs(a, b);
                        _oldMove = "stairs";
                    }
                    else
                    {
                        GenerateBlock(a, b);
                        _oldMove = "bloc";
                    }
                }
            }
            else // so  x < 6 || 12 < x (midle case)
            {
                if (_ctx.GetNext(0, 2) == 0) //same as the 1rst one random  = 0
                {
                    GeneratePlatform(a, b);
                    _oldMove = "plat";
                }
                else
                {
                    if ((_oldMove != "piramid" && (_direction & Path.Direction.Bottom) == Path.Direction.None && b < _rows / 2) || (_oldMove != "piramid" && (_direction & Path.Direction.Top) == Path.Direction.None && b >= _rows / 2)) // if  this is not botom direction and y < 6 ||  if  this is not top direction and y >= 6
                    {
                        GeneratePyramid(a, b);
                        _oldMove = "piramid";
                    }
                    else
                    {
                        GenerateBlock(a, b);
                        _oldMove = "bloc";
                    }
                }
            }
        }

        private void GenerateStairs(int x, int y)
        {
            int length = _ctx.GetNext(2, 3);
            int height = _ctx.GetNext(5, 7);
            int offset = 0;
            for (int b = -height / 2; b < height / 2; b++)
            {
                offset++;
                for (int a = -length / 2; a < length / 2; a++)
                {
                    Tiles[Clamp(x + a + offset, 0, _columns - 1), Clamp(y + b, 0, _rows - 1)].IsBlocked = true;
                    Tiles[Clamp(x + a + offset, 0, _columns - 1), Clamp(y + b, 0, _rows - 1)].Type = Tile.TypeSet.Platform;
                }
            }
        }

        private void GenerateBlock(int x, int y)
        {
            int maxlength = _ctx.GetNext(3, 7);
            int length = _ctx.GetNext(2, 5);
            int height = _ctx.GetNext(2, 5);
            for (int b = -height / 2; b < height / 2; b++)
            {
                if (b < height)
                {
                    length += 2;
                    length = Clamp(length, 2, maxlength);

                    for (int a = -length / 2; a < length / 2; a++)
                    {
                        Tiles[Clamp(x + a, 0, _columns - 1), Clamp(y + b, 0, _rows - 1)].IsBlocked = true;
                        Tiles[Clamp(x + a, 0, _columns - 1), Clamp(y + b, 0, _rows - 1)].Type = Tile.TypeSet.Platform;
                    }
                }
                else
                {
                    length -= 2;
                    length = Clamp(length, 2, maxlength);

                    for (int a = -length / 2; a < length / 2; a--)
                    {
                        Tiles[Clamp(x + a, 0, _columns - 1), Clamp(y + b, 0, _rows - 1)].IsBlocked = true;
                        Tiles[Clamp(x + a, 0, _columns - 1), Clamp(y + b, 0, _rows - 1)].Type = Tile.TypeSet.Platform;
                    }
                }
            }
        }

        private void GeneratePyramid(int x, int y)
        {
            if ((_direction & Path.Direction.Bottom) == Path.Direction.None)
            {
                if (x < _columns || y < _rows)
                {
                    Tiles[y + 2, x + 3].IsBlocked = true;
                    Tiles[y + 3, x + 3].IsBlocked = true;

                    Tiles[y + 1, x + 4].IsBlocked = true;
                    Tiles[y + 2, x + 4].IsBlocked = true;
                    Tiles[y + 3, x + 4].IsBlocked = true;
                    Tiles[y + 4, x + 4].IsBlocked = true;

                    Tiles[y + 0, x + 5].IsBlocked = true;
                    Tiles[y + 1, x + 5].IsBlocked = true;
                    Tiles[y + 2, x + 5].IsBlocked = true;
                    Tiles[y + 3, x + 5].IsBlocked = true;
                    Tiles[y + 4, x + 5].IsBlocked = true;
                    Tiles[y + 5, x + 5].IsBlocked = true;


                    Tiles[y + 2, x + 3].Type = Tile.TypeSet.Platform;
                    Tiles[y + 3, x + 3].Type = Tile.TypeSet.Platform;

                    Tiles[y + 1, x + 4].Type = Tile.TypeSet.Platform;
                    Tiles[y + 2, x + 4].Type = Tile.TypeSet.Platform;
                    Tiles[y + 3, x + 4].Type = Tile.TypeSet.Platform;
                    Tiles[y + 4, x + 4].Type = Tile.TypeSet.Platform;

                    Tiles[y + 0, x + 5].Type = Tile.TypeSet.Platform;
                    Tiles[y + 1, x + 5].Type = Tile.TypeSet.Platform;
                    Tiles[y + 2, x + 5].Type = Tile.TypeSet.Platform;
                    Tiles[y + 3, x + 5].Type = Tile.TypeSet.Platform;
                    Tiles[y + 4, x + 5].Type = Tile.TypeSet.Platform;
                    Tiles[y + 5, x + 5].Type = Tile.TypeSet.Platform;
                }
            }
        }

        private void GeneratePlatform(int x, int y)
        {
            int length = _ctx.GetNext(3, 5);
            int flor = _ctx.GetNext(2, 4);
            int leftOrRight = _ctx.GetNext(0, 1);
            if (y < 12)
            {
                for (int i = 0; i < length; i++)
                {
                    Tiles[x + i, y + flor].IsBlocked = true;
                    Tiles[x + i, y + flor].Type = Tile.TypeSet.Platform;
                    if (length <= 3)
                    {
                        if (leftOrRight == 0)
                        {
                            Tiles[x + length + i, y + flor - length].IsBlocked = true;
                            Tiles[x + length + i, y + flor - length].Type = Tile.TypeSet.Platform;
                        }
                        else
                        {
                            Tiles[x - length + i, y + flor - length].IsBlocked = true;
                            Tiles[x - length + i, y + flor - length].Type = Tile.TypeSet.Platform;
                        }
                    }
                    else
                    {
                        if (leftOrRight == 0)
                        {
                            Tiles[x + 3 + i, y + flor - 3].IsBlocked = true;
                            Tiles[x + 3 + i, y + flor - 3].Type = Tile.TypeSet.Platform;
                        }
                        else
                        {
                            Tiles[x - 3 + i, y + flor - 3].IsBlocked = true;
                            Tiles[x - 3 + i, y + flor - 3].Type = Tile.TypeSet.Platform;
                        }
                    }
                }
            }
        }
    }
}
