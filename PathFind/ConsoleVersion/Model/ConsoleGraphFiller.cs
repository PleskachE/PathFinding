﻿using ConsoleVersion.StatusSetter;
using GraphLibrary.Graph;
using GraphLibrary.Model;
using GraphLibrary.StatusSetter;
using GraphLibrary.Vertex;

namespace ConsoleVersion.Model
{
    internal class ConsoleGraphFiller : AbstractGraphFiller
    {
        protected override void ChargeGraph(AbstractGraph graph, IVertexStatusSetter changer)
        {
            
        }

        protected override void ChargeVertex(IVertex vertex, IVertexStatusSetter changer)
        {
            
        }

        protected override IGraphField GetField()
        {
            return null;
        }

        protected override IVertexStatusSetter GetStatusSetter(AbstractGraph graph)
        {
            return new ConsoleVertexStatusSetter(graph);
        }
    }
}
