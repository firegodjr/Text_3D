using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Text_3D_Renderer.Rendering;

namespace Text_3D_Renderer.Movement
{
    public class MatrixMaker
    {
        public static Matrix ComputeFOVProjection(float fov, float aspect, float nearDist, float farDist, bool leftHanded /* = true */ )
        {
            Matrix result = new Matrix().clear();
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

                return new Matrix();
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
        public static Matrix GetTranslationMatrix(double X = 0, double Y = 0, double Z = 0)
        {
            Matrix output = new Matrix();
            output.X = X;
            output.Y = Y;
            output.Z = Z;
            return output;
        }

        public static Matrix GetTranslationMatrix(Vector3 vec)
        {
            Matrix output = new Matrix();
            output.X = vec.X;
            output.Y = vec.Y;
            output.Z = vec.Z;
            return output;
        }

        public static Matrix GetScaleMatrix(double X = 1, double Y = 1, double Z = 1)
        {
            Matrix output = new Matrix();
            output.ColumnwiseArray[0][0] = X;
            output.ColumnwiseArray[1][1] = Y;
            output.ColumnwiseArray[2][2] = Z;
            return output;
        }

        public static Matrix GetScaleMatrix(double XYZ = 1)
        {
            Matrix output = new Matrix();
            output.ColumnwiseArray[0][0] = XYZ;
            output.ColumnwiseArray[1][1] = XYZ;
            output.ColumnwiseArray[2][2] = XYZ;
            return output;
        }

        public static Matrix GetRotationMatrix(double radians, int axis)
        {
            if(axis == Rotation.X)
            {
                return new Matrix().newFromRowwise(new double[][] {
                    new double[] { 1, 0, 0, 0 },
                    new double[] { 0, Math.Cos(radians), Math.Sin(radians), 0 },
                    new double[] { 0, -Math.Sin(radians), Math.Cos(radians), 0 },
                    new double[] { 0, 0, 0, 1 }
                });
            }
            else if(axis == Rotation.Y)
            {
                return new Matrix().newFromRowwise(new double[][] {
                    new double[] { Math.Cos(radians), 0, -Math.Sin(radians), 0 },
                    new double[] { 0, 1, 0, 0 },
                    new double[] { Math.Sin(radians), 0, Math.Cos(radians), 0 },
                    new double[] { 0, 0, 0, 1 }
                });
            }
            else if(axis == Rotation.Z)
            {
                return new Matrix().newFromRowwise(new double[][] {
                    new double[] { Math.Cos(radians), Math.Sin(radians), 0, 0 },
                    new double[] { -Math.Sin(radians), Math.Cos(radians), 0, 0 },
                    new double[] { 0, 0, 1, 0 },
                    new double[] { 0, 0, 0, 1 }
                });
            }
            else
            {
                return new Matrix();
            }
        }
    }

    public class Rotation
    {
        public const int X = 0;
        public const int Y = 1;
        public const int Z = 2;
    }
}
