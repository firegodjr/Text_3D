using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_3D_Renderer.World
{
    public class Model
    {
        protected ModelType type;
        protected Vector3[] vertices;

        public ModelType ModelType
        {
            get { return type; }
        }

        public Vector3[] Vertices
        {
            get { return vertices; }
        }

        public Model()
        {
            type = ModelType.QUADS;
            vertices = new Vector3[0];
        }

        public Model(ModelType type, params Vector3[] vertices)
        {
            this.type = type;
            this.vertices = vertices;
        }
    }

    public enum ModelType
    {
        TRIS,
        QUADS
    }
}
