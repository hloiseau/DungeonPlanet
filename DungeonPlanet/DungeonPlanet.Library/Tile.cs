using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Drawing;

namespace DungeonPlanet.Library
{
    public class Tile
    {
        public Vector2 Position { get; set; }
        public Rectangle Bounds { get; set; }
        public bool IsBlocked { get; set; }

        public Tile(Vector2 position, Rectangle bounds, bool isBlocked)
        {
            Position = position;
            Bounds = bounds;
            IsBlocked = isBlocked;
        }
    }
}
