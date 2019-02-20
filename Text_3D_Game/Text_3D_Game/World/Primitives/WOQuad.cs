using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Text_3D_Renderer.World;
using Text_3D_Renderer.Rendering;

namespace Text_3D_Renderer.World
{
    public class WOQuad : WorldObject
    {
        bool isTransparent = false;
        Vector3[] verts = new Vector3[4];
        Vector3[] tempVerts = new Vector3[4];

        public Vector3[] Verts
        {
            get
            {
                return verts;
            }

            set
            {
                verts = value;
            }
        }

        public bool IsTransparent
        {
            get
            {
                return isTransparent;
            }

            set
            {
                isTransparent = value;
            }
        }

        public WOQuad(string name) : this(name, new Vector3(), false)
        {

        }

        public WOQuad(string name, Vector3 position, bool isTransparent = false)
        {
            this.name = name.ToLower();
            this.IsTransparent = isTransparent;
            verts[0] = new Vector3(-1, -1, 0);
            verts[1] = new Vector3(-1, 1, 0);
            verts[2] = new Vector3(1, 1, 0);
            verts[3] = new Vector3(1, -1, 0);

            for(int i = 0; i < 4; ++i)
            {
                tempVerts[i] = new Vector3();
            }

            base.position = (Vector3)position.Clone();
        }

        public WOQuad(string name, Vector3 vec1, Vector3 vec2, Vector3 vec3, Vector3 vec4)
        {
            this.name = name.ToLower();
            verts[0] = (Vector3)vec1.Clone();
            verts[1] = (Vector3)vec2.Clone();
            verts[2] = (Vector3)vec3.Clone();
            verts[3] = (Vector3)vec4.Clone();

            for (int i = 0; i < 4; ++i)
            {
                tempVerts[i] = new Vector3();
            }
        }

        public WOQuad(string name, Vector3[] vecs) : this(name, vecs[0], vecs[1], vecs[2], vecs[3]) { }

        public override object Clone()
        {
            WOQuad clone = new WOQuad(name);
            clone.position = (Vector3)position.Clone();
            clone.rotation = (Vector3)rotation.Clone();
            clone.scale = (Vector3)scale.Clone();
            clone.tempTranslation = (Matrix4)tempTranslation.Clone();
            clone.tempRotation = (Matrix4)tempRotation.Clone();
            clone.tempScale = (Matrix4)tempScale.Clone();

            for (byte i = 0; i < verts.Length; ++i)
            {
                clone.verts[i] = (Vector3)verts[i].Clone();
            }

            return clone;
        }

        public override void UseParentOrigin()
        {
            for(byte i = 0; i < verts.Length; ++i)
            {
                tempVerts[i] = verts[i];
                verts[i] = verts[i] * GetModelMatrix();
            }

            base.UseParentOrigin();
        }

        public override void RevertParentOrigin()
        {
            for (byte i = 0; i < verts.Length; ++i)
            {
                verts[i] = tempVerts[i];
            }

            base.RevertParentOrigin();
        }

        public string getName()
        {
            return name;
        }

        public void setName(string name)
        {
            this.name = name;
        }

        public override RenderObject[] Render(ScreenMgr mgr, Matrix4 viewMatrix, Matrix4 projectionMatrix, Frustum frustum, Matrix4 modelMatrix = null, FrameLayer fl = null)
        {
            if (fl == null)
            {
                fl = new FrameLayer(mgr.Width, mgr.Height);
            }

            modelMatrix = modelMatrix ?? GetModelMatrix();

            //ModelViewProjection Matrix
            Matrix4 modelViewMatrix = viewMatrix * modelMatrix;

            Vector3[] viewspaceVerts = (Vector3[]) verts.Clone();

            for(int i = 0; i < viewspaceVerts.Length; ++i)
            {
                viewspaceVerts[i] = (Vector3)viewspaceVerts[i].Clone() * modelViewMatrix;
            }

            //Draw lines between the corners
            for (byte i = 0; i < 4; ++i)
            {
                mgr.Draw3DLine(viewspaceVerts[i], viewspaceVerts[i < 3 ? i + 1 : 0], projectionMatrix, frustum, fl);
            }

            return new RenderObject[0];
        }

        public override Vector3[] GetVertices()
        {
            Vector3[] vertices = new Vector3[4];
            for(int i = 0; i < verts.Length; ++i)
            {
                vertices[i] = verts[i] * GetModelMatrix();
            }
            return vertices;
        }
    }
}
