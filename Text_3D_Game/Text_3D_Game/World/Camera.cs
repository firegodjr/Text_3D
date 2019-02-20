using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Text_3D_Renderer.World;
using Text_3D_Renderer.Rendering;

namespace Text_3D_Renderer.World
{
    /// <summary>
    /// A camera for rendering the world from a certain viewpoint
    /// </summary>
    public class Camera : WorldObject
    {
        /// <summary>
        /// The amount of pixels something can be offscreen before it is culled
        /// </summary>
        public const int OFFSCREEN_ALLOWANCE = 100;

        private int[] customProperties = new int[4];

        /// <summary>
        /// Custom integer properties that can be applied to this camera
        /// </summary>
        public int[] Properties
        {
            get { return customProperties; }
        }

        private double viewDist = 30;

        public double ViewDist
        {
            get { return viewDist; }
            set { viewDist = value; }
        }

        /// <summary>
        /// The pitch of the camera
        /// </summary>
        public double Pitch
        {
            get
            {
                return RotationMatrix.Pitch;
            }
        }

        /// <summary>
        /// The yaw of the camera
        /// </summary>
        public double Yaw
        {
            get
            {
                return RotationMatrix.Yaw;
            }
        }

        /// <summary>
        /// The roll of the camera
        /// </summary>
        public double Roll
        {
            get
            {
                return RotationMatrix.Roll;
            }
        }

        /// <summary>
        /// Creates a new camera 
        /// </summary>
        /// <param name="name"></param>
        public Camera() : this(new Vector3(), new Vector3(), new Vector3()) { }

        /// <summary>
        /// Creates a new camera with given transform, rotation and scale, and optional name
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="scale"></param>
        /// <param name="rotation"></param>
        /// <param name="name"></param>
        public Camera(Vector3 transform, Vector3 rotation, Vector3 scale, string name = "camera")
        {
            this.name = name;
            transform = (Vector3)transform.Clone();
            rotation = (Vector3)rotation.Clone();
            scale = (Vector3)scale.Clone();
        }

        /// <summary>
        /// Gets a view matrix from this camera's properties
        /// </summary>
        /// <returns></returns>
        public Matrix4 GetViewMatrix()
        {
            return (TranslationMatrix * RotationMatrix * ScaleMatrix).Inverse();
        }
    }

}
