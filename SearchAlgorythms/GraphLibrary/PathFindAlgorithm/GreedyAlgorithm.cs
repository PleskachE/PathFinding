﻿using System.Collections.Generic;
using System.Linq;
using SearchAlgorythms.Extensions.ListExtensions;
using SearchAlgorythms.Graph;
using SearchAlgorythms.Statistics;
using SearchAlgorythms.Top;

namespace SearchAlgorythms.Algorithm
{
    /// <summary>
    /// Greedy algorithm. Each step looks for the chippest top and visit it
    /// </summary>
    public class GreedyAlgorithm : IPathFindAlgorithm
    {
        private readonly AbstractGraph graph;
        private Stack<IGraphTop> stack = new Stack<IGraphTop>();
        private WeightedGraphSearchAlgoStatistics statCollector;

        public GreedyAlgorithm(AbstractGraph graph)
        {
            statCollector = new WeightedGraphSearchAlgoStatistics();
            this.graph = graph;
        }

        private IGraphTop GoChippestNeighbour(IGraphTop top)
        {
            var neighbours = top.Neighbours.Count(t => t.IsVisited) == 0 
                ? top.Neighbours : top.Neighbours.Where(t => !t.IsVisited).ToList();
            neighbours.Shuffle();
            if (neighbours.Any())
            {
                double min = neighbours.Min(t => int.Parse(t.Text));
                return neighbours.Find(t => t.Text == min.ToString());
            }
            return null;
        }

        public PauseCycle Pause { set; get; }

        public void DrawPath()
        {
            var top = graph.End;
            while (!top.IsStart)
            {
                var temp = top;
                top = top.ParentTop;
                if (top.IsSimpleTop)
                    top.MarkAsPath();
                statCollector.AddLength(int.Parse(temp.Text));
                Pause(35);
            }
        }

        public bool FindDestionation()
        {
            if (graph.End == null)
                return false;
            statCollector.BeginCollectStatistic();
            var currentTop = graph.Start;
            IGraphTop temp = null;
            Visit(currentTop);
            while(!IsDestination(currentTop))
            {
                temp = currentTop;
                currentTop = GoChippestNeighbour(currentTop);
                if (IsRightCellToVisit(currentTop))
                {
                    Visit(currentTop);
                    currentTop.ParentTop = temp;
                }
                else
                    currentTop = stack.Pop();
                Pause(2);
            }
            statCollector.StopCollectStatistics();
            return graph.End.IsVisited;
        }

        private bool IsDestination(IGraphTop top)
        {
            return top.IsEnd && top.IsVisited || !stack.Any();
        }

        private bool IsRightCellToVisit(IGraphTop top)
        {
            if (top == null)
                return false;
            if (top.IsObstacle)
                return false;
            return true;
        }

        private void Visit(IGraphTop top)
        {
            top.IsVisited = true;
            stack.Push(top);
            if (top.IsSimpleTop)
            {
                top.MarkAsCurrentlyLooked();
                Pause(8);
                top.MarkAsVisited();
            }
            statCollector.CellVisited();
        }

        public string GetStatistics()
        {
            return statCollector.Statistics;
        }
    }
}
