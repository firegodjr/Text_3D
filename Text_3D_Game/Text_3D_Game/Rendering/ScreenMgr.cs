using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Text_3D_Renderer.World;

namespace Text_3D_Renderer.Rendering
{
    /// <summary>
    /// Contains the different layers 
    /// </summary>
    public enum Layer
    {
        /// <summary>
        /// The white rendering layer
        /// </summary>
        WHITE,
        /// <summary>
        /// The gray rendering layer
        /// </summary>
        GRAY,
        /// <summary>
        /// The red rendering layer
        /// </summary>
        RED,
        /// <summary>
        /// The yellow rendering layer
        /// </summary>
        YELLOW,
        /// <summary>
        /// The green rendering layer
        /// </summary>
        GREEN,
        /// <summary>
        /// The blue rendering layer
        /// </summary>
        BLUE,
        /// <summary>
        /// The readable-text layer
        /// </summary>
        TEXT
    }
    /// <summary>
    /// A class for managing FrameLayers and converting them to strings for console output
    /// </summary>
    public class ScreenMgr
    {
        /// <summary>
        /// The index of a text/color layer
        /// </summary>
        public const int WHITE = 0, GRAY = 1, RED = 2, YELLOW = 3, GREEN = 4, BLUE = 5, TEXT = 6;
        public int LayerAmnt = 6;
        private int width, height;

        public const char DRAW_CHAR = '#';
        public const char CLEAR_CHAR = ' ';

        /// <summary>
        /// The width of the screen
        /// </summary>
        public int Width { get { return width; } }

        /// <summary>
        /// The height of the screen
        /// </summary>
        public int Height { get { return height; } }

        Dictionary<int, bool[][]> pixelLayers = new Dictionary<int, bool[][]>();
        char[][] textLayer;

        /// <summary>
        /// Creates a new ScreenMgr with given width and height
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public ScreenMgr(int width, int height)
        {
            this.width = width;
            this.height = height;
            for(int i = 0; i < LayerAmnt; ++i)
            {
                pixelLayers[i] = new bool[width][];
                for(int x = 0; x < width; ++x)
                {
                    pixelLayers[i][x] = new bool[height];
                }
            }
            textLayer = new char[width][];
            for (int x = 0; x < width; ++x)
            {
                textLayer[x] = new char[height];
            }
        }

        private void ClearTextLayer()
        {
            textLayer = new char[width][];
            for (int x = 0; x < width; ++x)
            {
                textLayer[x] = new char[height];
            }
        }

        private void ClearColorLayer(Layer color)
        {
            pixelLayers[(int)color] = new bool[width][];
            for (int x = 0; x < width; ++x)
            {
                pixelLayers[(int)color][x] = new bool[height];
            }
        }

        public Vector2 ToExactScreenCoords(Vector3 point, Matrix4 projection)
        {
            //Multiply by MVP Matrix
            Vector3 rawPos = Matrix4.MultiplyVector(point, projection);

            //Calculate rough screen position
            double rawX = Width * (rawPos.X + 1.0) / 2.0;
            double rawY = Height * (1.0 - ((rawPos.Y + 1.0) / 2.0));
            return new Vector2(rawX, rawY);
        }

        public Coords ToScreenCoords(Vector3 point, Matrix4 projection)
        {
            //Multiply by MVP Matrix
            Vector3 rawPos = Matrix4.MultiplyVector(point, projection);

            //Calculate screen position
            int X = Convert.ToInt32(Width * (rawPos.X + 1.0) / 2.0);
            int Y = Convert.ToInt32(Height * (1.0 - ((rawPos.Y + 1.0) / 2.0)));
            return new Coords(X, Y);
        }

        /// <summary>
        /// Draws all non-3d text to a string with screen dimensions and returns it
        /// </summary>
        /// <returns></returns>
        public string StringifyText()
        {
            string output = "";
            string frame = "";

            for (short y = 0; y < textLayer[0].Length; ++y)
            {
                output = "";
                for (short x = 0; x < textLayer.Length; ++x)
                {
                    if (textLayer[x][y] != default(char))
                    {
                        output += textLayer[x][y];
                    }
                    else
                    {
                        output += " ";
                    }
                }
                frame = (y < Height - 1 ? "\n" : "") + output + frame;
            }

            ClearTextLayer();

            return frame;
        }

        /// <summary>
        /// Converts the internal array of bools (for the given color) to a string
        /// </summary>
        /// <param name="color"></param>
        /// <returns>The resulting text</returns>
        public string StringifyColorLayer(Layer color, bool debug = false)
        {
            StringBuilder output = new StringBuilder();
            StringBuilder frame = new StringBuilder();
            
            for(short y = 0; y < pixelLayers[(int)color][0].Length; ++y)
            {
                output.Clear();
                for(short x = 0; x < pixelLayers[(int)color].Length; ++x)
                {
                    output.Append(pixelLayers[(int)color][x][y] ? DRAW_CHAR : CLEAR_CHAR);
                }
                frame.Insert(0, (y < Height - 1 ? "\n" : "") + output.ToString());
            }
            if(!debug)
            {
                ClearColorLayer(color);
            }

            return frame.ToString();
        }

        /// <summary>
        /// Converts a FrameLayer to a drawable string
        /// </summary>
        /// <param name="color"></param>
        /// <returns>The resulting text</returns>
        public string StringifyFrameLayer(FrameLayer layer)
        {
            string output = "";
            string frame = "";

            int[][] bools = layer.getInternalArray();

            for (short y = 0; y < bools[0].Length; ++y)
            {
                output = "";
                for (short x = 0; x < bools.Length; ++x)
                {
                    output += bools[x][y] == 1 ? "#" : " ";
                }
                frame = (y < bools[0].Length - 1 ? "\n" : "") + output + frame;
            }

            return frame;
        }

        /// <summary>
        /// Gets a string of only spaces with the screen dimensions.
        /// </summary>
        /// <returns></returns>
        public string GetEmptyStringifiedLayer()
        {
            string output = "";
            string frame = "";

            for (short y = 0; y < Height; ++y)
            {
                output = "";
                for (short x = 0; x < Width; ++x)
                {
                    output +=  " ";
                }
                frame = (y < Height - 1 ? "\n" : "") + output + frame;
            }

            return frame;
        }

        /// <summary>
        /// Returns a list of all rendered strings, indexed by color
        /// </summary>
        /// <returns></returns>
        public string[] StringifyAllColorLayers(bool debug = false)
        {
            string[] colors = new string[LayerAmnt];
            
            for(int i = 0; i < LayerAmnt; ++i)
            {
                if (pixelLayers.ContainsKey(i))
                {
                    colors[i] = StringifyColorLayer((Layer)i, debug);
                }
                else
                {
                    colors[i] = GetEmptyStringifiedLayer();
                }
            }

            return colors;
        }

        /// <summary>
        /// Adds a pixel to an internal color layer
        /// </summary>
        /// <param name="color"></param>
        /// <param name="pos"></param>
        public void AddPixelToColorLayer(Layer color, Coords pos)
        {
            int x = pos.X,
                y = pos.Y;

            if(x < Width && y < Height && x > 0 && y > 0)
            {
                pixelLayers[(int)color][x][y] = true;
            }
        }

        /// <summary>
        /// Adds a string that will draw left-to-right, starting at the given screen coordinates
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="text"></param>
        public void AddTextToDraw(Coords pos, string text)
        {
            int x = pos.X;
            int y = pos.Y;
            for (int i = 0; i < text.Length + 0; ++i)
            {
                if(i + x < Width && y < Height && x >= 0 && y >= 0)
                {
                    textLayer[i + x][y] = text[i];
                }
            }
        }

        /// <summary>
        /// Draws lines between several given points
        /// </summary>
        /// <param name="fl"></param>
        /// <param name="points"></param>
        public void DrawLineBetweenAll(FrameLayer fl, Matrix4 projection, Frustum frustum, params Vector3[] points)
        {
            if (points.Length > 1)
            {
                for(int i = 0; i < points.Length; ++i)
                {
                     Draw3DLine(points[i], i + 1 >= points.Length ? points[0] : points[i + 1], projection, frustum, fl);
                }
            }
        }

        /// <summary>
        /// The math-only near-plane-culling line-drawing method. Doesn't work. (yet!)
        /// </summary>
        /// <param name="vPoint1"></param>
        /// <param name="vPoint2"></param>
        /// <param name="projection"></param>
        /// <param name="frustum"></param>
        /// <param name="fl"></param>
        /// <param name="color"></param>
        public void DrawLineCullingNear(Vector3 vPoint1, Vector3 vPoint2, Matrix4 projection, Frustum frustum, FrameLayer fl = null, Layer color = Layer.WHITE)
        {
            if (vPoint1.Mag > frustum.FarDist / 10 && vPoint2.Mag > frustum.FarDist / 10)
            {
                DrawLine(ToScreenCoords(vPoint1, projection), ToScreenCoords(vPoint2, projection), fl);
                return;
            }

            if (frustum.Contains(vPoint1) && frustum.Contains(vPoint2))
            {
                DrawLine(ToScreenCoords(vPoint1, projection), ToScreenCoords(vPoint2, projection), fl);
            }
            else
            {
                //TODO fix this
                if(frustum.Contains(vPoint1) || frustum.Contains(vPoint2))
                {
                    Plane nearPlane = frustum.getNearPlane();
                    double p1dist = nearPlane.SignedDist(vPoint1);
                    double p2dist = nearPlane.SignedDist(vPoint2);
                    if (p1dist > 0)
                    {
                        double height = p1dist - p2dist;
                        double width = vPoint1.X - vPoint2.X;
                        double depth = vPoint1.Y - vPoint2.Y;
                        double whRatio = width / height;
                        double adjustX = whRatio * p2dist;
                        double adjustY = depth / height * p2dist;
                        vPoint2 = new Vector3(vPoint2.X + adjustX, vPoint2.Y + adjustY, frustum.NearDist);
                    }
                    else
                    {
                        double height = p1dist - p2dist;
                        double width = vPoint1.X - vPoint2.X;
                        double depth = vPoint1.Y - vPoint2.Y;
                        double whRatio = width / height;
                        double adjustX = whRatio * p2dist;
                        double adjustY = depth / height * p2dist;
                        vPoint2 = new Vector3(vPoint2.X + adjustX, vPoint2.Y + adjustY, frustum.NearDist);

                    }
                }

                DrawLine(ToScreenCoords(vPoint1, projection), ToScreenCoords(vPoint2, projection), fl);
            }
        }

        /// <summary>
        /// Draws a line between two points in viewspace, taking the view frustum into account by cutting lines into segments. Is good enough for most cases.
        /// </summary>
        /// <param name="vPoint1"></param>
        /// <param name="vPoint2"></param>
        /// <param name="modelMatrix"></param>
        /// <param name="MVPMatrix"></param>
        /// <param name="camera"></param>
        /// <param name="frustum"></param>
        /// <param name="fl"></param>
        public void Draw3DLine(Vector3 vPoint1, Vector3 vPoint2, Matrix4 projection, Frustum frustum, FrameLayer fl = null, Layer color = Layer.WHITE)
        {
            bool mergingToColorLayer = false;

            if(vPoint1.Mag > frustum.FarDist / 10 && vPoint2.Mag > frustum.FarDist / 10)
            {
                DrawLine(ToScreenCoords(vPoint1, projection), ToScreenCoords(vPoint2, projection), fl);
                return;
            }

            if(fl == null)
            {
                mergingToColorLayer = true;
                fl = new FrameLayer(Width, Height);
            }

            //The direction in space that this line progresses
            Vector3 direction = vPoint1 - vPoint2;
            direction.Normalize();

            //The maximum length that this line will travel
            double iterationLength = WorldUtil.GetVectorDistance(vPoint1, vPoint2);

            //The last position that a line segment started from
            Vector3 lastPos = (Vector3)vPoint1.Clone();
            //The next position that a line segment will start from
            Vector3 nextPos = new Vector3();
            //The final position that a line segment will draw to
            Vector3 endPos = (Vector3)vPoint2.Clone();
            //The last screen position that a line started from
            Coords lastScreenPos = ToScreenCoords(lastPos, projection);
            //The next screen position that a line will start from
            Coords nextScreenPos = new Coords();
            //The length that this line segment will be
            double initialDistance = GetStepLength(lastPos);
            double nextStepLength = initialDistance / 2;

            //If this process already tried inverting the direction
            bool invertedOnce = false;
            bool doneDrawing = false;

            //If this process is currently drawing a line segment outside of the screen
            // (This happens to keep screen edges seamless)
            bool isDoingOverdraw = false;
            for (double currLength = 0; currLength <= iterationLength; currLength += nextStepLength)
            {
                //Make sure line segment will fit into the maximum length
                if(nextStepLength + currLength > iterationLength)
                {
                    nextPos = endPos;
                    currLength = iterationLength;
                }
                else
                {
                    nextPos = lastPos + direction * -nextStepLength;
                }

                //Find the next position
                if (frustum.Contains(lastPos) || frustum.Contains(nextPos))
                {
                    nextScreenPos = ToScreenCoords(nextPos, projection);
                    DrawLine(lastScreenPos, nextScreenPos, fl);
                    lastPos = nextPos;
                    lastScreenPos = nextScreenPos;

                    //Get the next step length
                    nextStepLength = GetStepLength(lastPos);

                    if(!doneDrawing && nextStepLength + currLength > iterationLength)
                    {
                        nextStepLength = WorldUtil.GetVectorDistance(lastPos, endPos);
                        doneDrawing = true;
                    }
                }
                else
                {
                    if (currLength == 0 && !invertedOnce) //If we just started, try and draw it backwards
                    {
                        lastPos = (Vector3)vPoint2.Clone();
                        nextPos = new Vector3();
                        endPos = (Vector3)vPoint1.Clone();
                        lastScreenPos = ToScreenCoords(lastPos, projection);
                        direction = vPoint2 - vPoint1;
                        direction.Normalize();
                        currLength -= nextStepLength;
                        invertedOnce = true;
                    }

                    nextStepLength += 0.1;
                }
            }
            if(mergingToColorLayer)
            {
                MergeFrameLayer(color, fl);
            }
        }

        private double GetStepLength(Vector3 viewspacePoint)
        {
            return viewspacePoint.Mag;
        }

        /// <summary>
        /// Draws a line between two points
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <param name="color"></param>
        /// <param name="fl">A FrameLayer to draw the line to</param>
        public void DrawLine(Coords point1, Coords point2, FrameLayer fl)
        {
            fl.Draw(point1);
            fl.Draw(point2);

            int x1 = Convert.ToInt32(point1.X),
                y1 = Convert.ToInt32(point1.Y),
                x2 = Convert.ToInt32(point2.X),
                y2 = Convert.ToInt32(point2.Y); 
            if(fl == null)
            {
                fl = new FrameLayer(Width, Height);
            }

            //Draw the line between the two points
            //If it's less than one, do horizontal drawing
            //If it's more than one, do vertical drawing
            if (Math.Abs(x2 - x1) > Math.Abs(y2 - y1))
            {
                //Make sure the start point has a lower X value
                if (x1 > x2)
                {
                    int swapX = x1;
                    x1 = x2;
                    x2 = swapX;
                    int swapY = y1;
                    y1 = y2;
                    y2 = swapY;
                }

                double[] yValues = Util.Interpolate(x1, y1, x2, y2);

                for(int x = x1; x < x2; ++x)
                {
                    fl.Draw(x, (int)yValues[x - x1]);
                }
            }
            else
            {
                //Make sure the start point has a lower Y value
                if (y1 > y2)
                {
                    int swapX = x1;
                    x1 = x2;
                    x2 = swapX;
                    int swapY = y1;
                    y1 = y2;
                    y2 = swapY;
                }
                
                double[] xValues = Util.Interpolate(y1, x1, y2, x2);

                for (int y = y1; y < y2; ++y)
                {
                    fl.Draw((int)xValues[y - y1], y);
                }
            }
        }

        int fillcount = 0;
        /// <summary>
        /// Recursively fills a given space in a FrameLayer with either white or black. If no FrameLayer is given, it writes directly to the WHITE color layer.
        /// </summary>
        /// <param name="startX"></param>
        /// <param name="startY"></param>
        /// <param name="fillColor"></param>
        /// <param name="wallColor"></param>
        /// <param name="color"></param>
        /// <param name="fl"></param>
        public void Fill(int startX, int startY, int fillColor, int wallColor, int color = WHITE, FrameLayer fl = null)
        {
            fillcount++;
            bool drawToFrameBuffer = true;
            if(fl == null)
            {
                fl = new FrameLayer(Width, Height);
                drawToFrameBuffer = false;
            }

            //If it's within the bounds of the screen
            if(startX < Width && startX > 0 && startY < Height && startY > 0)
            {
                if(fl.getInternalArray()[startX][startY] != wallColor)
                {
                    fillRecurse(startX, startY, fillColor, wallColor, fl);
                }
            }


            if(!drawToFrameBuffer && color > 0)
            {
                MergeFrameLayer((Layer)color, fl);
            }

        }

        private void fillRecurse(int nodeX, int nodeY, int fillColor, int wallColor, FrameLayer fb)
        {
            fillcount++;
            if (fb == null)
            {
                fb = new FrameLayer(Width, Height);
            }

            if (fb.getInternalArray()[nodeX][nodeY] == fillColor || fb.getInternalArray()[nodeX][nodeY] == wallColor)
            {
                return;
            }
            else
            {
                fb.Draw(nodeX, nodeY, fillColor > 0 ? true : false);
            }

            if (nodeY > 0)
            {
                fillRecurse(nodeX, nodeY - 1, fillColor, wallColor, fb);
            }

            if (nodeY < Height - 1)
            {
                fillRecurse(nodeX, nodeY + 1, fillColor, wallColor, fb);
            }

            if (nodeX > 0)
            {
                fillRecurse(nodeX - 1, nodeY, fillColor, wallColor, fb);
            }

            if (nodeX < Width - 1)
            {
                fillRecurse(nodeX + 1, nodeY, fillColor, wallColor, fb);
            }
        }

        /// <summary>
        /// Changes the screen size and updates all internal pixel and text layers accordingly
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void ChangeScreenSize(int width, int height)
        {
            this.width = width;
            this.height = height;

            for (int i = 0; i < LayerAmnt; ++i)
            {
                ClearColorLayer((Layer)i);
            }

            ClearTextLayer();
        }
        
        /// <summary>
        /// Gets a new FrameLayer with the screen's width and height
        /// </summary>
        /// <returns></returns>
        public FrameLayer GetNewFrameLayer()
        {
            return new FrameLayer(Width, Height);
        }

        /// <summary>
        /// Merges a frame layer
        /// </summary>
        /// <param name="color"></param>
        /// <param name="fb"></param>
        public void MergeFrameLayer(Layer color, FrameLayer fb)
        {
            int[][] array = fb.getInternalArray();
            for(int x = 0; x < Width - 1; ++x)
            {
                for(int y = 0; y < Height - 1; ++y)
                {
                    if(array[x][y] <= -1)
                    {
                        for(int i = 0; i < LayerAmnt; ++i)
                        {
                            pixelLayers[i][x][y] = false;
                        }
                    }
                    else if(array[x][y] >= 1)
                    {
                        for (int i = 0; i < LayerAmnt; ++i)
                        {
                            pixelLayers[i][x][y] = false;
                        }

                        pixelLayers[(int)color][x][y] = true;
                    }
                }
            }
        }
    }
}
