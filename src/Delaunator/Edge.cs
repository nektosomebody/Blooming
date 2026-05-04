using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LevelGenerator
{
    /// <summary>
    /// Ребра создаются только тогда, когда мы 
    /// знаем обо всех соседях вершины
    /// </summary>
    public class Edge
    {
        // индекс в массиве правых вершин
        int cur_left;
        // аналогично в левых
        int cur_right;
        // все соседи левой вершины
        private List<Vertex> lefts;
        // аналогично с правой
        private List<Vertex> rigths;

        public Edge(Vertex left, Vertex rigth)
        {
            lefts = left.all_sorted_neighbours;
            for (int i = 0; i < lefts.Count; ++i)
            {
                if (lefts[i] == rigth) { cur_right = i; break; }
            }
            rigths = rigth.all_sorted_neighbours;
            for (int i = 0; i < rigths.Count; ++i)
            {
                if (rigths[i] == left) { cur_left = i; break; }
            }
        }

        public Vector3 GetLeft()
        {
            return new Vector3(rigths[cur_left].X, rigths[cur_left].Y, 0);
        }

        public Vector3 GetRigth()
        {
            return new Vector3(lefts[cur_right].X, lefts[cur_right].Y, 0);
        }

        /// <param name="center">вершина, вокруг которой вращаем</param>
        public void Rotate(Vertex center)
        {
            if (center == lefts[cur_right])
            {
                RotateAroundRigth();
            }
            else
            {
                RotateAroundLeft();
            }
        }

        private void RotateAroundRigth()
        {
            rigths[cur_left].DeleteEdge(this);
            cur_left = (cur_left + 1) % rigths.Count;
            rigths[cur_left].AddEdge(this, false);
        }

        private void RotateAroundLeft()
        {
            lefts[cur_right].DeleteEdge(this);
            cur_right = (cur_right + 1) % lefts.Count;
            lefts[cur_right].AddEdge(this, false);
        }
    }
}
