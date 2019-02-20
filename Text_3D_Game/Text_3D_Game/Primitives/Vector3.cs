using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_3D_Renderer
{
    /// <summary>
    /// A 3-dimensional vector
    /// </summary>
    public class Vector3 : ICloneable
    {
        private double x, y, z;

        private int w;

        /// <summary>
        /// Creates a new 3d vector at 0,0,0
        /// </summary>
        public Vector3()
        {
            X = 0;
            Y = 0;
            Z = 0;
            W = 1;
        }

        /// <summary>
        /// Creates a new 3d vector with given values
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public Vector3(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.W = 1;
        }

        /// <summary>
        /// Creates a new 3d vector with a specific W value
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="w"></param>
        public Vector3(double x, double y, double z, int w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        /// <summary>
        /// Vector X position
        /// </summary>
        public double X
        {
            get { return x; }
            set { x = value; }
        }

        /// <summary>
        /// Vector Y position
        /// </summary>
        public double Y
        {
            get { return y; }
            set { y = value; }
        }

        /// <summary>
        /// Vector Z position
        /// </summary>
        public double Z
        {
            get { return z; }
            set { z = value; }
        }

        /// <summary>
        /// Vector magnitude
        /// </summary>
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

        /// <summary>
        /// Vector type identifier (mostly unused)
        /// </summary>
        public int W
        {
            get { return w; }
            set { w = value; }
        }

        public double XYZ
        {
            set { X = value; Y = value; Z = value; }
        }

        /// <summary>
        /// Clones the vector
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return new Vector3(X, Y, Z, W);
        }

        /// <summary>
        /// Gets the vector reflected over all axes
        /// </summary>
        /// <returns></returns>
        public Vector3 Inverse()
        {
            return new Vector3(-X, -Y, -Z, W);
        }

        /// <summary>
        /// Gets the dot product of the vector
        /// </summary>
        /// <param name="vec"></param>
        /// <returns></returns>
        public double Dot(Vector3 vec)
        {
            return X * vec.X + Y * vec.Y + Z * vec.Z;
        }

        /// <summary>
        /// Sets the magnitude to 1
        /// </summary>
        /// <returns></returns>
        public Vector3 Normalize()
        {
            double mag = Mag;
            if (mag != 0)
            {
                X /= mag;
                Y /= mag;
                Z /= mag;
            }
            else X = Y = Z = 0;

            return this;
        }

        /// <summary>
        /// Gets the sum of two vectors
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static Vector3 operator +(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }

        /// <summary>
        /// Gets the difference of two vectors
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static Vector3 operator -(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        }

        /// <summary>
        /// Multiplies the magnitude of the vector by the given double
        /// </summary>
        /// <param name="vec"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static Vector3 operator *(Vector3 vec, double length)
        {
            Vector3 result = (Vector3)vec.Clone();
            result.Mag *= length;
            return result;
        }

        /// <summary>
        /// Gets the dot product of the vectors
        /// </summary>
        /// <param name="vec1"></param>
        /// <param name="vec2"></param>
        /// <returns></returns>
        public static double operator *(Vector3 vec1, Vector3 vec2)
        {
            return vec1.X * vec2.X + vec1.Y * vec2.Y + vec1.Z * vec2.Z;
        }

        /// <summary>
        /// Gets the cross product of the vectors
        /// </summary>
        /// <param name="vec1"></param>
        /// <param name="vec2"></param>
        /// <returns></returns>
        public static Vector3 operator /(Vector3 vec1, Vector3 vec2)
        {
            return new Vector3()
            {
                X = vec1.Y * vec2.Z - vec1.Z * vec2.Y,
                Y = vec1.Z * vec2.X - vec1.X * vec2.Z,
                Z = vec1.X * vec2.Y - vec1.Y * vec2.X
            };
        }

        /// <summary>
        /// Increases the magnitude of the vector by the given amount
        /// </summary>
        /// <param name="vec"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static Vector3 operator +(Vector3 vec, double d)
        {
            vec.Mag += d;
            return vec;
        }

        /// <summary>
        /// Shorthand for getting the vector reflected across all axes
        /// </summary>
        /// <param name="vec"></param>
        /// <returns></returns>
        public static Vector3 operator -(Vector3 vec)
        {
            return vec.Inverse();
        }
    }
}
