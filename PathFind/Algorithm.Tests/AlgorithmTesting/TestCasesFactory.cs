﻿using Algorithm.Algorithms;
using Algorithm.PathFindingAlgorithms;
using GraphLib.Coordinates.Infrastructure.Factories;
using GraphLib.Graphs;
using GraphLib.Graphs.Abstractions;
using GraphLib.Graphs.Factories;
using GraphLib.Graphs.Factories.Interfaces;
using NUnit.Framework;
using System.Collections;
using System.Linq;

namespace Algorithm.Tests.AlgorithmTesting
{
    internal class TestCasesFactory
    {
        private const int ObstaclePercent = 0;
        private const int Width2D = 25;
        private const int Length2D = 15;
        private const int Width3D = 7;
        private const int Length3D = 8;
        private const int Height3D = 9;

        private static readonly IGraphFactory graph2DFactory;
        private static readonly IGraphFactory graph3DFactory;

        static TestCasesFactory()
        {
            var vertexFactory = new TestVertexFactory();
            var coordinate2DFactory = new Coordinate2DFactory();
            var coordinate3DFactory = new Coordinate3DFactory();

            graph2DFactory = new GraphFactory<Graph2D>(vertexFactory, coordinate2DFactory);
            graph3DFactory = new GraphFactory<Graph3D>(vertexFactory, coordinate3DFactory);
        }

        internal static IEnumerable AlgorithmWithGraphsTestCases
        {
            get
            {
                yield return new TestCaseData(GetGraph2D(), new LeeAlgorithm());
                yield return new TestCaseData(GetGraph2D(), new BestFirstLeeAlgorithm());
                yield return new TestCaseData(GetGraph2D(), new DijkstraAlgorithm());
                yield return new TestCaseData(GetGraph2D(), new AStarAlgorithm());
                yield return new TestCaseData(GetGraph2D(), new AStarModified());
                yield return new TestCaseData(GetGraph2D(), new DepthFirstAlgorithm());
                yield return new TestCaseData(GetGraph2D(), new CostGreedyAlgorithm());
                yield return new TestCaseData(GetGraph2D(), new DistanceGreedyAlgoritm());

                yield return new TestCaseData(GetGraph3D(), new LeeAlgorithm());
                yield return new TestCaseData(GetGraph3D(), new BestFirstLeeAlgorithm());
                yield return new TestCaseData(GetGraph3D(), new DijkstraAlgorithm());
                yield return new TestCaseData(GetGraph3D(), new AStarAlgorithm());
                yield return new TestCaseData(GetGraph3D(), new AStarModified());
                yield return new TestCaseData(GetGraph3D(), new DepthFirstAlgorithm());
                yield return new TestCaseData(GetGraph3D(), new CostGreedyAlgorithm());
                yield return new TestCaseData(GetGraph3D(), new DistanceGreedyAlgoritm());
            }
        }

        internal static IEnumerable AlgorithmsTestCases
        {
            get
            {
                yield return new TestCaseData(new LeeAlgorithm());
                yield return new TestCaseData(new BestFirstLeeAlgorithm());
                yield return new TestCaseData(new DijkstraAlgorithm());
                yield return new TestCaseData(new AStarAlgorithm());
                yield return new TestCaseData(new AStarModified());
                yield return new TestCaseData(new DepthFirstAlgorithm());
                yield return new TestCaseData(new CostGreedyAlgorithm());
                yield return new TestCaseData(new DistanceGreedyAlgoritm());
            }
        }

        private static IGraph GetGraph2D()
        {
            var graph = graph2DFactory.CreateGraph(ObstaclePercent, Width2D, Length2D);
            graph.Start = graph.First();
            graph.End = graph.Last();
            return graph;
        }

        private static IGraph GetGraph3D()
        {
            var graph = graph3DFactory.CreateGraph(ObstaclePercent, Width3D, Length3D, Height3D);
            graph.Start = graph.First();
            graph.End = graph.Last();
            return graph;
        }
    }
}
