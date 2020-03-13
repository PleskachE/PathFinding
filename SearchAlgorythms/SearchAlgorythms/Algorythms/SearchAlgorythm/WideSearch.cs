﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using SearchAlgorythms.Top;

namespace SearchAlgorythms.Algorythms.SearchAlgorythm
{
    public class WideSearch : ISearchAlgorythm
    {
        private Queue<GraphTop> queue = new Queue<GraphTop>();
        private Button[,] graph = null;

        public WideSearch(Button[,] graph)
        {
            this.graph = graph;
        }

        public void ExtractNeighbours(Button button)
        {
            GraphTop top = button as GraphTop;
            if (top is null)
                return;
            foreach (var g in top.GetNeighbours())
                queue.Enqueue(g);
        }

        public void FindDestionation(GraphTop start)
        {
            Visit(start);
            GraphTop currentTop = queue.Dequeue();
            while(!IsDestination(currentTop))
            {
                if (!currentTop.IsVisited 
                    && currentTop.BackColor != Color.FromName("Black"))
                    Visit(currentTop);
                currentTop = queue.Dequeue();
            }
        }

        public bool IsDestination(Button button)
        {
            GraphTop top = button as GraphTop;
            if (top is null)
                return false;
            return top.IsEnd;
        }

        public void Visit(Button button)
        {
            GraphTop top = button as GraphTop;
            top.IsVisited = true;
            if (!top.IsStart)
                top.BackColor = Color.FromName("Blue");
            ExtractNeighbours(top);
        }
    }
}
