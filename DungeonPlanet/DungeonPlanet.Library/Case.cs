using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonPlanet.Library
{
    public class Case
    {
        int _height;
        int _width;
        public Tile[,] Tiles { get; private set; }
        public Case(int height, int width)
        {
            _height = height;
            _width = width;
            Tiles = new Tile[width, height];
        }
    }
}
