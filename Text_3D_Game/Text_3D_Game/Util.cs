using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_3D_Renderer
{
    /// <summary>
    /// General utilities for math
    /// </summary>
    public static class Util
    {
        public static double[] Interpolate(int i0, double d0, int i1, double d1)
        {
            if(i0 == i1)
            {
                return new double[] { d0 };
            }

            List<double> values = new List<double>();
            double slope = (d1 - d0) / (i1 - i0);
            double value = d0;

            for(int i = i0; i < i1; ++i)
            {
                values.Add(value);
                value += slope;
            }

            return values.ToArray();
        }
    }
}
