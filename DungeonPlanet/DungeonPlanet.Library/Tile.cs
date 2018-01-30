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
        public TypeSet Type { get; set;}
        public enum TypeSet
        {
            Invisible = 0,
            External = 1,
            Background = 2,
            Wall = 4,
            Platform = 8,
        }

        public Tile(Vector2 position, Rectangle bounds, bool isBlocked, TypeSet type)
        {
            Position = position;
            Bounds = bounds;
            IsBlocked = isBlocked;
            Type = type;
        }
        public Tile(Vector2 position, Rectangle bounds, bool isBlocked)
        {
            Position = position;
            Bounds = bounds;
            IsBlocked = isBlocked;
            Type = TypeSet.Invisible;
        }
    }
}
