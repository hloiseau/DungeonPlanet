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
        public TileLib[,] Tiles { get; set; }
        public int Columns { get; set; }
        public int Rows { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        private Random _rnd = new Random();

        public PathGeneration(int columns, int rows)
        {
            Columns = columns;
            Rows = rows;

        }







    }
}
