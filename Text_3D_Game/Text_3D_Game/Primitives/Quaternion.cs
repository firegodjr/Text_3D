using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Text_3D_Renderer.Rendering;

namespace Text_3D_Renderer.Primitives
{
    /// <summary>
    /// Represents a rotation in 3d space.
    /// </summary>
    class Quaternion
    {
        private double qx, qy, qz, qw;

        public Quaternion(double radians, double x, double y, double z)
        {
            qx = x * Math.Sin(radians / 2);
            qy = y * Math.Sin(radians / 2);
            qz = z * Math.Sin(radians / 2);
            qw = Math.Cos(radians / 2);
        }

        public Quaternion() : this(0, 0, 0, 0) { }

        public void setRotation(double radians, double x, double y, double z)
        {
            qx = x * Math.Sin(radians / 2);
            qy = y * Math.Sin(radians / 2);
            qz = z * Math.Sin(radians / 2);
            qw = Math.Cos(radians / 2);
        }

        public void normalize()
        {
            double n = 1.0 / Math.Sqrt(qx * qx + qy * qy + qz * qz + qw * qw);
            qx *= n;
            qy *= n;
            qz *= n;
            qw *= n;
        }

        public Matrix toMatrix()
        {
            normalize();
            return Matrix.Multiply(
                new Matrix(
                    new double[] { qw, -qz, qy, -qx }, 
                    new double[] { qz, qw, -qx, -qy }, 
                    new double[] { -qy, qx, qw, -qz },
                    new double[] { qx, qy, qz, qw }
                    ), 
                new Matrix(
                    new double[] { qw, -qz, qy, qx }, 
                    new double[] { qz, qw, -qx, qy }, 
                    new double[] { -qy, qx, qw, qz }, 
                    new double[] { -qx, -qy, -qz, qw }));
        }

        public double W
        {
            get
            {
                return qw;
            }

            set
            {
                qw = value;
            }
        }

        public double X
        {
            get
            {
                return qx;
            }

            set
            {
                qx = value;
            }
        }

        public double Y
        {
            get
            {
                return qy;
            }

            set
            {
                qy = value;
            }
        }

        public double Z
        {
            get
            {
                return qz;
            }

            set
            {
                qz = value;
            }
        }
    }
}
