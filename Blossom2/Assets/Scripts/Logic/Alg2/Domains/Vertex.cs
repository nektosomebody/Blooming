using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alg2.Domains
{
    public class Vertex
    {
        public const float MAXX = 1000000000;
        public float x { get; private set; } = MAXX;
        public float y { get; private set; } = MAXX;
        public HashSet<Vertex> AllNeighbours { get; private set; } = new HashSet<Vertex>();
        public int ind { get; private set; }

        public Vertex(int ind) 
        { 
            this.ind = ind;
        }
        public Vertex(int ind, float x, float y)
        {
            this.ind = ind;
            this.x = x;
            this.y = y;
        }

        internal void ChangeCoordinates(float x, float y)
        {
            this.x = x; 
            this.y = y;
        }
        public void AddToAllNeighbours(Vertex vert)
        {
            AllNeighbours.Add(vert);
        }
        public void RemoveFromAllNeighbours(Vertex vert)
        {
            AllNeighbours.Remove(vert);
        }
        public int CompareTo(Vertex other)
        {
            if (other == null) return 1;
            return ind.CompareTo(other.ind);
        }
        public override string ToString()
        {
            return ind.ToString();
        }
    }
}
