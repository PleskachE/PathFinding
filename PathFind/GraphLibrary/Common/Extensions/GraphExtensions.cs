﻿using GraphLibrary.Collection;
using GraphLibrary.Extensions;
using GraphLibrary.Vertex;
using System.Linq;

namespace GraphLibrary.Common.Extensions
{
    public static class GraphExtensions
    {
        public static string GetFormattedInfo(this Graph graph, string format)
        {
            return string.Format(format, graph.Width, graph.Height,
               graph.ObstaclePercent, graph.ObstacleNumber, graph.Size);
        }

        public static int GetNumberOfVisitedVertices(this Graph graph)
        {
            return graph.GetArray().Cast<IVertex>().Count(vertex => vertex.IsVisited);
        }

        public static void Refresh(this Graph graph)
        {
            graph.End = null;
            graph.Start = null;
            foreach(IVertex vertex in graph)
            {
                if (!vertex.IsObstacle)
                    vertex.SetToDefault();
            }
        }
    }
}
