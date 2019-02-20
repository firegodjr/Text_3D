using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_3D_Renderer
{
    /// <summary>
    /// A 2-dimensional vector
    /// </summary>
    public class Vector2 : ICloneable
    {
        private double x, y;

        /// <summary>
        /// Creates a new 2d vector at 0,0
        /// </summary>
        public Vector2()
        {
            X = 0;
            Y = 0;
        }

        /// <summary>
        /// Creates a new 2d vector with given values
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Vector2(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// Vector X value
        /// </summary>
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

        /// <summary>
        /// Vector Y value
        /// </summary>
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

        /// <summary>
        /// Deeply clones the vector
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return new Vector2(X, Y);
        }

        public override string ToString()
        {
            return $"{X},{Y}";
        }
    }
}
