using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Text_3D_Renderer.Rendering;

namespace Text_3D_Renderer.World
{
    /// <summary>
    /// A worldobject made up of multiple worldobjects
    /// </summary>
    public class PolyObject : WorldObject
    {
        protected List<WorldObject> children = new List<WorldObject>();

        public PolyObject()
        {

        }

        public PolyObject(string name, List<WorldObject> objects)
        {
            this.name = name;
            foreach(WorldObject obj in objects)
            {
                obj.UseParentOrigin();
                children.Add(obj);
            }
        }

        public PolyObject(string name, params WorldObject[] objects)
        {
            this.name = name;
            foreach (WorldObject obj in objects)
            {
                obj.UseParentOrigin();
                children.Add(obj);
            }
        }

        public override void UseParentOrigin()
        {
            foreach(WorldObject obj in children)
            {
                obj.Translate(position);
                obj.Rotate(rotation);
                obj.Scale(scale);
                obj.UseParentOrigin();
            }
            base.UseParentOrigin();
        }

        public override RenderObject[] Render(ScreenMgr mgr, Matrix4 viewMatrix, Matrix4 projectionMatrix, Frustum frustum, Matrix4 parentMatrix = null, FrameLayer fl = null)
        {
            List<RenderObject> layers = new List<RenderObject>();
            List<WorldObject> sortChildren = new List<WorldObject>(children);
            sortChildren.OrderBy(obj => obj.GetVectorPosition().Mag).Reverse().ToList();
            foreach(WorldObject obj in sortChildren)
            {
                //TODO: There's probably a better way to do this
                WorldObject clone = ((WorldObject)obj.Clone());
                Vector3 oldpos = clone.GetPosition();
                Vector3 oldRotation = clone.GetRotation();
                clone.Translate(GetVectorPosition()).SetRotation(GetRotation());
                clone.Translate(GetForwardDirection() * oldpos.Z);
                clone.Translate(GetRightDirection() * oldpos.X);
                clone.Translate(GetUpDirection() * oldpos.Y);
                clone.Rotate(oldRotation);
                clone.SetColor(obj.Color);

                clone.Render(mgr, viewMatrix, projectionMatrix, frustum, GetModelMatrix(), fl);
            }

            return layers.ToArray();
        }

        /// <summary>
        /// Clones this object
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            PolyObject clone = new PolyObject(this.name);
            clone.SetPosition((Vector3)position.Clone());
            clone.SetRotation((Vector3)rotation.Clone());
            clone.SetScale((Vector3)scale.Clone());

            for (byte i = 0; i < children.Count; ++i)
            {
                clone.children[i] = (WorldObject)children[i].Clone();
            }

            return clone;
        }
    }
}
