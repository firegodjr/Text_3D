using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_3D_Renderer.Rendering.Primitives
{
    class Plane
    {
        double A, B, C, D;

        public Plane(double A, double B, double C)
        {
            this.A = A;
            this.B = B;
            this.C = C;
            this.D = -(A + B + C);
        }
    }
}
