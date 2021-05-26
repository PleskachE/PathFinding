﻿using GraphLib.Realizations.Factories.NeighboursCoordinatesFactories;
using GraphLib.Realizations.Factories.GraphAssembles;

namespace GraphLib.TestRealizations.TestFactories
{
    public sealed class TestGraphAssemble : GraphAssemble
    {
        public TestGraphAssemble()
            : base(new TestVertexFactory(),
                  new TestCoordinateFactory(),
                  new TestGraphFactory(),
                  new TestCostFactory(),
                  new AroundNeighboursCoordinatesFactory())
        {

        }
    }
}
