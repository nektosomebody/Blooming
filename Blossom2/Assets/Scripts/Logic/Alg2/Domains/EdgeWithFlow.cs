using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alg2.Domains
{
    public class EdgeWithFlow
    {
        public Vertex To { get; private set; }
        public int Cap { get; private set; }
        public int Rev { get; private set; }
        public int Flow { get; set; }
        public EdgeWithFlow(EdgeWithFlow other)
        {
            To = other.To;
            Cap = other.Cap;
            Rev = other.Rev;
            Flow = other.Flow;
        }
        public EdgeWithFlow(Vertex to, int cap, int rev)
        {
            To = to;
            Cap = cap;
            Flow = 0;
            Rev = rev;
        }

        public void ResetFlow()
        {
            Flow = 0;
        }
    }

}
