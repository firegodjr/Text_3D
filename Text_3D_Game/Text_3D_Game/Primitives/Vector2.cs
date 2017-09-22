using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_3D_Renderer.Rendering
{

    public class Vector2 : ICloneable
    {
        private double x, y;

        public Vector2()
        {
            X = 0;
            Y = 0;
        }

        public Vector2(double x, double y)
        {
            this.X = x;
            this.Y = y;
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

        public object Clone()
        {
            return new Vector2(X, Y);
        }

        public double Dot(Vector2 vec)
        {
            return X * vec.X + Y * vec.Y;
        }
    }
}
