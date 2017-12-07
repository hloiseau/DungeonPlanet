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
        private Random _rnd = new Random();
        Tile[,] _marks;
        public Tile[,] Tiles { get; private set; }
        int _xTilesOffset;
        int _yTilesOffset;


        public Case(int rows, int columns, Path.Direction direction, Level ctx, int xOffset, int yOffset)
        {
            _rows = rows;
            _columns = columns;
            _ctx = ctx;
            _xTilesOffset = xOffset * 20;
            _yTilesOffset = yOffset * 14;
            _direction = direction;
            Tiles = new Tile[columns, rows];
            _marks = new Tile[columns, rows];
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
                    }

                    if ((y == 0 && (_direction & Path.Direction.Top) == Path.Direction.None) || (y == _rows - 1 && (_direction & Path.Direction.Bottom) == Path.Direction.None))
                    {
                        Tiles[x, y].IsBlocked = true;
                    }
                }
            }
        }

        public void RandomTiles()
        {
            int col;
            int row;
            for (int x = 0; x < 5; x++)
            {
                col = _rnd.Next(_columns - 1);
                row = _rnd.Next(_rows - 1);
                Tiles[col, row].IsBlocked = true;
                _marks[col, row] = Tiles[col, row];
            }
        }

        public void StructuresCreation()
        {
            for (int x = 0; x < _columns; x++)
            {
                for (int y = 0; y < _rows; y++)
                {
                    if (_marks[x, y] != null)
                    {
                        PartsAnalysis(x, y);
                    }
                }
            }
        }

        private void PartsAnalysis(int x, int y)
        {
            for (int a = 0; a < _rows; a += _rows / 2)
            {
                for (int b = 0; b < _columns; b += _columns / 3)
                {
                    if (!(x >= _columns / 3 && x <= 2 * (_columns / 3)))
                    {
                        if (_rnd.Next(2) == 0)
                        {
                            GeneratePlatform(x, y);
                        }
                        else
                        {
                            if (((_direction & Path.Direction.Left) == Path.Direction.None && x < _columns / 3) || ((_direction & Path.Direction.Right) == Path.Direction.None && x >= 2 * (_columns / 3)))
                            {
                                GenerateStairs(x, y);
                            }
                            else
                            {
                                GenerateBlock(x, y);
                            }
                        }
                    }
                    else
                    {
                        if (_rnd.Next(2) == 0)
                        {
                            GeneratePlatform(x, y);
                        }
                        else
                        {
                            if (((_direction & Path.Direction.Bottom) == Path.Direction.None && y < _rows / 2) || ((_direction & Path.Direction.Top) == Path.Direction.None && y >= _rows / 2))
                            {
                                GeneratePyramid(x, y);
                            }
                        }
                    }
                }
            }
        }

        private void GenerateStairs(int x, int y)
        {
            int length = _rnd.Next(2, 4);
            int height = _rnd.Next(3, 7);
            int offset = 0;
            for (int b = -height / 2; b < height / 2; b++)
            {
                offset++;
                for (int a = -length / 2; a < length / 2; a++)
                {
                    Tiles[Clamp(x + a + offset, 0, _columns - 1), Clamp(y + b, 0, _rows - 1)].IsBlocked = true;
                }
            }
        }

        private void GenerateBlock(int x, int y)
        {
            int maxlength = _rnd.Next(3, 7);
            int length = _rnd.Next(0, 3);
            int height = _rnd.Next(3, 7);
            for (int b = -height / 2; b < height / 2; b++)
            {
                if (b < height)
                {
                    length += 2;
                    length = Clamp(length, 2, maxlength);

                    for (int a = -length / 2; a < length / 2; a++)
                    {
                        Tiles[Clamp(x + a, 0, _columns - 1), Clamp(y + b, 0, _rows - 1)].IsBlocked = true;
                    }
                }
                else
                {
                    length -= 2;
                    length = Clamp(length, 2, maxlength);

                    for (int a = -length / 2; a < length / 2; a--)
                    {
                        Tiles[Clamp(x + a, 0, _columns - 1), Clamp(y + b, 0, _rows - 1)].IsBlocked = true;
                    }
                }
            }
        }

        private void GeneratePyramid(int x, int y)
        {
            int length = 1;
            if ((_direction & Path.Direction.Bottom) == Path.Direction.None)
            {
                for (int b = 1; b+y < _rows - 1 / 2; b++)
                {
                    length += 2;
                    for (int a = -length / 2; a < length / 2; a++)
                    {
                        Tiles[Clamp(x + a, 0, _columns - 1), Clamp(y + b, 0, _rows - 1)].IsBlocked = true;
                    }
                }
            }
            else
            {
                for (int b = 0; b+y < 0 / 2; b--)
                {
                    length += 2;
                    for (int a = -length / 2; a < length / 2; a++)
                    {
                        Tiles[Clamp(x + a, 0, _columns - 1), Clamp(y + b, 0, _rows - 1)].IsBlocked = true;
                    }
                }
            }
        }

        private void GeneratePlatform(int x, int y)
        {
            int length = _rnd.Next(3, 7);
            for (int a = -length / 2; a < length / 2; a++)
            {
                Tiles[Clamp(x + a, 0, _columns - 1), y].IsBlocked = true;
            }
        }
    }
}
