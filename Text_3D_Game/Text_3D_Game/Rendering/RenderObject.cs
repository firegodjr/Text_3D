using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Text_3D_Renderer.World;

namespace Text_3D_Renderer.Rendering
{
    /// <summary>
    /// An encapsulation of a FrameLayer with its own weight and position for calculating its priority to the camera
    /// </summary>
    public class RenderObject
    {
        private double weight;
        private Vector3 position;
        private int color;

        /// <summary>
        /// Gets or sets the color of this RenderObject
        /// </summary>
        public int Color
        {
            get { return color; }
            set { color = value; }
        }

        /// <summary>
        /// The relative weight of this RenderObject. Multiple renderobjects with the same position will be ordered based on weight.
        /// </summary>
        public double Weight
        {
            get
            {
                return weight;
            }

            set
            {
                weight = value;
            }
        }

        /// <summary>
        /// The position in worldspace that this RenderObject came from
        /// </summary>
        public Vector3 Position
        {
            get
            {
                return position;
            }

            set
            {
                position = value;
            }
        }

        /// <summary>
        /// Creates a new RenderObject with given FrameLayer, weight, color and position
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="weight"></param>
        /// <param name="color"></param>
        /// <param name="position"></param>
        public RenderObject(double weight, int color, Vector3 position)
        {
            Weight = weight;
            Position = position;
            Color = color;
        }

        /// <summary>
        /// Gets the distance between the camera and this RenderObject
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public double GetCameraDist(Camera c)
        {
            return World.WorldUtil.GetVectorDistance(c.GetPosition(), position);
        }
    }
}
