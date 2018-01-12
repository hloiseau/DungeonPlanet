using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Drawing;

namespace DungeonPlanet.Library
{

    public class Path
    {
        public int Columns { get; set; }
        public int Rows { get; set; }
        private Random _rnd = new Random();
        public Direction[,] Board { get; private set; }
        Level _ctx;

        [Flags]
        public enum Direction
        {
            None = 0,
            Left = 1,
            Right = 2,
            Top = 4,
            Bottom = 8,
            Last = 16
        }

        public Path(int columns, int rows, Level ctx)
        {
            Columns = columns;
            Rows = rows;
            Board = new Direction[columns, rows];
            _ctx = ctx;
        }

        public void InitializeBoard()
        {
            for (int x = 0; x < Columns; x++)
            {
                for (int y = 0; y < Rows; y++)
                {
                    Board[x, y] = 0;
                }
            }
        }

        public void CreatePath()
        {
            int x = 0;
            int y = 0;
            
            int whereToGo;
            bool isOk = true;
            while (isOk)
            {
                whereToGo = _ctx.GetNext(1, 12);
                //whereToGo = _rnd.Next(1, 6);
                if (whereToGo == 1)
                {
                    isOk = Down(x, y);
                    y++;
                }
                else if (whereToGo >= 2 && whereToGo <= 8) // direction right
                {
                    x++;
                    if (x < Columns)
                    {
                        if ((Board[x, y] & Direction.Left) == Direction.None)
                        {
                            Board[x - 1, y] |= Direction.Right;
                            Board[x, y] |= Direction.Left;
                        }
                    }
                    else
                    {
                        x--;
                        isOk = Down(x, y);

                        y++;
                    }
                }
                else if (whereToGo >= 9 && whereToGo <= 11) // direction left
                {
                    x--;
                    if (x >= 0)
                    {
                        if ((Board[x, y] & Direction.Right) == Direction.None)
                        {
                            Board[x + 1, y ] |= Direction.Left;
                            Board[x, y] |= Direction.Right;
                        }
                    }
                    else
                    {
                        x++;
                        isOk = Down(x, y);
                        y++;
                    }
                }
            }
        }

        private bool Down(int x, int y)
        {
            y++;
            if (y < Rows)
            {
                Board[x, y-1] |= Direction.Bottom;
                Board[x, y] |= Direction.Top;
                return true;
            }
            else
            {
                Board[x, y-1] |= Direction.Last;
                return false;
            }
        }
    }
}
