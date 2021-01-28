﻿using Algorithm.Algorithms.Abstractions;
using Algorithm.NUnitTests.AlgorithmTesting;
using GraphLib.Graphs;
using GraphLib.Graphs.Abstractions;
using GraphLib.Graphs.Infrastructure;
using NUnit.Framework;
using System.Linq;

namespace Algorithm.NUnitTests
{
    [TestFixture]
    internal class AlgorithmTests
    {
        [TestCaseSource(typeof(TestCasesFactory), 
            nameof(TestCasesFactory.AlgorithmTestCases))]
        public void FindPath_NotNullGraph_Success(IGraph graph, 
            IAlgorithm algorithm)
        {
            graph.Start = graph.First();
            graph.End = graph.Last();
            algorithm.Graph = graph;

            algorithm.FindPath();
            var path = new GraphPath(graph);

            Assert.IsTrue(path.IsExtracted);
        }

        [TestCaseSource(typeof(TestCasesFactory), 
            nameof(TestCasesFactory.Algorithms))]
        public void FindPath_NullGraph_Failed(IAlgorithm algorithm)
        {
            IGraph graph = new NullGraph();
            algorithm.Graph = graph;

            algorithm.FindPath();
            var path = new GraphPath(graph);

            Assert.IsFalse(path.IsExtracted);
        }
    }
}
