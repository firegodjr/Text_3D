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
    /// A primitive shape made up of six separate quads
    /// </summary>
    public class Cube : PolyObject
    {
        bool isTransparent = false;

        /// <summary>
        /// If this cube should have filled faces or not
        /// </summary>
        protected bool IsTransparent
        {
            get { return isTransparent; }
        }

        public Cube()
        {
            children.AddRange(new WorldObject[]
            {
                new WOQuad("1", new Vector3(0, 0, 1)),
                new WOQuad("2", new Vector3(0, 0, -1)),
                new WOQuad("3", new Vector3(1, 0, 0)).Rotate(0, Math.PI/2, 0),
                new WOQuad("4", new Vector3(-1, 0, 0)).Rotate(0, Math.PI/2, 0),
                new WOQuad("5", new Vector3(0, 1, 0)).Rotate(Math.PI/2, 0, 0),
                new WOQuad("6", new Vector3(0, -1, 0)).Rotate(Math.PI/2, 0, 0)
            });
        }

        public Cube(bool isTransparent) : this()
        {
            this.isTransparent = isTransparent;
            foreach (WorldObject c in children)
            {
                c.UseParentOrigin();
            }
        }

        public Cube(string name) : this(true)
        {
            this.name = name;
        }

        public Cube(string name, bool isTransparent) : this(isTransparent)
        {
            this.name = name;
        }

        public Cube(string name, Vector3 position) : this(true)
        {
            Translate(position);
            this.name = name;
        }

        ///// <summary>
        ///// Draws a cube to the ScreenMgr or to a given FrameLayer
        ///// </summary>
        ///// <param name="mgr">The ScreenMgr that will handle the drawing</param>
        ///// <param name="viewMatrix">The View Matrix</param>
        ///// <param name="projectionMatrix">The Projection Matrix</param>
        ///// <param name="camera">The camera</param>
        ///// <param name="frustum">The view frustum, passed here to allow for sub-object culling</param>
        ///// <param name="parentMatrix">A model matrix to use instead of this object's model matrix</param>
        ///// <param name="fl">A FrameLayer to render the pixels to</param>
        ///// <returns></returns>
        //public override RenderObject[] Render(ScreenMgr mgr, Matrix viewMatrix, Matrix projectionMatrix, Camera camera, Frustum frustum, Matrix parentMatrix = null, FrameLayer fl = null)
        //{
        //    //TODO: remove
        //    RenderObject[] renderObjs = new RenderObject[6];

        //    FrameLayer[] quadBuffers = new FrameLayer[6];
        //    Matrix cubeMatrix = GetModelMatrix();

        //    //Sort the quads by 
        //    List<WorldObject> sortQuads = new List<WorldObject>(quads).OrderBy(obj => WorldUtil.GetVectorDistance(camera.GetVectorPosition(), WorldUtil.getCenterFromPoints(cubeMatrix, ((Quad)obj).Verts))).Reverse().ToList();
            
        //    for(int i = 0; i < sortQuads.Count; ++i)
        //    {
        //        sortQuads[i].Render(mgr, viewMatrix, projectionMatrix, camera, frustum, cubeMatrix, fl);
        //    }

        //    return renderObjs;
        //}
    }
}
