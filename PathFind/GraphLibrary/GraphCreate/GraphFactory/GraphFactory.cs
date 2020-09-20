﻿using System;
using GraphLibrary.Extensions.SystemTypeExtensions;
using GraphLibrary.Graphs;
using GraphLibrary.Vertex.Interface;
using GraphLibrary.VertexBinding;
using GraphLibrary.GraphCreate.GraphFactory.Interface;
using GraphLibrary.Graphs.Interface;

namespace GraphLibrary.GraphFactory
{
    public class GraphFactory : IGraphFactory
    {
        public GraphFactory(GraphParametres parametres)
        {
            this.parametres = parametres;
        }

        static GraphFactory()
        {
            rand = new Random();
        }


        public IGraph GetGraph(Func<IVertex> generator)
        {
            graph = new Graph(parametres.Width, parametres.Height);

            IVertex InitializeVertex(IVertex vertex)
            {                
                var indices = graph.GetIndices(vertex);
                vertex = generator();
                vertex.Cost = rand.GetRandomValueCost();
                if (rand.IsObstacleChance(parametres.ObstaclePercent))
                    vertex.MarkAsObstacle();
                vertex.Position = indices;
                return vertex;
            }

            graph.Array.Apply(InitializeVertex);
            VertexBinder.ConnectVertices(graph);

            return graph;
        }

        private static readonly Random rand;

        private IGraph graph;
        private readonly GraphParametres parametres;
    }
}
