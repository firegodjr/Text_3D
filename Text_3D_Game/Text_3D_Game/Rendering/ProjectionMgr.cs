using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Text_3D_Renderer.World;

namespace Text_3D_Renderer.Rendering
{
    /// <summary>
    /// An encapsulation of all important values needed in a projection matrix
    /// </summary>
    public class ProjectionMgr
    {
        bool recalcViewFrustum = true;
        private Frustum viewFrustum;
        public Frustum ViewspaceFrustum
        {
            get
            {
                if(recalcViewFrustum)
                {
                    viewFrustum = GetFrustum();
                    recalcViewFrustum = false;
                }

                return viewFrustum;
            }
        }

        double fov;
        double aspect;
        double nearDist;
        double farDist;
        bool leftHanded;

        /// <summary>
        /// The field of view
        /// </summary>
        public double Fov
        {
            get
            {
                return fov;
            }

            set
            {
                recalcViewFrustum = true;
                fov = value;
            }
        }

        /// <summary>
        /// The aspect ratio
        /// </summary>
        public double Aspect
        {
            get
            {
                return aspect;
            }

            set
            {
                recalcViewFrustum = true;
                aspect = value;
            }
        }

        /// <summary>
        /// The near distance of the frustum
        /// </summary>
        public double NearDist
        {
            get
            {
                return nearDist;
            }

            set
            {
                recalcViewFrustum = true;
                nearDist = value;
            }
        }

        /// <summary>
        /// The far distance of the frustum
        /// </summary>
        public double FarDist
        {
            get
            {
                return farDist;
            }

            set
            {
                recalcViewFrustum = true;
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
                recalcViewFrustum = true;
                leftHanded = value;
            }
        }

        /// <summary>
        /// Computes a projection matrix
        /// </summary>
        /// <returns></returns>
        public Matrix4 ComputeMatrix()
        {
            Matrix4 result = new Matrix4();
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

        /// <summary>
        /// Gets the frustum object associated with this 
        /// </summary>
        /// <param name="camera"></param>
        /// <returns></returns>
        public Frustum GetFrustum(Camera camera = null)
        {
            return new Frustum(fov, aspect, nearDist, farDist, leftHanded, camera);
        }
    }

    /// <summary>
    /// The view frustum, containing six planes
    /// </summary>
    public class Frustum
    {
        double fov, aspect, nearDist, farDist;
        Vector3 forward, right, up, origin;
        Plane[] planes = new Plane[6];
        bool leftHanded;
        Camera camera;

        Vector2 near
        {
            get { return getNearPlaneDimensions(); }
        }

        Vector2 far
        {
            get { return getFarPlaneDimensions(); }
        }

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

        /// <summary>
        /// Create a new frustum with the given perspective
        /// </summary>
        /// <param name="fov"></param>
        /// <param name="aspect"></param>
        /// <param name="nearDist"></param>
        /// <param name="farDist"></param>
        /// <param name="leftHanded"></param>
        /// <param name="camera"></param>
        public Frustum(double fov, double aspect, double nearDist, double farDist, bool leftHanded, Camera camera)
        {
            this.fov = fov;
            this.aspect = aspect;
            this.nearDist = nearDist;
            this.farDist = farDist;
            this.leftHanded = leftHanded;
            this.camera = camera;
            this.forward = camera == null ? new Vector3(0, 0, -1) : camera.GetForwardDirection();
            this.right = camera == null ? new Vector3(1, 0, 0) : camera.GetRightDirection();
            this.up = camera == null ? new Vector3(0, 1, 0) : camera.GetUpDirection();
            this.origin = camera == null ? new Vector3(0, 0, 0) : camera.GetVectorPosition();

            planes = new Plane[] { getNearPlane(), getLeftPlane(), getRightPlane(), getUpPlane(), getDownPlane(), getFarPlane() };
        }

        /// <summary>
        /// Gets if a given point is inside the frustum
        /// </summary>
        /// <param name="point"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public bool Contains(Vector3 point, double radius = 0)
        {
            radius = Math.Abs(radius);
            bool isWithin = true;
            string planeName = "";

            for (int i = 0; i < planes.Length; ++i)
            {
                Plane p = planes[i];
                double dist = p.SignedDist(point);
                if (dist > 0)
                {
                    if (i == 5)
                    {
                        if (Math.Abs(dist) < radius)
                        {
                            isWithin = false;
                        }
                    }
                    else
                    {
                        if(Math.Abs(dist) > radius)
                        {
                            isWithin = false;
                        }
                    }
                }
            }
            return isWithin;
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

        public Plane getFarPlane()
        {
            Vector3 farPlaneCenter = origin + (forward * farDist);
            Vector3 farTL = farPlaneCenter + (up * (far.Y / 2)) - (right * (far.X / 2));
            Vector3 farTR = farPlaneCenter + (up * (far.Y / 2)) + (right * (far.X / 2));
            Vector3 farBL = farPlaneCenter - (up * (far.Y / 2)) - (right * (far.X / 2));

            return new Plane(farTL, farTR, farBL);
        }

        public Plane getNearPlane()
        {
            Vector3 nearPlaneCenter = origin + (forward * nearDist);
            Vector3 nearTL = nearPlaneCenter + (up * (near.Y / 2)) - (right * (near.X / 2));
            Vector3 nearTR = nearPlaneCenter + (up * (near.Y / 2)) + (right * (near.X / 2));
            Vector3 nearBL = nearPlaneCenter - (up * (near.Y / 2)) - (right * (near.X / 2));

            return new Plane(nearTL, nearTR, nearBL);
        }

        public Plane getRightPlane(bool viewspace = false)
        {
            Vector3 p = origin;
            Vector3 d = forward;
            Vector3 nc = p + d * nearDist;
            Vector3 fc = p + d * farDist;

            Vector3 a = (nc + right * (getNearPlaneDimensions().X / 2)) - p;

            a.Normalize();
            Vector3 normalRight = up / a;

            return new Plane(normalRight, p);
        }

        public Plane getLeftPlane()
        {
            Vector3 p = origin;
            Vector3 d = forward;
            Vector3 nc = p + d * nearDist;
            Vector3 fc = p + d * farDist;

            Vector3 a = (nc + -right * (getNearPlaneDimensions().X / 2)) - p;

            a.Normalize();
            Vector3 normalLeft = -up / a;

            return new Plane(normalLeft, p);
        }

        public Plane getUpPlane()
        {
            Vector3 p = origin;
            Vector3 d = forward;
            Vector3 nc = p + d * nearDist;
            Vector3 fc = p + d * farDist;

            Vector3 a = (nc + up * (getNearPlaneDimensions().Y / 2)) - p;

            a.Normalize();
            Vector3 normalUp = -right / a;

            return new Plane(normalUp, p);
        }

        public Plane getDownPlane()
        {
            Vector3 p = origin;
            Vector3 d = forward;
            Vector3 nc = p + d * nearDist;
            Vector3 fc = p + d * farDist;

            Vector3 a = (nc + -up * (getNearPlaneDimensions().Y / 2)) - p;

            a.Normalize();
            Vector3 normalDown = right / a;

            return new Plane(normalDown, p);
        }

        public WOQuad GetNearPlaneAsQuad()
        {
            Vector3 nearPlaneCenter = origin + (forward * nearDist);
            Vector3 nearTL = nearPlaneCenter + (up * (near.Y / 2)) - (right * (near.X / 2));
            Vector3 nearTR = nearPlaneCenter + (up * (near.Y / 2)) + (right * (near.X / 2));
            Vector3 nearBL = nearPlaneCenter - (up * (near.Y / 2)) - (right * (near.X / 2));
            Vector3 nearBR = nearPlaneCenter - (up * (near.Y / 2)) + (right * (near.X / 2));

            return new WOQuad("nearplane", nearTL, nearTR, nearBR, nearBL);
        }

        public WOQuad GetFarPlaneAsQuad()
        {
            Vector3 farPlaneCenter = origin + (forward * farDist);
            Vector3 farTL = farPlaneCenter + (up * (far.Y / 2)) - (right * (far.X / 2));
            Vector3 farTR = farPlaneCenter + (up * (far.Y / 2)) + (right * (far.X / 2));
            Vector3 farBL = farPlaneCenter - (up * (far.Y / 2)) - (right * (far.X / 2));
            Vector3 farBR = farPlaneCenter - (up * (far.Y / 2)) + (right * (far.X / 2));

            return new WOQuad("farplane", farTL, farTR, farBR, farBL);
        }

        public WOQuad GetLeftPlaneAsQuad()
        {
            Vector3 farPlaneCenter = origin + (forward * farDist);
            Vector3 farTL = farPlaneCenter + (up * (far.Y / 2)) - (right * (far.X / 2));
            Vector3 farBL = farPlaneCenter - (up * (far.Y / 2)) - (right * (far.X / 2));
            Vector3 nearPlaneCenter = origin + (forward * nearDist);
            Vector3 nearTL = nearPlaneCenter + (up * (near.Y / 2)) - (right * (near.X / 2));
            Vector3 nearBL = nearPlaneCenter - (up * (near.Y / 2)) - (right * (near.X / 2));

            return new WOQuad("leftplane", farTL, farBL, nearBL, nearTL);
        }

        public WOQuad GetRightPlaneAsQuad()
        {
            Vector3 farPlaneCenter = origin + (forward * farDist);
            Vector3 farTR = farPlaneCenter + (up * (far.Y / 2)) + (right * (far.X / 2));
            Vector3 farBR = farPlaneCenter - (up * (far.Y / 2)) + (right * (far.X / 2));
            Vector3 nearPlaneCenter = origin + (forward * nearDist);
            Vector3 nearTR = nearPlaneCenter + (up * (near.Y / 2)) + (right * (near.X / 2));
            Vector3 nearBR = nearPlaneCenter - (up * (near.Y / 2)) + (right * (near.X / 2));

            return new WOQuad("rightplane", farTR, farBR, nearBR, nearTR);
        }
    }
}
