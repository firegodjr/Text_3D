using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Text_3D_Renderer.Movement;

namespace Text_3D_Renderer.Rendering
{
    class Tri : WorldObject
    {
        string name = "";
        Vector3[] verts = new Vector3[3];
        Matrix transformMatrix = new Matrix();
        Matrix rotationMatrix = new Matrix();
        Matrix scaleMatrix = new Matrix();

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
            transformMatrix.X = transform.X;
            transformMatrix.Y = transform.Y;
            transformMatrix.Z = transform.Z;
            verts[0] = new Vector3(0, 1, 0);
            verts[1] = new Vector3(0.5 * Math.Sqrt(3), -0.5, 0);
            verts[2] = new Vector3(-0.5 * Math.Sqrt(3), -0.5, 0);
        }

        public object Clone()
        {
            Tri clone = new Tri(this.name);
            clone.setTransform((Matrix)transformMatrix.Clone());
            clone.setRotation((Matrix)rotationMatrix.Clone());
            clone.setScale((Matrix)scaleMatrix.Clone());

            for (byte i = 0; i < verts.Length; ++i)
            {
                clone.verts[i] = (Vector3)verts[i].Clone();
            }

            return clone;
        }

        public void useParentOrigin()
        {
            foreach (Vector3 v in verts)
            {
                Matrix.MultiplyVector(v, transformMatrix);
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

            modelMatrix = modelMatrix ?? getModelMatrix();
            if (Math.Abs(modelMatrix.Z) > 20)
            {
                return;
            }

            //ModelViewProjection Matrix
            Matrix MVPMatrix = projectionMatrix * viewMatrix * modelMatrix; //TODO: why (sometimes) doesn't the MVPm have an element at [3, 3]?
            Vector2[] corners = new Vector2[3];

            //Multiply the corners by the MVP Matrix and find their screen position
            for (byte i = 0; i < verts.Length; ++i)
            {
                //Clone the corner to offset
                Vector3 pos = (Vector3)verts[i].Clone();

                //Multiply by MVP Matrix
                pos = Matrix.MultiplyVector(pos, MVPMatrix);

                //Calculate screen position (ints)
                pos.X = Math.Round(mgr.width * (pos.X + 1.0) / 2.0);
                pos.Y = Math.Round(mgr.height * (1.0 - ((pos.Y + 1.0) / 2.0)));

                //Add to corner array
                corners[i] = new Vector2(pos.X, pos.Y);
                mgr.addPixelToDraw(new Vector2(pos.X, pos.Y));
            }

            //Draw lines between the corners
            for (byte i = 0; i < verts.Length; ++i)
            {
                Vector2 start = corners[i];
                Vector2 target = (i + 1 >= corners.Length ? corners[0] : corners[i + 1]);

                mgr.drawLine(target, start);
            }
        }

        public void setName(string name)
        {
            this.name = name;
        }

        public WorldObject transform(Vector3 vec)
        {
            transformMatrix = transformMatrix.multiplyBy(MatrixMaker.GetTranslationMatrix(vec.X, vec.Y, vec.Z));
            return this;
        }

        public WorldObject transform(Matrix m)
        {
            transformMatrix = transformMatrix.multiplyBy(m);
            return this;
        }

        public WorldObject scale(Vector3 vec)
        {
            scaleMatrix = scaleMatrix.multiplyBy(MatrixMaker.GetScaleMatrix(vec.X, vec.Y, vec.Z));
            return this;
        }

        public WorldObject scale(Matrix m)
        {
            scaleMatrix = scaleMatrix.multiplyBy(m);
            return this;
        }

        public WorldObject rotate(double radians, int axis)
        {
            rotationMatrix = rotationMatrix.multiplyBy(MatrixMaker.GetRotationMatrix(radians, axis));
            return this;
        }

        public WorldObject rotate(Matrix m)
        {
            rotationMatrix = rotationMatrix.multiplyBy(m);
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
