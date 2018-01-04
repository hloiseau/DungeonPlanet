using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Numerics;

namespace DungeonPlanet.Library
{
    public class MediPackLib
    {
        Vector2 _position;
        int _width;
        int _height;
        PlayerLib _playerLib;
        public MediPackLib(Vector2 position, int width, int height, PlayerLib playerLib)
        {
            _position = position;
            _width = width;
            _height = height;
            _playerLib = playerLib;
        }

        public Rectangle Bounds
        {
            get { return new Rectangle((int)_position.X, (int)_position.Y, _width, _height); }
        }

        public bool PlayerIntersect()
        {
            return Bounds.IntersectsWith(_playerLib.Bounds);            
        }
    }
}
