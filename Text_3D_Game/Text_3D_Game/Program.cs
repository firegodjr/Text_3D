using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Text_3D_Renderer.Movement;
using Text_3D_Renderer.Rendering;

namespace Text_3D_Renderer
{
    /*

        Author: Ethan Schrunk
        Description: A 3d renderer built from the ground up to render within a console viewspace, based on OpenGL conventions
        Start Date: 8/14/17

    */

    public static class Renderer
    {
        public static Matrix worldTransformMatrix = new Matrix();
        public static Matrix viewMatrix = new Matrix();
        public static Matrix projectionMatrix = new Matrix();

        public static Scene scene = new Scene();
        public static Camera camera = new Camera();
        public static ScreenMgr screenMgr = new ScreenMgr(80, 45);

        public static ProjectionMatrix projection = new ProjectionMatrix();

        public static int frame = 0;
        public static double lastframetime;
        public static double timeelapsed;
        public static double deltatime;

        public static List<double> last20fps = new List<double>();
        public static int avgfps;

        public static bool endGame = false;

        static void Main(string[] args)
        {
            Setup();
            while(!endGame)
            {
                Console.Clear();
                Console.WriteLine(Draw());
            }
        }

        public static void Setup(float fov = 1.3f, float aspect = 1.3f, float neardist = 0.5f, float fardist = 100, bool lefthanded = true)
        {
            viewMatrix.newFromRowwise(MatrixInverter.MatrixInverse(worldTransformMatrix.RowwiseArray));

            projection.Fov = fov;
            projection.Aspect = aspect;
            projection.NearDist = neardist;
            projection.FarDist = fardist;
            projection.LeftHanded = lefthanded;

            projectionMatrix = projection.computeMatrix();

            scene.Camera.transform(new Vector3(0, 0, -5));
            //scene.Camera.rotate(Math.PI / 32, Rotation.Y);
            scene.RegisterWorldObject(new Cube("test0"));
            scene.RegisterWorldObject(new Cube("test1").scale(new Vector3(1.1, 1.1, 1.1)));
            scene.RegisterWorldObject(new Tri("It thinks of sin"));
        }

        public static string Draw()
        {
            double frametime = DateTime.Now.Millisecond;
            ++frame;

            screenMgr.addTextToDraw(new Vector2(screenMgr.width/2 - 13, screenMgr.height/2), "[Thinking Kahoot Thoughts]");
            screenMgr.addTextToDraw(new Vector2(0, 0), "fps: [" + avgfps + "]");
            
            scene.Camera.setRotation(MatrixMaker.GetRotationMatrix(Math.Sin(timeelapsed)/2, Rotation.Z));
            scene.GetWorldObject("test0").setRotation(MatrixMaker.GetRotationMatrix(Math.Cos(timeelapsed), Rotation.Y));
            scene.GetWorldObject("test1").setRotation(MatrixMaker.GetRotationMatrix(Math.Cos(timeelapsed), Rotation.Y));
            scene.GetWorldObject("It thinks of sin").rotate(deltatime*2, Rotation.Y);

            deltatime = (frametime >= lastframetime ? frametime - lastframetime : frametime + 1000 - lastframetime) / 1000;
            timeelapsed += deltatime;
            lastframetime = frametime;

            //Get the average fps
            if(frame % 20 == 0)
            {
                avgfps = (int)last20fps.Average();
                last20fps.Clear();
            }
            last20fps.Add(Math.Round(1000 / (deltatime * 1000)));
            
            //Render everything in the scene
            return scene.Render(screenMgr, scene.Camera.getViewMatrix(), projectionMatrix);
        }
    }
}
