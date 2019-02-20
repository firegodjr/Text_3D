using System;
using Text_3D_Renderer.Rendering;

namespace Text_3D_Renderer.World
{
    /// <summary>
    /// An object that can exist within a world
    /// </summary>
    public class WorldObject : ICloneable
    {
        #region Variables
        /// <summary>
        /// The vec3 representing this object's translation
        /// </summary>
        protected Vector3 position = new Vector3();

        /// <summary>
        /// The matrix representing this object's rotation in euler angles
        /// </summary>
        protected Vector3 rotation = new Vector3();

        /// <summary>
        /// The vec3 representing this object's scale
        /// </summary>
        protected Vector3 scale = new Vector3(1, 1, 1);

        protected Matrix4 tempTranslation = new Matrix4();
        
        protected Matrix4 tempRotation = new Matrix4();
        
        protected Matrix4 tempScale = new Matrix4();

        protected Matrix4 modelMatrix = new Matrix4();
        bool modelMatrixNeedsRecalc = true;

        /// <summary>
        /// The text id of this WorldObject
        /// </summary>
        protected string name;

        protected bool disposed = false;

        public bool Disposed
        {
            get { return disposed; }
        }

        private Worldspace worldspace;

        /// <summary>
        /// A ref to the worldspace that this object is in
        /// </summary>
        public Worldspace World
        {
            get { return worldspace; }
        }

        /// <summary>
        /// The color layer that this object will be drawn to, white by default
        /// </summary>
        protected Layer color = Layer.WHITE;

        /// <summary>
        /// Whether or not this object is locked to the camera, rather than to world coordinates
        /// </summary>
        protected bool relativeToView = false;

        /// <summary>
        /// Gets or sets if the object is locked to the camera, rather than to world coordinates
        /// </summary>
        public bool RelativeToView
        {
            get { return relativeToView; }
            set { relativeToView = value; }
        }

        /// <summary>
        /// Gets or sets the color of the object
        /// </summary>
        public Layer Color
        {
            get { return color; }
            set { color = value; }
        }

        /// <summary>
        /// Gets or sets the name of the object
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        protected Matrix4 ScaleMatrix
        {
            get
            {
                return MatrixMaker.GetScaleMatrix(scale.X, scale.Y, scale.Z);
            }
        }

        protected Matrix4 TranslationMatrix
        {
            get
            {
                return MatrixMaker.GetTranslationMatrix(position);
            }
        }

        protected Matrix4 RotationMatrix
        {
            get
            {
                return MatrixMaker.GetRotationMatrix(rotation);
            }
        }

        #endregion

        string[][] strings = new string[5][];

        /// <summary>
        /// Create a new empty world object
        /// </summary>
        public WorldObject() : this("newworldobject", new Vector3(), new Vector3(), new Vector3(1, 1, 1))
        { }

        public WorldObject(string name, Model model) : this(name, new Vector3(), new Vector3(), new Vector3(1, 1, 1))
        { }

        public WorldObject(string name, Vector3 position) : this(name, position, new Vector3(), new Vector3(1, 1, 1))
        { }

        public WorldObject(string name, Vector3 position, Vector3 rotation) : this(name, position, rotation, new Vector3(1, 1, 1))
        { }

        /// <summary>
        /// Create a new world object with given name and matrices
        /// </summary>
        /// <param name="name"></param>
        /// <param name="translationMatrix"></param>
        /// <param name="rotationMatrix"></param>
        /// <param name="scaleMatrix"></param>
        public WorldObject(string name, Vector3 position, Vector3 rotation, Vector3 scale)
        {
            this.name = name;
            this.position = position;
            this.rotation = rotation;
            this.scale = scale;
        }

        /// <summary>
        /// Registers pixels that should be drawn to one or more FrameLayers, then returns them as an array of RenderObjects.
        /// </summary>
        /// <param name="mgr">The ScreenMgr that will handle the drawing</param>
        /// <param name="viewMatrix">The View Matrix</param>
        /// <param name="projectionMatrix">The Projection Matrix</param>
        /// <param name="camera">The camera</param>
        /// <param name="frustum">The view frustum, passed here to allow for sub-object culling</param>
        /// <param name="parentMatrix">A model matrix to use instead of this object's model matrix</param>
        /// <param name="fl">A FrameLayer to render the pixels to</param>
        public virtual RenderObject[] Render(ScreenMgr mgr, Matrix4 viewMatrix, Matrix4 projectionMatrix, Frustum frustum, Matrix4 parentMatrix = null, FrameLayer fl = null)
        {
            Console.WriteLine("Object '{0}' does not have an overridden render method, so nothing was rendered.", name);
            return new RenderObject[0];
        }

        /// <summary>
        /// Returns the vertices that this object contains.
        /// </summary>
        /// <returns></returns>
        public virtual Vector3[] GetVertices()
        {
            return new Vector3[0];
        }

        /// <summary>
        /// Used to offset all points in the WorldObject using its origin position, then set its origin to 0,0,0.
        /// </summary>
        public virtual void UseParentOrigin()
        {
            //tempTranslation = (Matrix)TranslationMatrix.Clone();
            //TranslationMatrix = new Matrix();
            //tempRotation = (Matrix)RotationMatrix.Clone();
            //RotationMatrix = new Matrix();
            //tempScale = (Matrix)ScaleMatrix.Clone();
            //ScaleMatrix = new Matrix();
        }

        /// <summary>
        /// Reverts the object's origin and rotation
        /// </summary>
        public virtual void RevertParentOrigin()
        {
            //TranslationMatrix = (Matrix)tempTranslation.Clone();
            //tempTranslation = new Matrix();
            //RotationMatrix = (Matrix)tempRotation.Clone();
            //tempRotation = new Matrix();
            //ScaleMatrix = (Matrix)tempScale.Clone();
            //tempScale = new Matrix();
        }

        /// <summary>
        /// Gets the Vector3 that represents the position of the object in worldspace
        /// </summary>
        /// <returns></returns>
        public Vector3 GetVectorPosition()
        {
            return TranslationMatrix.ToVector();
        }

        /// <summary>
        /// Gets the translation matrix from this object
        /// </summary>
        /// <returns></returns>
        public Vector3 GetPosition()
        {
            return position;
        }

        /// <summary>
        /// Gets the rotation matrix from this object
        /// </summary>
        /// <returns></returns>
        public Vector3 GetRotation()
        {
            return rotation;
        }

        /// <summary>
        /// Gets the scale matrix from this object
        /// </summary>
        /// <returns></returns>
        public Vector3 GetScale()
        {
            return scale;
        }
        
        /// <summary>
        /// Sets the translation of the object to the given vector3
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public WorldObject SetPosition(Vector3 vec)
        {
            return SetPosition(vec.X, vec.Y, vec.Z);
        }

        public WorldObject SetPosition(double x, double y, double z)
        {
            position.X = x;
            position.Y = y;
            position.Z = z;
            modelMatrixNeedsRecalc = true;
            return this;
        }

        public WorldObject SetRotation(Vector3 vec)
        {
            return SetRotation(vec.X, vec.Y, vec.Z);
        }

        public WorldObject SetRotation(double x, double y, double z)
        {
            rotation.X = x;
            rotation.Y = y;
            rotation.Z = z;
            modelMatrixNeedsRecalc = true;
            return this;
        }

        /// <summary>
        /// Sets the scale of the object
        /// </summary>
        /// <param name="d">The scale percentage</param>
        /// <returns></returns>
        public WorldObject SetScale(double d)
        {
            scale.XYZ = d;
            modelMatrixNeedsRecalc = true;
            return this;
        }

        public WorldObject SetScale(double x, double y, double z)
        {
            scale.X = x;
            scale.Y = y;
            scale.Z = z;
            modelMatrixNeedsRecalc = true;
            return this;
        }

        public WorldObject SetScale(Vector3 vec)
        {
            return SetScale(vec.X, vec.Y, vec.Z);
        }

        /// <summary>
        /// Gets this object's forward-pointing normal
        /// </summary>
        /// <returns></returns>
        public Vector3 GetForwardDirection()
        {
            return ((new Vector3(0, 0, 1)) * RotationMatrix * ScaleMatrix).Normalize();
        }

        /// <summary>
        /// Gets this object's right-pointing normal
        /// </summary>
        /// <returns></returns>
        public Vector3 GetRightDirection()
        {
            return ((new Vector3(1, 0, 0)) * RotationMatrix * ScaleMatrix).Normalize();
        }

        /// <summary>
        /// Gets this object's up-pointing normal
        /// </summary>
        /// <returns></returns>
        public Vector3 GetUpDirection()
        {
            return ((new Vector3(0, 1, 0)) * RotationMatrix * ScaleMatrix).Normalize();
        }

        /// <summary>
        /// Sets the color layer this object will be rendered to
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public WorldObject SetColor(Layer color)
        {
            this.color = color;
            return this;
        }

        /// <summary>
        /// Gets the combination of translation, rotation and scale that represent the transform of this WorldObject
        /// </summary>
        /// <param name="transformMatrix"></param>
        /// <param name="rotationMatrix"></param>
        /// <param name="scaleMatrix"></param>
        /// <returns></returns>
        public Matrix4 GetModelMatrix(Matrix4 transformMatrix = null, Matrix4 rotationMatrix = null, Matrix4 scaleMatrix = null)
        {
            if (modelMatrixNeedsRecalc)
            {
                RecalcModelMatrix();
                modelMatrixNeedsRecalc = false;
            }

            return modelMatrix;
        }

        public void RecalcModelMatrix()
        {
            modelMatrixNeedsRecalc = false;
            modelMatrix = TranslationMatrix * RotationMatrix * ScaleMatrix;
        }

        /// <summary>
        /// Translates the object in worldspace using a vector
        /// </summary>
        /// <param name="vec"></param>
        /// <returns></returns>
        public WorldObject Translate(Vector3 vec)
        {
            return Translate(vec.X, vec.Y, vec.Z);
        }

        public WorldObject Translate(double x, double y, double z)
        {
            position.X += x;
            position.Y += y;
            position.Z += z;
            return this;
        }

        /// <summary>
        /// Scales the object using a vector
        /// </summary>
        /// <param name="vec"></param>
        /// <returns></returns>
        public WorldObject Scale(Vector3 vec)
        {
            return Scale(vec.X, vec.Y, vec.Z);
        }

        public WorldObject Scale(double x, double y, double z)
        {
            scale.X *= x;
            scale.Y *= y;
            scale.Z *= z;
            return this;
        }

        public WorldObject Scale(double xyz)
        {
            return Scale(xyz, xyz, xyz);
        }

        public WorldObject Rotate(Vector3 vec)
        {
            return Rotate(vec.X, vec.Y, vec.Z);
        }

        /// <summary>
        /// Rotates the object using a vector
        /// </summary>
        /// <param name="radians"></param>
        /// <param name="axis"></param>
        /// <returns></returns>
        public WorldObject Rotate(double x, double y, double z)
        {
            rotation.X += x;
            rotation.Y += y;
            rotation.Z += z;
            modelMatrixNeedsRecalc = true;
            return this;
        }

        /// <summary>
        /// Called every frame
        /// </summary>
        /// <param name="deltatime"></param>
        public virtual void Update(double deltatime)
        {

        }

        /// <summary>
        /// SHOULD NOT BE USED OUTSIDE OF WORLDSPACE OBJECT
        /// </summary>
        public void SetContainingWorld(Worldspace world)
        {
            this.worldspace = world;
        }

        /// <summary>
        /// Clones the object
        /// </summary>
        /// <returns></returns>
        public virtual object Clone()
        {
            WorldObject clone = new WorldObject();
            clone.position = position;
            clone.rotation = rotation;
            clone.scale = scale;
            clone.name = name;
            clone.color = color;
            clone.worldspace = worldspace;
            clone.relativeToView = relativeToView;
            return clone;
        }

        /// <summary>
        /// Dispose() runs immediately before the object is removed from worldspace
        /// </summary>
        public virtual void Dispose()
        {
            disposed = true;
        }

        public virtual void UnDispose()
        {
            disposed = false;
        }
    }
}
