using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace DungeonPlanet.Library
{
    public class JetPackLib : MediPackLib
    {
        public JetPackLib(Vector2 position, int width, int height, PlayerLib playerLib)
            :base(position, width, height, playerLib)
        {
        }
    }
}
