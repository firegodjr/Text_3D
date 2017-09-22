using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_3D_Renderer.Rendering.Primitives
{
    class Model : WorldObject
    {
        Matrix transformMatrix = new Matrix();
        Matrix rotationMatrix = new Matrix();
        Matrix scaleMatrix = new Matrix();
        List<Tri> triangles = new List<Tri>();
        string name = "";

        public string getName()
        {
            return name;
        }

        public void useParentOrigin()
        {
            foreach(Tri t in triangles)
            {
                t.transform(transformMatrix);
            }
            transformMatrix = new Matrix();
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

        public object getTransformed(Vector3 v)
        {
            return ((Matrix)transformMatrix.Clone()).transformBy(v);
        }

        public object getTransformed(Matrix m)
        {
            return ((Matrix)transformMatrix.Clone()).multiplyBy(m);
        }

        public void Render(ScreenMgr mgr, Matrix viewMatrix, Matrix projectionMatrix, Camera camera, Matrix modelMatrix = null)
        {
            throw new NotImplementedException();
        }

        public WorldObject rotate(double radians, int axis)
        {
            throw new NotImplementedException();
        }

        public WorldObject rotate(Matrix m)
        {
            throw new NotImplementedException();
        }

        public WorldObject scale(Vector3 v)
        {
            throw new NotImplementedException();
        }

        public WorldObject scale(Matrix m)
        {
            throw new NotImplementedException();
        }

        public void setName(string name)
        {
            throw new NotImplementedException();
        }

        public WorldObject transform(Vector3 v)
        {
            throw new NotImplementedException();
        }

        public WorldObject transform(Matrix m)
        {
            throw new NotImplementedException();
        }

        public Matrix getModelMatrix(Matrix transformMatrix = null, Matrix rotationMatrix = null, Matrix scaleMatrix = null)
        {
            transformMatrix = transformMatrix ?? this.transformMatrix;
            rotationMatrix = rotationMatrix ?? this.rotationMatrix;
            scaleMatrix = scaleMatrix ?? this.scaleMatrix;
            
            return transformMatrix * rotationMatrix * scaleMatrix;
        }

        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
