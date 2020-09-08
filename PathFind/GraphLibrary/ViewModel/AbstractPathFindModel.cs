﻿using GraphLibrary.AlgoSelector;
using System.Diagnostics;
using System.Linq;
using GraphLibrary.Extensions;
using GraphLibrary.Enums;
using GraphLibrary.Graphs;
using GraphLibrary.PathFindingAlgorithm.Interface;
using GraphLibrary.ViewModel.Interface;

namespace GraphLibrary.ViewModel
{
    public abstract class AbstractPathFindModel : IModel
    {
        public int DelayTime { get; set; } // miliseconds
        public Algorithms Algorithm { get; set; }

        public AbstractPathFindModel(IMainModel mainViewModel)
        {
            this.mainViewModel = mainViewModel;
            graph = mainViewModel.Graph;
            pathFindStatisticsFormat = LibraryResources.StatisticsFormat;
            badResultMessage = LibraryResources.BadResultMsg;
            DelayTime = 4;
        }

        public virtual void FindPath()
        {
            pathAlgorithm = AlgorithmSelector.
                GetPathFindAlgorithm(Algorithm, graph);
            PrepareAlgorithm();
            var path = pathAlgorithm.FindPath();
            mainViewModel.Statistics +=
                string.Format("   " + pathFindStatisticsFormat,
                path.Count(),
                path.Sum(vertex => vertex.Cost),
                graph.NumberOfVisitedVertices);
            mainViewModel.Graph.RemoveExtremeVertices();
        }

        protected IPathFindingAlgorithm pathAlgorithm;

        protected virtual void PrepareAlgorithm()
        {
            var timer = new Stopwatch();

            pathAlgorithm.OnVertexVisited += (vertex) =>
            {
                vertex.IsVisited = true;
                if (vertex.IsSimpleVertex())
                    vertex.MarkAsVisited();
            };

            pathAlgorithm.OnStarted +=
                (sender, eventArgs) => { timer.Start(); };

            pathAlgorithm.OnFinished += (sender, eventArgs) =>
            {
                timer.Stop();
                mainViewModel.Statistics = timer.
                GetTimeInformation(LibraryResources.TimerInfoFormat);
                pathAlgorithm.DrawPath();
            };
        }

        protected Graph graph;
        protected IMainModel mainViewModel;
        protected string badResultMessage;
        protected string pathFindStatisticsFormat;
    }
}
