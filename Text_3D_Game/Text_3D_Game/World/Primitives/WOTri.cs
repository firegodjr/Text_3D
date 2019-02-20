using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Text_3D_Renderer.World;
using Text_3D_Renderer.Rendering;

namespace Text_3D_Renderer.World
{
    class Tri : WorldObject
    {
        Vector3[] verts = new Vector3[3];

        public Tri(string name) : this(name, new Vector3())
        {

        }

        public Tri(Vector3 vert1, Vector3 vert2, Vector3 vert3)
        {
            verts[0] = vert1;
            verts[1] = vert2;
            verts[2] = vert3;
        }

        public Tri(string name, Vector3 transform)
        {
            this.name = name;
            TranslationMatrix.X = transform.X;
            TranslationMatrix.Y = transform.Y;
            TranslationMatrix.Z = transform.Z;
            verts[0] = new Vector3(0, 1, 0);
            verts[1] = new Vector3(0.5 * Math.Sqrt(3), -0.5, 0);
            verts[2] = new Vector3(-0.5 * Math.Sqrt(3), -0.5, 0);
        }

        public override object Clone()
        {
            Tri clone = new Tri(this.name);
            clone.SetPosition(position);
            clone.SetRotation(rotation);
            clone.SetScale(scale);

            for (byte i = 0; i < verts.Length; ++i)
            {
                clone.verts[i] = (Vector3)verts[i].Clone();
            }

            return clone;
        }

        public override void UseParentOrigin()
        {
            foreach (Vector3 v in verts)
            {
                Matrix4.MultiplyVector(v, TranslationMatrix);
            }
            position = new Vector3();

            base.UseParentOrigin();
        }

        public override RenderObject[] Render(ScreenMgr mgr, Matrix4 viewMatrix, Matrix4 projectionMatrix, Frustum frustum, Matrix4 modelMatrix = null, FrameLayer fb = null)
        {
            return new RenderObject[0];
        }

        public void setName(string name)
        {
            this.name = name;
        }
    }
}
