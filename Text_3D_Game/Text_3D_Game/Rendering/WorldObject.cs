using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_3D_Renderer.Rendering
{
    public interface WorldObject : ICloneable
    {
        /// <summary>
        /// Registers pixels that should be drawn to the ScreenMgr
        /// </summary>
        /// <param name="mgr"></param>
        /// <param name="viewMatrix"></param>
        /// <param name="projectionMatrix"></param>
        /// <param name="camera"></param>
        /// <param name="parentMatrix"></param>
        void Render(ScreenMgr mgr, Matrix viewMatrix, Matrix projectionMatrix, Camera camera, Matrix parentMatrix = null);
        string getName();
        void setName(string name);

        /// <summary>
        /// Offsets all points in the WorldObject using its origin position, then sets its origin to 0,0,0.
        /// </summary>
        void useParentOrigin();
        Matrix getTransform();
        Matrix getRotation();
        Matrix getScale();
        void setTransform(Matrix m);
        void setRotation(Matrix m);
        void setScale(Matrix m);

        /// <summary>
        /// Gets the combination of translation, rotation and scale that represent the transform of this WorldObject
        /// </summary>
        /// <param name="transformMatrix"></param>
        /// <param name="rotationMatrix"></param>
        /// <param name="scaleMatrix"></param>
        /// <returns></returns>
        Matrix getModelMatrix(Matrix transformMatrix = null, Matrix rotationMatrix = null, Matrix scaleMatrix = null);
        WorldObject transform(Vector3 v);
        WorldObject transform(Matrix m);
        WorldObject scale(Vector3 v);
        WorldObject scale(Matrix m);
        WorldObject rotate(double radians, int axis);
        WorldObject rotate(Matrix m);
    }
}
