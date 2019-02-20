using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Text_3D_Renderer.Rendering;

namespace Text_3D_Renderer.World
{
    public class WOLine : WorldObject
    {
        Vector3 point1, point2;

        public WOLine(Vector3 point1, Vector3 point2) : base()
        {
            this.point1 = point1;
            this.point2 = point2;
        }

        public override RenderObject[] Render(ScreenMgr mgr, Matrix4 viewMatrix, Matrix4 projectionMatrix, Frustum frustum, Matrix4 parentMatrix = null, FrameLayer fl = null)
        {
            Matrix4 modelMatrix = GetModelMatrix();
            Matrix4 MVPMatrix = modelMatrix * viewMatrix * projectionMatrix;
            //mgr.Draw3DLine(point1, point2, modelMatrix, MVPMatrix, camera, frustum, fl);

            return new RenderObject[0];
        }
    }
}
