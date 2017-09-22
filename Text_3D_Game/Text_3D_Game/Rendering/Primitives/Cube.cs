using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Text_3D_Renderer.Movement;

namespace Text_3D_Renderer.Rendering
{
    public class Cube : WorldObject
    {
        Matrix transformMatrix = new Matrix();
        Matrix rotationMatrix = new Matrix();
        Matrix scaleMatrix = new Matrix();
        WorldObject[] quads = new WorldObject[]
        {
            new Quad("test1", new Vector3(0, 0, 1)),
            new Quad("test1", new Vector3(0, 0, -1)),
            new Quad("test1", new Vector3(1, 0, 0)).rotate(Math.PI/2, Rotation.Y),
            new Quad("test1", new Vector3(-1, 0, 0)).rotate(Math.PI/2, Rotation.Y),
            new Quad("test1", new Vector3(0, 1, 0)).rotate(Math.PI/2, Rotation.X),
            new Quad("test1", new Vector3(0, -1, 0)).rotate(Math.PI/2, Rotation.X)
        };

        public object Clone()
        {
            Cube clone = new Cube(this.name);
            clone.setTransform((Matrix)transformMatrix.Clone());
            clone.setRotation((Matrix)rotationMatrix.Clone());
            clone.setScale((Matrix)scaleMatrix.Clone());
            for(byte i = 0; i < quads.Length; ++i)
            {
                clone.quads[i] = (Quad)quads[i].Clone();
            }

            return clone;
        }

        string name = "";

        public Cube(string name)
        {
            this.name = name;

            foreach(Quad q in quads)
            {
                q.useParentOrigin();
            }
        }

        public void useParentOrigin()
        {
            foreach(Quad q in quads)
            {
                q.transform(transformMatrix);
            }
            transformMatrix = new Matrix();
        }

        public string getName()
        {
            return name;
        }

        public Matrix getTransform()
        {
            return transformMatrix;
        }

        public Matrix getRotation()
        {
            return rotationMatrix;
        }

        public Matrix getScale()
        {
            return scaleMatrix;
        }

        public void setTransform(Matrix m)
        {
            transformMatrix = (Matrix)m.Clone();
        }

        public void setRotation(Matrix m)
        {
            rotationMatrix = (Matrix)m.Clone();
        }

        public void setScale(Matrix m)
        {
            scaleMatrix = (Matrix)m.Clone();
        }

        public void Render(ScreenMgr mgr, Matrix viewMatrix, Matrix projectionMatrix, Camera camera, Matrix modelMatrix = null)
        {
            foreach (WorldObject w in quads)
            {
                w.Render(mgr, viewMatrix, projectionMatrix, camera, getModelMatrix());
            }
        }

        public WorldObject rotate(double radians, int axis)
        {
            rotationMatrix *= MatrixMaker.GetRotationMatrix(radians, axis);
            return this;
        }

        public WorldObject rotate(Matrix m)
        {
            rotationMatrix *= m;
            return this;
        }

        public WorldObject scale(Vector3 v)
        {
            scaleMatrix = scaleMatrix * MatrixMaker.GetScaleMatrix(v.X, v.Y, v.Z);
            return this;
        }

        public WorldObject scale(Matrix m)
        {
            scaleMatrix = scaleMatrix * m;
            return this;
        }

        public void setName(string name)
        {
            this.name = name;
        }

        public WorldObject transform(Vector3 v)
        {
            transformMatrix.transformBy(v);
            return this;
        }

        public WorldObject transform(Matrix m)
        {
            transformMatrix = transformMatrix * m;
            return this;
        }

        public Matrix getModelMatrix(Matrix transformMatrix = null, Matrix rotationMatrix = null, Matrix scaleMatrix = null)
        {
            transformMatrix = transformMatrix ?? this.transformMatrix;
            rotationMatrix = rotationMatrix ?? this.rotationMatrix;
            scaleMatrix = scaleMatrix ?? this.scaleMatrix;

            return transformMatrix * rotationMatrix * scaleMatrix;
        }
    }
}
