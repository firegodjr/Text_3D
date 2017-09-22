using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Text_3D_Renderer.Movement;

namespace Text_3D_Renderer.Rendering
{
    public class Quad : WorldObject
    {
        Matrix transformMatrix = new Matrix();
        Matrix rotationMatrix = new Matrix();
        Matrix scaleMatrix = new Matrix();
        Vector3[] verts = new Vector3[4];
        string name = "";

        public Quad(string name) : this(name, new Vector3())
        {

        }

        public Quad(string name, Vector3 transform)
        {
            this.name = name.ToLower();
            verts[0] = new Vector3(-1, -1, 0);
            verts[1] = new Vector3(-1, 1, 0);
            verts[2] = new Vector3(1, 1, 0);
            verts[3] = new Vector3(1, -1, 0);
            transformMatrix.X = transform.X;
            transformMatrix.Y = transform.Y;
            transformMatrix.Z = transform.Z;
        }

        public Quad(string name, Vector3 vec1, Vector3 vec2, Vector3 vec3, Vector3 vec4)
        {
            this.name = name.ToLower();
            verts[0] = (Vector3)vec1.Clone();
            verts[1] = (Vector3)vec2.Clone();
            verts[2] = (Vector3)vec3.Clone();
            verts[3] = (Vector3)vec4.Clone();
        }

        public Quad(string name, Vector3[] vecs) : this(name, vecs[0], vecs[1], vecs[2], vecs[3]) { }

        public object Clone()
        {
            Quad clone = new Quad(this.name);
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
            for(byte i = 0; i < verts.Length; ++i)
            {
                verts[i] = verts[i] * getModelMatrix();
            }
            transformMatrix = new Matrix();
            rotationMatrix = new Matrix();
            scaleMatrix = new Matrix();
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

        public string getName()
        {
            return name;
        }

        public void setName(string name)
        {
            this.name = name;
        }

        public void Render(ScreenMgr mgr, Matrix viewMatrix, Matrix projectionMatrix, Camera camera, Matrix modelMatrix = null)
        {
            modelMatrix = modelMatrix ?? getModelMatrix(); 
            if (Math.Abs(modelMatrix.Z) > 20)
            {
                return;
            }

            //ModelViewProjection Matrix
            Matrix MVPMatrix = projectionMatrix * viewMatrix * modelMatrix;
            Vector2[] corners = new Vector2[4];

            //Multiply the corners by the MVP Matrix and find their screen position
            for (byte i = 0; i < 4; ++i)
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
            for(byte i = 0; i < 4; ++i)
            {
                Vector2 start = corners[i];
                Vector2 target = (i + 1 >= corners.Length ? corners[0] : corners[i + 1]);

                mgr.drawLine(target, start);
            }
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
