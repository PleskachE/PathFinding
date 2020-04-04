﻿using System.Collections.Generic;
using System.Linq;
using SearchAlgorythms.Graph;
using SearchAlgorythms.Top;

namespace SearchAlgorythms.Algorithm
{
    /// <summary>
    /// An euristic Li algorithm. Ignors tops that are far from destination top
    /// </summary>
    public class BestFirstAlgorithm : WidePathFindAlgorithm
    {
        private Queue<IGraphTop> waveQueue = new Queue<IGraphTop>();

        public BestFirstAlgorithm(AbstractGraph graph) : base(graph)
        {
            
        }

        private void MakeWavesFromEnd(IGraphTop end)
        {
            var top = end;
            MarkTop(top);
            while (!top.IsStart && waveQueue.Any())
            {
                top = waveQueue.Dequeue();
                MarkTop(top);
            }
        }

        public override bool IsRightNeighbour(IGraphTop top)
        {
            return !top.IsStart;
        }

        public override bool IsRightPath(IGraphTop top)
        {
            return !top.IsEnd;
        }

        public override bool IsRightCellToVisit(IGraphTop button)
        {
            return base.IsRightCellToVisit(button) && button.Value > 0;
        }

        private void MarkTop(IGraphTop top)
        {
            if (top is null)
                return;
            foreach (var neigbour in top.Neighbours)
            {
                if (neigbour.Value == 0)
                {
                    neigbour.Value = top.Value + 1;
                    waveQueue.Enqueue(neigbour);
                }
            }
        }

        public override void ExtractNeighbours(IGraphTop button)
        {
            if (button is null)
                return;
            foreach (var neigbour in button.Neighbours)
                if (!neigbour.IsVisited)
                    queue.Enqueue(neigbour);
        }

        public override bool FindDestionation()
        {
            graph.End.Value = 1;
            MakeWavesFromEnd(graph.End);           
            bool found = base.FindDestionation();
            graph.End = graph.Start;
            return found;
        }
    }
}
