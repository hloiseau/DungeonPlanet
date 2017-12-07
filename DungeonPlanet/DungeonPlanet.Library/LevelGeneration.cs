using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonPlanet.Library
{
    public class LevelGeneration
    {
        int _columns;
        int _rows;
        PathGeneration _path;
        public LevelGeneration(int columns, int rows)
        {
            _columns = columns;
            _rows = rows;
            _path = new PathGeneration(columns, rows);
        }



    }
}
