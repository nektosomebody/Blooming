using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alg1
{
    public class Edge
    {
        // start always isRotateAble
        public Vertex start { get; private set; }
        public Vertex end { get; private set; }

        public Edge(Vertex start, Vertex end)
        {
            if (start.isRotateAble)
            {
                this.start = start;
                this.end = end;
            }
            else
            {
                this.start = end;
                this.end = start;
            }
        }

        public void Rotate(Vertex new_v)
        {
            if (start.isRotateAble)
            {
                end = new_v;
            }
            else
            {
                start = new_v;
            }
        }
    }
}
