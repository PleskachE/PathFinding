﻿using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SearchAlgorythms.Top;
using SearchAlgorythms.Extensions;
using System;
using System.Windows.Forms;
using SearchAlgorythms.Algorythms.Statistics;

namespace SearchAlgorythms.Algorythms.SearchAlgorythm
{
    public class WideSearch : ISearchAlgorythm
    {
        protected Queue<IGraphTop> queue 
            = new Queue<IGraphTop>();
        UnweightedGraphSearchAlgoStatistics statCollector;
        protected readonly IGraphTop end;


        public WideSearch(IGraphTop end)
        {
            this.end = end;
            statCollector = new UnweightedGraphSearchAlgoStatistics();
        }

        public IGraphTop GoChippestNeighbour(IGraphTop top)
        {
            var neighbours = top.Neighbours;
            double min = neighbours.Min(t => t.Value);
            return neighbours.Find(t => min == t.Value
                    && t.IsVisited && IsRightNeighbour(t));
        }

        public virtual bool IsRightNeighbour(IGraphTop top)
        {
            return !top.IsEnd;
        }

        public virtual bool IsRightPath(IGraphTop top)
        {
            return !top.IsStart;
        }

        public virtual bool IsRightCellToVisit(IGraphTop button)
        {
            return !button.IsVisited;
        }

        public bool DestinationFound { get; set; }

        public PauseCycle Pause { get; set; }

        public virtual void ExtractNeighbours(IGraphTop button)
        {
            if (button is null)
                return;
            foreach (var neigbour in button.Neighbours)
            {
                if (neigbour.Value == 0 && !neigbour.IsStart)
                    neigbour.Value = button.Value + 1;
                if (!neigbour.IsVisited)
                    queue.Enqueue(neigbour);
            }            
        }

        public virtual bool FindDestionation(IGraphTop start)
        {
            if (end == null)
                return false;
            statCollector.BeginCollectStatistic();
            var currentTop = start;
            Visit(currentTop);
            while (!IsDestination(currentTop))
            {
                currentTop = queue.Dequeue();
                if (IsRightCellToVisit(currentTop))
                    Visit(currentTop);
                Pause(10);              
            }
            statCollector.StopCollectStatistics();
            return end.IsVisited;          
        }

        public void DrawPath(IGraphTop end)
        {
            var top = end;
            while (IsRightPath(top))
            {
                top = GoChippestNeighbour(top);
                if (top.IsSimpleTop)
                    top.MarkAsPath();
                statCollector.AddStep();
                //Pause(250);
            }
        }

        public bool IsDestination(IGraphTop button)
        {
            if (button is null)
                return false;
            return button.IsEnd || queue.IsEmpty();
        }

        public void Visit(IGraphTop top)
        {          
            if (top.IsObstacle)
                return;
            top.IsVisited = true;
            if (top.IsSimpleTop)
                top.MarkAsVisited();
            statCollector.CellVisited();
            ExtractNeighbours(top);
        }

        public string GetStatistics()
        {
            return statCollector.Statistics;
        }
    }
}
