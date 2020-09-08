﻿using GraphLibrary.Extensions;
using GraphLibrary.Graphs;
using System;
using System.Collections.Generic;
using GraphLibrary.EventArguments;
using GraphLibrary.Extensions.CollectionExtensions;
using GraphLibrary.Graphs.Interface;
using GraphLibrary.PathFindingAlgorithm.Interface;
using GraphLibrary.Vertex.Interface;

namespace GraphLibrary.PathFindingAlgorithm
{
    /// <summary>
    /// Finds the chippest path to destination vertex. 
    /// </summary>
    public class DijkstraAlgorithm : IPathFindingAlgorithm
    {
        public event AlgorithmEventHanlder OnStarted;
        public event Action<IVertex> OnVertexVisited;
        public event AlgorithmEventHanlder OnFinished;

        public Func<IVertex, IVertex, double> RelaxFunction { private get; set; }
        public IGraph Graph { get; set; }

        public DijkstraAlgorithm()
        {
            Graph = NullGraph.GetInstance();
            neigbourQueue = new List<IVertex>();
            RelaxFunction = (neighbour, vertex)
                => neighbour.Cost + vertex.AccumulatedCost;
        }

        public IEnumerable<IVertex> FindPath()
        {
            OnStarted?.Invoke(this,
                new AlgorithmEventArgs(Graph));
            SetAccumulatedCostToInfinity();
            var currentVertex = Graph.Start;
            currentVertex.IsVisited = true;
            do
            {
                ExtractNeighbours(currentVertex);
                SpreadRelaxWave(currentVertex);
                currentVertex = GetChippestUnvisitedVertex();
                OnVertexVisited?.Invoke(currentVertex);
            } while (!currentVertex.IsEnd);
            OnFinished?.Invoke(this,
                new AlgorithmEventArgs(Graph));
            return this.GetFoundPath();
        }

        private void SetAccumulatedCostToInfinity()
        {
            IVertex SetValueToInfinity(IVertex vertex)
            {
                if (!vertex.IsStart && !vertex.IsObstacle)
                    vertex.AccumulatedCost = double.PositiveInfinity;
                return vertex;
            }
            Graph.Array.Apply(SetValueToInfinity);
        }

        private void SpreadRelaxWave(IVertex vertex)
        {
            foreach (var neighbour in vertex.Neighbours)
            {
                var relaxFunctionResult = RelaxFunction(neighbour, vertex);
                if (neighbour.AccumulatedCost > relaxFunctionResult)
                {
                    neighbour.AccumulatedCost = relaxFunctionResult;
                    neighbour.ParentVertex = vertex;
                }
            }
        }

        private void ExtractNeighbours(IVertex vertex)
        {
            neigbourQueue.AddRange(vertex.GetUnvisitedNeighbours());
        }

        private IVertex GetChippestUnvisitedVertex()
        {
            neigbourQueue.RemoveAll(vertex => vertex.IsVisited);
            neigbourQueue.Sort((vertex1, vertex2)
                => vertex1.AccumulatedCost.CompareTo(vertex2.AccumulatedCost));
            return neigbourQueue.FirstSecure();
        }

        private bool IsValidVertex(IVertex vertex)
        {
            return vertex.AccumulatedCost != double.PositiveInfinity;
        }

        private readonly List<IVertex> neigbourQueue;
    }
}
