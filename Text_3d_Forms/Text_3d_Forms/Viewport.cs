using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Text_3D_Renderer;
using Text_3D_Renderer.World;

namespace Text_3D_Engine
{
    public delegate void OnFrameDisplayedAction(double deltaTime);

    public class Viewport : Renderer
    {
        public bool ShowFPS;

        public Viewport(int width, int height, float fov = 1.0f, float aspect = 1.3f, float neardist = 1, float fardist = 100, bool lefthanded = false, bool showFPS = false) : base(width, height, fov, aspect, neardist, fardist, lefthanded) { ShowFPS = showFPS; }

        /// <summary>
        /// Updates everything in the frame before the viewport renders the scene
        /// </summary>
        public override void Update()
        {
            if(ShowFPS)
            {
                DisplayFPS();
            }
        }

        private void DisplayFPS()
        {
            //Get the average fps
            if (frame % 20 == 0 && last20fps.Count > 0)
            {
                avgfps = (int)last20fps.Average();
                last20fps.Clear();
            }
            last20fps.Add(Math.Round(1000 / (Deltatime)));
            screenMgr.AddTextToDraw(new Coords(), "fps: [" + avgfps + "]");
        }
    }
}
