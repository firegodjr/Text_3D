using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Text_3D_Renderer.Movement;

namespace Text_3D_Renderer.Rendering
{

    public class Camera : WorldObject
    {
        private Matrix transformMatrix = new Matrix();
        private Matrix scaleMatrix = new Matrix();
        private Matrix rotationMatrix = new Matrix();

        private double pitch;
        private double yaw;
        private double roll;

        private Matrix worldMatrix = new Matrix();

        string name;

        public double Pitch
        {
            get
            {
                return pitch;
            }

            set
            {
                pitch = value;
            }
        }

        public double Yaw
        {
            get
            {
                return yaw;
            }

            set
            {
                yaw = value;
            }
        }

        public double Roll
        {
            get
            {
                return roll;
            }

            set
            {
                roll = value;
            }
        }

        public Camera(string name = "camera") : this(new Matrix(), new Matrix(), new Matrix(), name) { }

        public Camera(Matrix transformMatrix, Matrix scaleMatrix, Matrix rotationMatrix, string name = "camera")
        {
            this.name = name;
            this.transformMatrix = (Matrix)transformMatrix.Clone();
            this.scaleMatrix = (Matrix)scaleMatrix.Clone();
            //TODO: add rotation matrix init
        }

        public Camera(Vector3 position, Vector3 scale, Vector3 upvector)
        {

        }

        public Matrix getViewMatrix()
        {
            return (transformMatrix * rotationMatrix * scaleMatrix).inverse();
        }

        public Vector3 getLookDirection()
        {
            return (new Vector3(0, 0, 1)) * rotationMatrix * scaleMatrix;
        }

        public Vector3 getCameraspaceTransform(Vector3 worldspaceTransformMatrix)
        {
            Vector3 output = (Vector3)worldspaceTransformMatrix.Clone();

            output.X = worldspaceTransformMatrix.X - this.transformMatrix.X;
            output.Y = worldspaceTransformMatrix.Y - this.transformMatrix.Y;
            output.Z = worldspaceTransformMatrix.Z - this.transformMatrix.Z;

            return output;
        }

        public Vector3 restoreWorldspaceTransform(Vector3 cameraspaceTransformMatrix)
        {
            Vector3 output = (Vector3)cameraspaceTransformMatrix.Clone();

            output.X = cameraspaceTransformMatrix.X + this.transformMatrix.X;
            output.Y = cameraspaceTransformMatrix.Y + this.transformMatrix.Y;
            output.Z = cameraspaceTransformMatrix.Z + this.transformMatrix.Z;

            return output;
        }

        public Vector3 getPosition()
        {
            return new Vector3(transformMatrix.X, transformMatrix.Y, transformMatrix.Z);
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

        public Matrix getModelMatrix()
        {
            return transformMatrix * rotationMatrix * scaleMatrix;
        }

        public WorldObject transform(Vector3 v)
        {
            transformMatrix *= MatrixMaker.GetTranslationMatrix(v.X, v.Y, v.Z);
            return this;
        }

        public WorldObject transform(Matrix m)
        {
            transformMatrix *= m;
            return this;
        }

        public WorldObject scale(Vector3 v)
        {
            scaleMatrix *= MatrixMaker.GetScaleMatrix(v.X, v.Y, v.Z);
            return this;
        }

        public WorldObject scale(Matrix m)
        {
            scaleMatrix *= m;
            return this;
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

        void WorldObject.Render(ScreenMgr mgr, Matrix viewMatrix, Matrix projectionMatrix, Camera camera, Matrix parentMatrix)
        {
            throw new NotSupportedException();
        }

        string WorldObject.getName()
        {
            return "camera";
        }

        void WorldObject.setName(string name)
        {
            throw new NotSupportedException();
        }

        void WorldObject.useParentOrigin()
        {
            throw new NotSupportedException();
        }

        Matrix WorldObject.getModelMatrix(Matrix transformMatrix, Matrix rotationMatrix, Matrix scaleMatrix)
        {
            throw new NotSupportedException();
        }

        object ICloneable.Clone()
        {
            throw new NotImplementedException();
        }
    }

}
