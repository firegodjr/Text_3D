using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_3D_Renderer.Rendering
{
    public class ScreenMgr
    {
        public int width, height;
        List<double> xcoords = new List<double>();
        List<double> ycoords = new List<double>();
        List<Vector2> pixelsToDraw = new List<Vector2>();
        bool[,] pixelGrid;
        char[,] textToDraw;

        public ScreenMgr(int width, int height)
        {
            this.width = width;
            this.height = height;
            pixelGrid = new bool[width, height];
            textToDraw = new char[width, height];
        }

        public string DrawPixels(bool debug = false)
        {
            string output = "";
            string frame = "";
            
            for(short y = 0; y < height; ++y)
            {
                output = "";
                for(short x = 0; x < width; ++x)
                {
                    if (textToDraw[x, y] != default(char))
                    {
                        output += textToDraw[x, y];
                    }
                    else
                    {
                        output += pixelGrid[x, y] ? "#" : " ";
                    }
                }
                frame = "\n" + output + frame;
            }

            if(debug == false)
            {
                pixelGrid = new bool[width, height];
                textToDraw = new char[width, height];
                ycoords.Clear();
                xcoords.Clear();
            }

            return frame;
        }

        public void addPixelToDraw(Vector2 vec2)
        {
            if(vec2.X < width && vec2.Y < height && vec2.X > 0 && vec2.Y > 0)
            {
                pixelGrid[(int)vec2.X, (int)vec2.Y] = true;
            }
        }

        public void addTextToDraw(Vector2 pos, string text)
        {
            for(int i = 0; i < text.Length + 0; ++i)
            {
                textToDraw[i + (int)pos.X, (int)pos.Y] = text[i];
            }
        }

        public void drawLineBetweenAll(params Vector2[] points)
        {
            if (points.Length > 1)
            {
                for(int i = 0; i < points.Length; ++i)
                {
                     drawLine(points[i], i + 1 >= points.Length ? points[0] : points[i + 1]);
                }
            }
        }

        public void drawLine(Vector2 target, Vector2 start)
        {
            //Get the slope of the line to draw
            double lineSlope = (target.Y - start.Y) / (target.X - start.X);

            //Draw the line between the two points
            //If it's less than one, do horizontal drawing 
            //If it's more than one, do vertical drawing
            if (lineSlope <= 1 && lineSlope >= -1)
            {
                //Make sure start has a lower X value
                if (start.X > target.X)
                {
                    Vector2 swap = (Vector2)start.Clone();
                    start = (Vector2)target.Clone();
                    target = swap;
                }

                for (int x = 0; x < target.X - start.X; ++x)
                {
                    //Make sure it's not drawing lines offscreen
                    if (x + start.X > width)
                    {
                        break;
                    }
                    else if (x + start.X < 0)
                    {
                        x = (int)Math.Round(-start.X);
                    }

                    addPixelToDraw(new Vector2(x + start.X, Math.Round(lineSlope * x) + start.Y));
                }
            }
            else if (lineSlope > 1 || lineSlope < -1)
            {
                //Make sure start has a lower Y value
                if (start.Y > target.Y)
                {
                    Vector2 swap = (Vector2)start.Clone();
                    start = (Vector2)target.Clone();
                    target = swap;
                }

                for (int y = 0; y < target.Y - start.Y; ++y)
                {
                    //Make sure it's not drawing lines offscreen
                    if (y + start.Y > height)
                    {
                        break;
                    }
                    else if (y + start.Y < 0)
                    {
                        y = (int)Math.Round(-start.Y);
                    }

                    addPixelToDraw(new Vector2(Math.Round(y / lineSlope) + start.X, y + start.Y));
                }
            }
        }

        public void changeScreenSize(int width, int height)
        {
            this.width = width;
            this.height = height;
            pixelGrid = new bool[width, height];
            textToDraw = new char[width, height];
        }
    }
}
