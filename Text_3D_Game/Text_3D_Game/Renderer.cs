using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Text_3D_Renderer.Rendering;
using Text_3D_Renderer.World;

namespace Text_3D_Renderer
{
    /*

        Author: Ethan Schrunk
        Description: A 3d renderer built from the ground up to render within a console viewspace, based on OpenGL conventions
        Start Date: 8/14/17

    */

    /// <summary>
    /// The encapsulation of all components necessary to render a 3D scene
    /// </summary>
    public class Renderer
    {
        private Matrix4 projectionMatrix = new Matrix4();
        private Dictionary<Type, List<WOAction>> objPostActions;

        private bool debug = true;

        // I've done very little threading work, so these volatile values might be a terrible way of doing things, but it works for basic thread status updates.
        Thread renderThread;
        volatile bool threadTerminated;
        volatile bool frameReady;

        /// <summary>
        /// Denotes whether or not a frame is ready to be displayed
        /// </summary>
        public bool FrameReady
        {
            get { return frameReady; }
        }
        bool receivedFrame;

        private volatile string[] stringLayers = new string[6];
        public string[] StringLayers
        {
            get { return stringLayers; }
            set { stringLayers = value; }
        }

        /// <summary>
        /// The list of string buffers that the Draw methods draw to.
        /// </summary>
        private List<string> frametext = new List<string>();

        /// <summary>
        /// A type of function that takes a WorldObject argument
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="namesubstr"></param>
        public delegate void WOAction(WorldObject obj);

        /// <summary>
        /// Holds an action to be applied for every object of a given type.
        /// </summary>
        public Dictionary<Type, List<WOAction>> ObjPostActions
        {
            get { return objPostActions; }
        }

        /// <summary>
        /// The scene to be rendered
        /// </summary>
        public Worldspace Worldspace = new Worldspace();

        /// <summary>
        /// The ScreenMgr that represents the viewport
        /// </summary>
        public ScreenMgr screenMgr;

        /// <summary>
        /// The encapsulation of all needed projection data
        /// </summary>
        public ProjectionMgr Projection = new ProjectionMgr();

        /// <summary>
        /// Shorthand for Renderer.Scene.Camera
        /// </summary>
        public Camera Camera
        {
            get { return Worldspace.Camera; }
        }

        /// <summary>
        /// The view frustum
        /// </summary>
        public Frustum Frustum = null;

        /// <summary>
        /// The general frame counter
        /// </summary>
        public int frame = 0;

        public Stopwatch stopWatch = new Stopwatch();
        private double deltatime;

        public double Deltatime
        {
            get { return deltatime; }
        }

        /// <summary>
        /// The speed at which time is counted in the game
        /// </summary>
        public double timescale = 1;

        /// <summary>
        /// The total amount of time that has elapsed
        /// </summary>
        public double timeelapsed;

        /// <summary>
        /// The list of the last 20 frame lengths, used to get a more slowly updating fps counter
        /// </summary>
        public List<double> last20fps = new List<double>();
        /// <summary>
        /// The average fps of the last 20 frames
        /// </summary>
        public int avgfps;

        /// <summary>
        /// Inits a new renderer with default values and a resolution of 80x55
        /// </summary>
        public Renderer() : this(80, 55) { }

        /// <summary>
        /// Sets up the projection and view matrices, and initializes the PostActions dictionary
        /// </summary>
        /// <param name="fov"></param>
        /// <param name="aspect"></param>
        /// <param name="neardist"></param>
        /// <param name="fardist"></param>
        /// <param name="lefthanded"></param>
        public Renderer(int width, int height, float fov = 1.0f, float aspect = 1.3f, float neardist = 1, float fardist = 100, bool lefthanded = false)
        {
            Projection.Fov = fov;
            Projection.Aspect = aspect;
            Projection.NearDist = neardist;
            Projection.FarDist = fardist;
            Projection.LeftHanded = lefthanded;
            projectionMatrix = Projection.ComputeMatrix();

            screenMgr = new ScreenMgr(width, height);

            renderThread = new Thread(DrawFrames);

            objPostActions = new Dictionary<Type, List<WOAction>>();
        }

        private void DrawFrames()
        {
            while (!threadTerminated)
            {
                if (!frameReady)
                {
                    if (debug)
                    {
                        Update();
                        stringLayers = DrawObjects();
                    }
                    else
                    {
                        try
                        {
                            Update();
                            stringLayers = DrawObjects();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"{ex.GetBaseException()} Exception while drawing\n{ex.InnerException}");
                        }
                    }

                    frameReady = true;
                }

                if (receivedFrame)
                {
                    frameReady = false;
                    receivedFrame = false;
                }
            }
        }

        public void NoteFrameRecieved()
        {
            receivedFrame = true;
        }

        public void StopRendering()
        {
            threadTerminated = true;
        }

        public void RecalculateProjection()
        {
            projectionMatrix = Projection.ComputeMatrix();
        }

        /// <summary>
        /// Starts the built-in render thread
        /// </summary>
        public void StartRenderThread()
        {
            renderThread.Start();
        }

        /// <summary>
        /// Updates the positions and status of everything in the frame
        /// </summary>
        public virtual void Update()
        {
            //TODO: Make this into a registered viewport task instead of hardcoding it here
            screenMgr.AddTextToDraw(new Coords(69, 54), "fps: [" + avgfps + "]");
            screenMgr.AddTextToDraw(new Coords(0, 2), string.Format("Time Elapsed: {0:f2}sec", timeelapsed));

            List<WorldObject> worldObjects = new List<WorldObject>(Worldspace.GetWorldObjects());
            screenMgr.AddTextToDraw(new Coords(0, 6), string.Format("World Objects: {0}", worldObjects.Count));

            screenMgr.AddTextToDraw(new Coords(0, 40), $"NearDist: {Projection.NearDist}");

            for (int i = 0; i < worldObjects.Count; ++i)
            {
                
                if (worldObjects[i].RelativeToView)
                {
                    WorldObject clone = ((WorldObject)worldObjects[i].Clone());
                    Vector3 oldpos = clone.GetPosition();
                    Vector3 oldRotation = clone.GetRotation();
                    clone.UseParentOrigin();
                    clone.Translate(Worldspace.Camera.GetVectorPosition()).SetRotation(Worldspace.Camera.GetRotation());
                    clone.Translate(Worldspace.Camera.GetForwardDirection() * oldpos.Z + Worldspace.Camera.GetForwardDirection() * 1.1);
                    clone.Translate(Worldspace.Camera.GetRightDirection() * oldpos.X);
                    clone.Translate(Worldspace.Camera.GetUpDirection() * oldpos.Y);
                    clone.Scale(new Vector3(0.3, 0.3, 0.3));
                    clone.Rotate(oldRotation);
                    worldObjects[i] = clone;
                }

                worldObjects[i].Update(Deltatime);
            }
        }

        List<WorldObject> worldObjects;
        Matrix4 viewMatrix;
        List<WorldObject> viewedWorldObjects;
        FrameLayer[] objLayers;

        /// <summary>
        /// Draws WorldObjects in the Worldspace to the screen using the View and Projection matrices
        /// </summary>
        /// <returns>An array of string layers, indexed by color</returns>
        public string[] DrawObjects()
        {
            deltatime = stopWatch.Elapsed.TotalMilliseconds;
            timeelapsed += deltatime / 1000;
            stopWatch.Restart();
            ++frame;

            Frustum = Projection.GetFrustum(Worldspace.Camera);
            frametext = new List<string>();

            worldObjects = Worldspace.GetWorldObjects();
            viewMatrix = Worldspace.Camera.GetViewMatrix();

            // Make a list to store all the objects we plan on rendering
            viewedWorldObjects = new List<WorldObject>();

            // Cull objects outside of worldspace
            for (int i = 0; i < worldObjects.Count; ++i)
            {
                if (worldObjects[i].Disposed)
                {
                    worldObjects.RemoveAt(i);
                    --i;
                    continue;
                }

                // Perform "post-processing" actions on individual objects.
                if (objPostActions.ContainsKey(worldObjects[i].GetType()))
                {
                    foreach (WOAction action in objPostActions[worldObjects[i].GetType()])
                    {
                        action(worldObjects[i]);
                    }
                }

                if (!Frustum.Contains(worldObjects[i].GetVectorPosition(), 1))
                {
                    continue;
                }

                viewedWorldObjects.Add(worldObjects[i]);
            }

            // Order the viewed WorldObjects from farthest to nearest
            viewedWorldObjects = viewedWorldObjects.OrderBy(obj => WorldUtil.GetWODistance(Worldspace.Camera, obj)).Reverse().ToList();

            // Render each object to color layers in order of distance from the camera
            objLayers = new FrameLayer[viewedWorldObjects.Count];
            for(int i = 0; i < viewedWorldObjects.Count; ++i)
            {
                objLayers[i] = new FrameLayer(screenMgr.Width, screenMgr.Height);
                viewedWorldObjects[i].Render(screenMgr, viewMatrix, projectionMatrix, Projection.ViewspaceFrustum, fl: objLayers[i]);
                screenMgr.MergeFrameLayer(viewedWorldObjects[i].Color, objLayers[i]);
            }
            
            frametext.AddRange(screenMgr.StringifyAllColorLayers());

            // Add the non-3d screen text
            frametext.Add(screenMgr.StringifyText());

            return frametext.ToArray();
        }

        /// <summary>
        /// Draws vertices using a certain ModelType. It's much slower than the hardcoded object drawing
        /// </summary>
        /// <returns></returns>
        public FrameLayer DrawVertices(ModelType type, Vector3[] viewspaceVerts)
        {
            FrameLayer layer = new FrameLayer(screenMgr.Width, screenMgr.Height);
            int amntPerUnit = (type == ModelType.QUADS ? 4 : type == ModelType.TRIS ? 3 : -1);

            for (int i = 0; i < viewspaceVerts.Length; ++i)
            {
                screenMgr.Draw3DLine(viewspaceVerts[i], (viewspaceVerts[i + 1 >= viewspaceVerts.Length ? i : i + 1]), projectionMatrix, Projection.GetFrustum(), layer);
            }

            return layer;
        }
    }
}
