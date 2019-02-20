using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_3D_Renderer.World
{
    public class Plane
    {
        double A, B, C, D;

        Vector3 normal
        {
            get
            {
                return new Vector3(A, B, C);
            }
            set
            {
                A = value.X;
                B = value.Y;
                C = value.Z;
            }
        }

        public Plane(Vector3 p0, Vector3 p1, Vector3 p2)
        {
            Vector3 v = p1 - p0;
            Vector3 u = p2 - p0;
            Vector3 n = v / u;

            n.Normalize();
            A = n.X;
            B = n.Y;
            C = n.Z;
            D = -n * p0;
        }

        public Plane(Vector3 normal, Vector3 p)
        {
            A = normal.X;
            B = normal.Y;
            C = normal.Z;
            D = -normal * p;
        }

        public double SignedDist(Vector3 p)
        {
            return normal * p + D;
        }
    }
}
