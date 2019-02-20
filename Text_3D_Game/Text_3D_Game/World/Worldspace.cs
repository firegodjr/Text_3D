using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Text_3D_Renderer.Rendering;
using System.Diagnostics;

namespace Text_3D_Renderer.World
{
    public class Worldspace
    {
        private List<WorldObject> worldObjects = new List<WorldObject>();

        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private Camera camera = new Camera();

        public Camera Camera
        {
            get
            {
                return camera;
            }
        }

        /// <summary>
        /// Gets a list of all world objects
        /// </summary>
        /// <returns></returns>
        public List<WorldObject> GetWorldObjects()
        {
            return worldObjects;
        }

        /// <summary>
        /// Adds a new world object to the worldspace
        /// </summary>
        /// <param name="obj"></param>
        public void RegisterWorldObject(WorldObject obj)
        {
            obj.SetContainingWorld(this);
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
                if(worldObjects[i].Name == name)
                {
                    return worldObjects[i];
                }
            }

            return new WOQuad("null");
        }

        public void SetWorldObject(string name, WorldObject wobject)
        {
            for(int i = 0; i < worldObjects.Count; ++i)
            {
                if(worldObjects[i].Name == name)
                {
                    worldObjects[i] = wobject;
                    return;
                }
            }

            wobject.Name = name;
            RegisterWorldObject(wobject);
        }

        public void RemoveWorldObject(string name)
        {
            for (int i = 0; i < worldObjects.Count; ++i)
            {
                if (worldObjects[i].Name == name)
                {
                    worldObjects[i].Dispose();
                    worldObjects.RemoveAt(i);
                    --i;
                }
            }
        }
    }
}
