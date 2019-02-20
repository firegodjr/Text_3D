using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_3D_Renderer.World
{
    public static class WorldUtil
    {
        public static Vector3 getCenterFromPoints(Matrix4 multiplier = null, params Vector3[] points)
        {
            double posX = 0, posY = 0, posZ = 0;
            for (int i = 0; i < points.Length; ++i)
            {
                Vector3 pos =  multiplier != null ? points[i] * multiplier : points[i];
                posX += pos.X;
                posY += pos.Y;
                posZ += pos.Z;
            }

            posX /= points.Length;
            posY /= points.Length;
            posZ /= points.Length;

            return new Vector3(posX, posY, posZ);
        }
        
        /// <summary>
        /// Gets the distance between two world objects
        /// </summary>
        /// <param name="wo1"></param>
        /// <param name="wo2"></param>
        /// <returns></returns>
        public static double GetWODistance(WorldObject wo1, WorldObject wo2)
        {
            if (wo1.RelativeToView || wo2.RelativeToView)
            {
                return 0;
            }

            Vector3 wo1pos = wo1.GetPosition();
            Vector3 wo2pos = wo2.GetPosition();
            return Math.Sqrt(Math.Pow(wo1pos.X - wo2pos.X, 2) + Math.Pow(wo1pos.Y - wo2pos.Y, 2) + Math.Pow(wo1pos.Z - wo2pos.Z, 2));
        }

        /// <summary>
        /// Gets the distance between two translation matrices
        /// </summary>
        /// <param name="wo1pos"></param>
        /// <param name="wo2pos"></param>
        /// <returns></returns>
        public static double GetWODistance(Matrix4 wo1pos, Matrix4 wo2pos)
        {
            return Math.Sqrt(Math.Pow(wo1pos.X - wo2pos.X, 2) + Math.Pow(wo1pos.Y - wo2pos.Y, 2) + Math.Pow(wo1pos.Z - wo2pos.Z, 2));
        }

        /// <summary>
        /// Gets the distance between two vectors
        /// </summary>
        /// <param name="wo1pos"></param>
        /// <param name="wo2pos"></param>
        /// <returns></returns>
        public static double GetVectorDistance(Vector3 wo1pos, Vector3 wo2pos)
        {
            return Math.Sqrt(Math.Pow(wo1pos.X - wo2pos.X, 2) + Math.Pow(wo1pos.Y - wo2pos.Y, 2) + Math.Pow(wo1pos.Z - wo2pos.Z, 2));
        }
    }
}
