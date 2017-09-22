using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Text_3D_Renderer.Movement;

namespace Text_3D_Renderer.Rendering
{
    public class Scene
    {
        private List<WorldObject> worldObjects = new List<WorldObject>();

        private Camera camera = new Camera();

        public Camera Camera
        {
            get
            {
                return camera;
            }
        }

        /// <summary>
        /// Renders every world object from the perspective of the viewport
        /// </summary>
        /// <param name="mgr"></param>
        /// <param name="viewMatrix"></param>
        /// <param name="projectionMatrix"></param>
        public string Render(ScreenMgr mgr, Matrix viewMatrix, Matrix projectionMatrix)
        {
            //Sort all world objects by camera distance
            List<WorldObject> sortedWorldObjects = new List<WorldObject>(worldObjects);
            sortedWorldObjects.OrderBy(x => GetWODistance(camera, x));

            for(int i = 0; i < worldObjects.Count; ++i)
            {
                WorldObject obj = (WorldObject)sortedWorldObjects[i].Clone();
                
                obj.Render(mgr, viewMatrix, projectionMatrix, camera);
            }

            return mgr.DrawPixels();
        }

        private double GetWODistance(WorldObject x, WorldObject y)
        {
            Matrix xpos = x.getTransform();
            Matrix ypos = y.getTransform();
            return Math.Sqrt(Math.Pow(xpos.X - ypos.X, 2) + Math.Pow(xpos.Y - ypos.Y, 2) + Math.Pow(xpos.Z - ypos.Z, 2));
        }

        /// <summary>
        /// Adds a new world object to the worldspace
        /// </summary>
        /// <param name="obj"></param>
        public void RegisterWorldObject(WorldObject obj)
        {
            worldObjects.Add(obj);
        }

        /// <summary>
        /// Returns a world object using its name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public WorldObject GetWorldObject(string name)
        {
            for(int i = 0; i < worldObjects.Count; ++i)
            {
                if(worldObjects[i].getName() == name)
                {
                    return worldObjects[i];
                }
            }

            return null;
        }

        public void SetWorldObject(string name, WorldObject wobject)
        {
            for(int i = 0; i < worldObjects.Count; ++i)
            {
                if(worldObjects[i].getName() == name)
                {
                    worldObjects[i] = wobject;
                }
            }
        }
    }
}
