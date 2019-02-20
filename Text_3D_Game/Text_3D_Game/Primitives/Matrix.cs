using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_3D_Renderer
{
    /// <summary>
    /// A columnwise 4x4 matrix for representing transformation and other data in a 3d space
    /// </summary>
    public class Matrix4 : ICloneable
    {
        private double[][] array;

        /// <summary>
        /// If this has position data, this is the X value
        /// </summary>
        public double X
        {
            get { return array[3][0]; }
            set { array[3][0] = value; }
        }

        /// <summary>
        /// If this has position data, this is the Y value
        /// </summary>
        public double Y
        {
            get { return array[3][1]; }
            set { array[3][1] = value; }
        }

        /// <summary>
        /// If this has position data, this is the Z value
        /// </summary>
        public double Z
        {
            get { return array[3][2]; }
            set { array[3][2] = value; }
        }

        /// <summary>
        /// If this has rotation data, this is its pitch radians.
        /// </summary>
        public double Pitch
        {
            get { return Math.Atan2(array[0][2], array[1][2]); }
        }

        /// <summary>
        /// If this has rotation data, this is its yaw radians.
        /// </summary>
        public double Yaw
        {
            get { return Math.Acos(array[2][2]); }
        }

        /// <summary>
        /// If this has rotation data, this is its roll radians.
        /// </summary>
        public double Roll
        {
            get { return Math.Atan2(array[2][0], array[2][1]); }
        }

        /// <summary>
        /// The internal 2d array, columnwise
        /// </summary>
        public double[][] ColumnwiseArray
        {
            get
            {
                return array;
            }

            set
            {
                array = value;
            }
        }

        /// <summary>
        /// The internal 2d array, rowwise
        /// </summary>
        public double[][] RowwiseArray
        {
            get
            {
                return getRowwise();
            }
            set
            {
                newFromRowwise(value);
            }
        }

        /// <summary>
        /// Creates a new identity matrix
        /// </summary>
        public Matrix4() : this(new double[] { 1, 0, 0, 0 }, new double[] { 0, 1, 0, 0 }, new double[] { 0, 0, 1, 0 }, new double[] { 0, 0, 0, 1 }) { }

        /// <summary>
        /// Creates a new matrix from four columns
        /// </summary>
        /// <param name="col1"></param>
        /// <param name="col2"></param>
        /// <param name="col3"></param>
        /// <param name="col4"></param>
        public Matrix4(double[] col1, double[] col2, double[] col3, double[] col4)
        {
            array = new double[][] { col1, col2, col3, col4 };
        }

        /// <summary>
        /// Creates a new matrix from a columnwise 4x4 array of doubles.
        /// </summary>
        /// <param name="array"></param>
        public Matrix4(double[][] array)
        {
            if (array.Length == 4 && array[0].Length == 4)
            {
                this.array = array;
            }
        }

        /// <summary>
        /// Multiplies one matrix by another and return the result
        /// </summary>
        /// <param name="m1"></param>
        /// <param name="m2"></param>
        /// <returns></returns>
        public static Matrix4 Multiply(Matrix4 m1, Matrix4 m2)
        {
            Matrix4 output = new Matrix4();

            
            for(byte i = 0; i < 4; ++i)
            {
                for (byte j = 0; j < 4; ++j)
                {
                    double multresult = 0;
                    for (byte k = 0; k < 4; ++k)
                    {
                        multresult += m1.array[k][j] * m2.array[i][k];
                    }
                    output.array[i][j] = multresult;

                    if(multresult.ToString() == "NaN")
                    {
                        Console.WriteLine("Matrix multiplication error");
                    }
                }
            }
            return output;
        }

        /// <summary>
        /// Multiplies a vector3 by a matrix and returns the vector3 result
        /// </summary>
        /// <param name="vec"></param>
        /// <param name="m1"></param>
        /// <returns></returns>
        public static Vector3 MultiplyVector(Vector3 vec, Matrix4 m1)
        {
            double[] m2 = new double[] { vec.X, vec.Y, vec.Z, vec.W };
            double[] output = new double[4];
            for(byte i = 0; i < 4; ++i)
            {
                double result = 0;
                for (byte j = 0; j < 4; ++j)
                {
                    result += m2[j] * m1.ColumnwiseArray[j][i];
                }
                output[i] = result;
            }

            if(output[3] != 0)
            {
                return new Vector3(output[0] / output[3], output[1] / output[3], output[2] / output[3], (int)output[3]);
            }
            else
            {
                return new Vector3(output[0], output[1], output[2], (int)output[3]);
            }
        }

        /// <summary>
        /// Sets all values in the matrix to 0
        /// </summary>
        public Matrix4 clear()
        {
            array = array = new double[][] { new double[4], new double[4], new double[4], new double[4] };
            return this;
        }

        /// <summary>
        /// Deeply clones the matrix
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return new Matrix4(
                new double[] { array[0][0], array[0][1], array[0][2], array[0][3] }, 
                new double[] { array[1][0], array[1][1], array[1][2], array[1][3] }, 
                new double[] { array[2][0], array[2][1], array[2][2], array[2][3] }, 
                new double[] { array[3][0], array[3][1], array[3][2], array[3][3] }
                );
        }

        /// <summary>
        /// Returns the result of multiplying this matrix with a new one. Does not affect either matrix.
        /// </summary>
        /// <param name="m2"></param>
        /// <returns></returns>
        public Matrix4 multiplyBy(Matrix4 m2)
        {
            return Multiply(this, m2);
        }

        /// <summary>
        /// Transforms the matrix using a vector
        /// </summary>
        /// <param name="vec"></param>
        /// <returns></returns>
        public Matrix4 transformBy(Vector3 vec)
        {
            double[] vecArray = new double[] { vec.X, vec.Y, vec.Z };
            for (byte i = 0; i < 3; ++i)
            {
                for(byte j = 0; j < 3; ++j)
                {
                    ColumnwiseArray[3][i] += vecArray[i] * RowwiseArray[i][j];
                }
            }
            return this;
        }

        /// <summary>
        /// Returns the inverse matrix
        /// </summary>
        /// <returns></returns>
        public Matrix4 Inverse()
        {
            return new Matrix4().newFromRowwise(MatrixInverter.MatrixInverse(getRowwise()));
        }
        
        /// <summary>
        /// Returns the rowwise version of the internal array
        /// </summary>
        /// <returns></returns>
        public double[][] getRowwise()
        {
            double[][] output = new double[4][];

            for(byte i = 0; i < 4; ++i)
            {
                output[i] = new double[4];
                for(byte j = 0; j < 4; ++j)
                {
                    output[i][j] = array[j][i];
                }
            }

            return output;
        }

        /// <summary>
        /// Replaces the internal 2d array with a columnwise array parsed from a given rowwise array
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Matrix4 newFromRowwise(double[][] input)
        {
            for(byte i = 0; i < 4; ++i)
            {
                for (byte j = 0; j < 4; ++j)
                {
                    array[j][i] = input[i][j];
                }
            }

            return this;
        }

        /// <summary>
        /// Get this Matrix as a position vector
        /// </summary>
        /// <returns></returns>
        public Vector3 ToVector()
        {
            return new Vector3(X, Y, Z);
        }

        /// <summary>
        /// Multiplies two matrices together
        /// </summary>
        /// <param name="m1"></param>
        /// <param name="m2"></param>
        /// <returns></returns>
        public static Matrix4 operator *(Matrix4 m1, Matrix4 m2)
        {
            return Multiply(m1, m2);
        }

        /// <summary>
        /// Multiplies a vector by a matrix
        /// </summary>
        /// <param name="v"></param>
        /// <param name="m1"></param>
        /// <returns></returns>
        public static Vector3 operator *(Vector3 v, Matrix4 m1)
        {
            return MultiplyVector(v, m1);
        }

        /// <summary>
        /// Multiplies a vector by a matrix
        /// </summary>
        /// <param name="v"></param>
        /// <param name="m1"></param>
        /// <returns></returns>
        public static Vector3 operator *(Matrix4 m1, Vector3 v)
        {
            return MultiplyVector(v, m1);
        }
    }

}
