using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_3D_Renderer.Rendering
{
    public class Vector3 : ICloneable
    {
        private double x, y, z;

        private bool w;

        public Vector3()
        {
            X = 0;
            Y = 0;
            Z = 0;
            W = 1;
        }

        public Vector3(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.W = 1;
        }

        public Vector3(bool isPoint) : this()
        {
            w = isPoint;
        }

        public Vector3(double x, double y, double z, bool w) : this(x, y, z)
        {
            this.w = w;
        }

        public Vector3(double x, double y, double z, int w) : this(x, y, z, w == 1 ? true : false)
        {

        }

        public double X
        {
            get
            {
                return x;
            }

            set
            {
                x = value;
            }
        }

        public double Y
        {
            get
            {
                return y;
            }

            set
            {
                y = value;
            }
        }

        public double Z
        {
            get
            {
                return z;
            }

            set
            {
                z = value;
            }
        }

        public double Mag
        {
            get
            {
                return Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2) + Math.Pow(z, 2));
            }
            set
            {
                Normalize();
                X *= value;
                Y *= value;
                Z *= value;
            }
        }

        public int W
        {
            get { return w ? 1 : 0; }
            set { w = (value == 1 ? true : false); }
        }

        public object Clone()
        {
            return new Vector3(X, Y, Z, W);
        }

        public Vector3 inverse()
        {
            return new Vector3(-X, -Y, -Z, W);
        }

        public double Dot(Vector3 vec)
        {
            return X * vec.X + Y * vec.Y + Z * vec.Z;
        }

        public Vector3 Normalize()
        {
            X /= Mag;
            Y /= Mag;
            Z /= Mag;

            return this;
        }

        public static  Vector3 operator +(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }

        public static Vector3 operator -(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        }

        public static Vector3 operator *(Vector3 vec, double length)
        {
            Vector3 result = (Vector3)vec.Clone();
            result.Mag *= length;
            return vec;
        }
    }
}
