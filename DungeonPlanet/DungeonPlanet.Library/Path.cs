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

        [Flags]
        public enum Direction
        {
            None = 0,
            Left = 1,
            Right = 2,
            Top = 4,
            Bottom = 8
        }

        public Path(int columns, int rows)
        {
            Columns = columns;
            Rows = rows;
            Board = new Direction[columns, rows];
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
                whereToGo = _rnd.Next(1, 6);
                if (whereToGo == 1)
                {
                    isOk = Down(x, y);
                    x++;
                }
                else if (whereToGo == 2 || whereToGo == 3)
                {
                    y++;
                    if (y < Rows)
                    {
                        if ((Board[x, y] & Direction.Left) == Direction.None)
                        {
                            Board[x, y - 1] |= Direction.Right;
                            Board[x, y] |= Direction.Left;
                        }
                    }
                    else
                    {
                        y--;
                        isOk = Down(x, y);

                        x++;
                    }
                }
                else if (whereToGo == 4 || whereToGo == 5)
                {
                    y--;
                    if (y >= 0)
                    {
                        if ((Board[x, y] & Direction.Right) == Direction.None)
                        {
                            Board[x, y + 1] |= Direction.Left;
                            Board[x, y] |= Direction.Right;
                        }
                    }
                    else
                    {
                        y++;
                        isOk = Down(x, y);
                        x++;
                    }
                }
            }
        }

        private bool Down(int x, int y)
        {
            Board[x, y] |= Direction.Bottom;
            x++;
            if (x < Rows)
            {
                Board[x, y] |= Direction.Top;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
