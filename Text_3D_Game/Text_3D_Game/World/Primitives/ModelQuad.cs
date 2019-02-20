using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_3D_Renderer.World
{
    public class ModelQuad : Model
    {
        public ModelQuad() : this(ModelType.QUADS)
        { }

        public ModelQuad(ModelType type)
        {
            this.type = type;
            if(type == ModelType.QUADS)
            {
                vertices = new Vector3[] 
                {
                    new Vector3(-1, -1, 0),
                    new Vector3(-1, 1, 0),
                    new Vector3(1, 1, 0),
                    new Vector3(1, -1, 0)
                };
            }
            else if(type == ModelType.TRIS)
            {
                vertices = new Vector3[]
                {
                    new Vector3(-1, 1, 0),
                    new Vector3(-1, -1, 0),
                    new Vector3(1, 1, 0),
                    new Vector3(-1, -1, 0),
                    new Vector3(1, 1, 0),
                    new Vector3(1, -1, 0)
                };
            }
        }
    }
}
