﻿using GraphLibrary.DTO;
using GraphLibrary.GraphFactory;
using GraphLibrary.Vertex.Interface;
using WpfVersion.Model.Vertex;

namespace WpfVersion.Model.GraphFactory
{
    internal class WpfGraphInitializer : AbstractGraphInfoInitializer
    {
        public WpfGraphInitializer(VertexInfo[,] info, int placeBetweenVertices) 
            : base(info, placeBetweenVertices)
        {

        }

        protected override IVertex CreateVertex(VertexInfo info) => new WpfVertex(info);
    }
}
