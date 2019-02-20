using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Text_3D_Renderer.Rendering;

namespace Text_3D_Renderer.World
{
    /// <summary>
    /// A class used to generate different types of matrices.
    /// </summary>
    public static class MatrixMaker
    {
        /// <summary>
        /// Computes a projection matrix
        /// </summary>
        /// <param name="fov"></param>
        /// <param name="aspect"></param>
        /// <param name="nearDist"></param>
        /// <param name="farDist"></param>
        /// <param name="leftHanded"></param>
        /// <returns></returns>
        public static Matrix4 ComputeFOVProjection(float fov, float aspect, float nearDist, float farDist, bool leftHanded /* = true */ )
        {
            Matrix4 result = new Matrix4().clear();
            //
            // General form of the Projection Matrix
            //
            // uh = Cot( fov/2 ) == 1/Tan(fov/2)
            // uw / uh = 1/aspect
            // 
            //   uw         0       0       0
            //    0        uh       0       0
            //    0         0      f/(f-n)  1
            //    0         0    -fn/(f-n)  0
            //
            // Make result to be identity first

            // check for bad parameters to avoid divide by zero:
            // if found, assert and return an identity matrix.

            if (fov <= 0 || aspect == 0)
            {
                if (!(fov > 0 && aspect != 0))
                {
                    Console.WriteLine("The fov or aspect is invalid.\nThings are about to go terribly wrong, aborting...");
                }

                return new Matrix4();
            }

            float frustumDepth = farDist - nearDist;
            float oneOverDepth = 1 / frustumDepth;

            result.ColumnwiseArray[1][1] = (1 / Math.Tan(0.5f * fov));
            result.ColumnwiseArray[0][0] = (leftHanded ? 1 : -1) * result.ColumnwiseArray[1][1] / aspect;
            result.ColumnwiseArray[2][2] = farDist * oneOverDepth;
            result.ColumnwiseArray[2][3] = (-farDist * nearDist) * oneOverDepth;
            result.ColumnwiseArray[3][2] = 1;
            result.ColumnwiseArray[3][3] = 0;

            return result;
        }

        /// <summary>
        /// Creates a new translation matrix from three doubles
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="Z"></param>
        /// <returns></returns>
        public static Matrix4 GetTranslationMatrix(double X = 0, double Y = 0, double Z = 0)
        {
            Matrix4 output = new Matrix4();
            output.X = X;
            output.Y = Y;
            output.Z = Z;
            return output;
        }

        /// <summary>
        /// Creates a new translation matrix from a vector
        /// </summary>
        /// <param name="vec"></param>
        /// <returns></returns>
        public static Matrix4 GetTranslationMatrix(Vector3 vec)
        {
            Matrix4 output = new Matrix4();
            output.X = vec.X;
            output.Y = vec.Y;
            output.Z = vec.Z;
            return output;
        }

        /// <summary>
        /// Creates a new scale matrix from three doubles
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="Z"></param>
        /// <returns></returns>
        public static Matrix4 GetScaleMatrix(double X = 1, double Y = 1, double Z = 1)
        {
            Matrix4 output = new Matrix4();
            output.ColumnwiseArray[0][0] = X;
            output.ColumnwiseArray[1][1] = Y;
            output.ColumnwiseArray[2][2] = Z;
            return output;
        }

        /// <summary>
        /// Creates a new scale matrix from a single double
        /// </summary>
        /// <param name="XYZ"></param>
        /// <returns></returns>
        public static Matrix4 GetScaleMatrix(double XYZ = 1)
        {
            Matrix4 output = new Matrix4();
            output.ColumnwiseArray[0][0] = XYZ;
            output.ColumnwiseArray[1][1] = XYZ;
            output.ColumnwiseArray[2][2] = XYZ;
            return output;
        }

        /// <summary>
        /// Creates a new rotation matrix from a radian measure and an axis
        /// </summary>
        /// <param name="radians"></param>
        /// <param name="axis"></param>
        /// <returns></returns>
        public static Matrix4 GetRotationMatrix(double radians, int axis)
        {
            if(axis == Rotation.X)
            {
                return new Matrix4().newFromRowwise(new double[][] {
                    new double[] { 1, 0, 0, 0 },
                    new double[] { 0, Math.Cos(radians), Math.Sin(radians), 0 },
                    new double[] { 0, -Math.Sin(radians), Math.Cos(radians), 0 },
                    new double[] { 0, 0, 0, 1 }
                });
            }
            else if(axis == Rotation.Y)
            {
                return new Matrix4().newFromRowwise(new double[][] {
                    new double[] { Math.Cos(radians), 0, -Math.Sin(radians), 0 },
                    new double[] { 0, 1, 0, 0 },
                    new double[] { Math.Sin(radians), 0, Math.Cos(radians), 0 },
                    new double[] { 0, 0, 0, 1 }
                });
            }
            else if(axis == Rotation.Z)
            {
                return new Matrix4().newFromRowwise(new double[][] {
                    new double[] { Math.Cos(radians), Math.Sin(radians), 0, 0 },
                    new double[] { -Math.Sin(radians), Math.Cos(radians), 0, 0 },
                    new double[] { 0, 0, 1, 0 },
                    new double[] { 0, 0, 0, 1 }
                });
            }
            else
            {
                return new Matrix4();
            }
        }

        public static Matrix4 GetRotationMatrix(Vector3 euler)
        {
            double sa, ca, sb, cb, sh, ch;
            sa = Math.Sin(euler.Z);
            ca = Math.Cos(euler.Z);
            sb = Math.Sin(euler.X);
            cb = Math.Cos(euler.X);
            sh = Math.Sin(-euler.Y);
            ch = Math.Cos(-euler.Y);
            return new Matrix4().newFromRowwise(new double[][]
            {
                new double[] {ch*ca, -ch*sa*cb + sh*sb, ch*sa*sb + sh*cb, 0},
                new double[] {sa, ca*cb, -ca*sb, 0},
                new double[] {-sh*ca, sh*sa*cb + ch*sb, -sh*sa*sb + ch*cb, 0},
                new double[] {0, 0, 0, 1}
            });
        }
    }

    /// <summary>
    /// Contains representations of the three axes
    /// </summary>
    public class Rotation
    {
        /// <summary>
        /// The X Axis
        /// </summary>
        public const int X = 0;
        /// <summary>
        /// The Y Axis
        /// </summary>
        public const int Y = 1;
        /// <summary>
        /// The Z Axis
        /// </summary>
        public const int Z = 2;
    }
}
