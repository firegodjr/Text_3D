using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_3D_Renderer
{
    /// <summary>
    /// Represents screen coordinates
    /// </summary>
    public struct Coords
    {
        public int X, Y;
        public Coords(int x = 0, int y = 0)
        {
            this.X = x;
            this.Y = y;
        }
    }
}
