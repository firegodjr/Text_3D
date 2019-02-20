using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Text_3D_Engine;
using Text_3D_Engine.Input;
using Text_3D_Renderer;

namespace Text_3D_Engine
{

    /// <summary>
    /// A class that handles all abstract things that the game engine needs, such as viewports, input handlers, and menus
    /// </summary>
    public class Game
    {
        Dictionary<string, Viewport> viewportRegistry = new Dictionary<string, Viewport>();
        Dictionary<string, InputHandler> inputHandlerRegistry = new Dictionary<string, InputHandler>();
        Dictionary<string, TextMenu> menuRegistry = new Dictionary<string, TextMenu>();

        public Dictionary<string, Viewport> ViewportRegistry
        {
            get
            {
                return viewportRegistry;
            }
        }

        public Dictionary<string, InputHandler> InputHandlerRegistry
        {
            get
            {
                return inputHandlerRegistry;
            }
        }

        //TODO: AUTOMATE MENU INPUT AND DISPLAY HANDLING THROUGH Game CLASS

        public Dictionary<string, TextMenu> MenuRegistry
        {
            get
            {
                return menuRegistry;
            }
        }

        public Game() { }

        /// <summary>
        /// Stops render threads and makes sure everything is cleaned up
        /// </summary>
        public void Shutdown()
        {
            foreach(Viewport v in viewportRegistry.Values)
            {
                v.StopRendering();
            }
        }

        public bool RegisterViewport(string key, Viewport viewport)
        {
            if (!viewportRegistry.ContainsKey(key))
            {
                viewportRegistry.Add(key, viewport);
                return true;
            }
            else return false;
        }
        
        public bool RegisterInputHandler(string key, InputHandler handler)
        {
            if (!inputHandlerRegistry.ContainsKey(key))
            {
                inputHandlerRegistry.Add(key, handler);
                return true;
            }
            else return false;
        }

        public bool RegisterTextMenu(string key, TextMenu menu)
        {
            if (!menuRegistry.ContainsKey(key))
            {
                menuRegistry.Add(key, menu);
                return true;
            }
            else return false;
        }
    }
}
