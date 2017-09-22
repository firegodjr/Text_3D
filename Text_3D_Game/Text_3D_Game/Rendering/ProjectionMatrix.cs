using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Text_3D_Renderer.Rendering.Primitives;

namespace Text_3D_Renderer.Rendering
{
    public class ProjectionMatrix
    {

        double fov;
        double aspect;
        double nearDist;
        double farDist;
        bool leftHanded;

        public double Fov
        {
            get
            {
                return fov;
            }

            set
            {
                fov = value;
            }
        }

        public double Aspect
        {
            get
            {
                return aspect;
            }

            set
            {
                aspect = value;
            }
        }

        public double NearDist
        {
            get
            {
                return nearDist;
            }

            set
            {
                nearDist = value;
            }
        }

        public double FarDist
        {
            get
            {
                return farDist;
            }

            set
            {
                farDist = value;
            }
        }

        public bool LeftHanded
        {
            get
            {
                return leftHanded;
            }

            set
            {
                leftHanded = value;
            }
        }

        public Matrix computeMatrix()
        {
            Matrix result = new Matrix();
            result.clear();

            if (Fov <= 0 || Aspect == 0)
            {
                if (!(Fov > 0 && Aspect != 0))
                {
                    Console.WriteLine("The fov or aspect is invalid.\nThings are about to go terribly wrong, aborting...");
                }

                throw new Exception();
            }

            double frustumDepth = FarDist - NearDist;
            double oneOverDepth = 1 / frustumDepth;

            result.ColumnwiseArray[1][1] = (1 / Math.Tan(0.5 * Fov));
            result.ColumnwiseArray[0][0] = (LeftHanded ? 1 : -1) * result.ColumnwiseArray[1][1] / Aspect;
            result.ColumnwiseArray[2][2] = FarDist * oneOverDepth;
            result.ColumnwiseArray[2][3] = (-FarDist * NearDist) * oneOverDepth;
            result.ColumnwiseArray[3][2] = 1;
            result.ColumnwiseArray[3][3] = 0;

            return result;
        }

        public Frustum getFrustum(Camera camera)
        {
            return new Frustum(fov, aspect, nearDist, farDist, leftHanded, camera);
        }
    }

    public class Frustum
    {
        double fov, aspect, nearDist, farDist;
        Plane[] planes = new Plane[6];
        bool leftHanded;

        Vector2 near
        {
            get { return getNearPlaneDimensions(); }
        }

        Vector2 far
        {
            get { return getFarPlaneDimensions(); }
        }

        public Frustum(double fov, double aspect, double nearDist, double farDist, bool leftHanded, Camera camera)
        {
            this.fov = fov;
            this.aspect = aspect;
            this.nearDist = nearDist;
            this.farDist = farDist;
            this.leftHanded = leftHanded;

            Vector3 lookdir = camera.getLookDirection();
            Vector3 center = lookdir + camera.getPosition();

            planes[0] = new Plane(lookdir.X, lookdir.Y, lookdir.Z);
        }

        Vector2 getNearPlaneDimensions()
        {
            double hnear = 2 * Math.Tan(fov / 2) * nearDist;
            double wnear = hnear * aspect;
            return new Vector2(wnear, hnear);
        }

        Vector2 getFarPlaneDimensions()
        {
            double hfar = 2 * Math.Tan(fov / 2) * farDist;
            double wfar = hfar * aspect;
            return new Vector2(wfar, hfar);
        }
    }
}
