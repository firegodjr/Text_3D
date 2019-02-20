using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_3D_Renderer.Rendering
{
    /// <summary>
    /// A virtual layer of pixels to be applied to the final frame, can contain white, black, and transparent pixels
    /// </summary>
    public class FrameLayer
    {
        int[][] array;
        int width, height;

        /// <summary>
        /// Creates a new framelayer with the given width and height
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public FrameLayer(int width, int height)
        {
            array = new int[width][];
            for(int x = 0; x < width; ++x)
            {
                array[x] = new int[height];
            }

            this.width = width;
            this.height = height;
        }

        /// <summary>
        /// Directly gets the internal array of values
        /// </summary>
        /// <returns></returns>
        public int[][] getInternalArray()
        {
            return array;
        }

        /// <summary>
        /// Draws either a white or black pixel to the buffer
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="isWhite"></param>
        public void Draw(int x, int y, bool isWhite = true)
        {
            if (x >= 0 && x < width && y >= 0 && y < height)
            {
                array[x][y] = isWhite ? 1 : -1;
            }
        }

        public void Draw(Coords pos, bool isWhite = true)
        {
            Draw(pos.X, pos.Y, isWhite);
        }

        /// <summary>
        /// Deletes a pixel from the buffer
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void erase(int x, int y)
        {
            if (x >= 0 && x < width && y >= 0 && y < height)
            {
                array[x][y] = 0;
            }
        }
    }
}
