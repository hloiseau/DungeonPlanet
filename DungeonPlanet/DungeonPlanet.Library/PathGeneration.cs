using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Drawing;

namespace DungeonPlanet.Library
{

    public class PathGeneration
    {
        public int Columns { get; set; }
        public int Rows { get; set; }
        private Random _rnd = new Random();
        public int[,] Board { get; private set; }

        public PathGeneration(int columns, int rows)
        {
            Columns = columns;
            Rows = rows;
            Board = new int[columns, rows];
        }
        
        public void InitializeBoard()
        {
            for(int x = 0; x < Columns; x++)
            {
                for(int y = 0; y < Rows; y++)
                {
                    Board[x, y] = 0;
                }
            }
        }
           
        public void CreatePath()
        {
            int x = 0;
            int y = _rnd.Next(Columns);
            int whereToGo;
            bool isOk = true;
            Board[x,y] = 1;
            while (isOk)
            {
                whereToGo = _rnd.Next(5);

                if(whereToGo == 0)
                {
                    isOk = Down(x, y);
                    x++;
                    
                }
                else if (whereToGo == 1 || whereToGo == 2)
                {
                    y++;
                    if (y < Rows)
                    {
                        Board[x, y] = 1;
                    }
                    else
                    {
                        y--;
                        isOk = Down(x, y);
                        x++;
                    }
                } 
                else if (whereToGo == 3 || whereToGo == 4)
                {
                    y--;
                    if (y >= 0)
                    {
                        Board[x, y] = 1;
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
            Board[x, y] = 2;
            x++;
            if (x < Rows)
            {
                Board[x, y] = 3;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
