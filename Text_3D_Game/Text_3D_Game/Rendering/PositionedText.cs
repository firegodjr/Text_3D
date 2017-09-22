using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_3D_Renderer.Rendering
{
    class PositionedText
    {
        public Vector2 pos = new Vector2();
        public string text = "";

        public PositionedText(Vector2 pos, string text)
        {
            this.pos = pos;
            this.text = text;
        }
    }
}
