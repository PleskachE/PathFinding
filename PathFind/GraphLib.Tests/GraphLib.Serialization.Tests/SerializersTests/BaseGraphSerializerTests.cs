﻿using Autofac;
using GraphLib.Interfaces;
using GraphLib.Interfaces.Factories;
using GraphLib.Serialization.Interfaces;
using NUnit.Framework;
using System.IO;

namespace GraphLib.Serialization.Tests.SerializersTests
{
    [TestFixture]
    public abstract class BaseGraphSerializerTests
    {
        protected abstract IContainer Container { get; }

        [TestCase(11, new[] { 8, 9, 7 })]
        [TestCase(22, new[] { 15, 15 })]
        [TestCase(66, new[] { 4, 3, 4, 2 })]
        [TestCase(44, new[] { 4, 3, 4, 2, 2 })]
        public void SaveGraph_LoadGraph_ReturnsEqualGraph(int obstaclePercent, int[] graphParams)
        {
            using (var scope = Container.BeginLifetimeScope())
            {
                var assembler = scope.Resolve<IGraphAssemble>();
                var serializer = scope.Resolve<IGraphSerializer>();
                IGraph deserialized;
                var graph = assembler.AssembleGraph(obstaclePercent, graphParams);

                using (var stream = new MemoryStream())
                {
                    serializer.SaveGraph(graph, stream);
                    stream.Seek(0, SeekOrigin.Begin);
                    deserialized = serializer.LoadGraph(stream);
                }

                Assert.AreEqual(graph, deserialized);
                Assert.AreNotSame(graph, deserialized);
            }
        }
    }
}
