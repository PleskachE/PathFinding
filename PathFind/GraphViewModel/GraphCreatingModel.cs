﻿using Common.Extensions.EnumerableExtensions;
using GraphLib.Interfaces.Factories;
using Logging.Interface;
using System.Collections.Generic;

namespace GraphLib.ViewModel
{
    public abstract class GraphCreatingModel
    {
        public int Width { get; set; }

        public int Length { get; set; }

        public int ObstaclePercent { get; set; }

        public IDictionary<string, IGraphAssemble> GraphAssembles { get; }

        public virtual IGraphAssemble SelectedGraphAssemble { get; set; }

        protected GraphCreatingModel(ILog log, IEnumerable<IGraphAssemble> graphAssembles)
        {
            this.log = log;
            GraphAssembles = graphAssembles.ToNameInstanceDictionary();
        }

        public abstract void CreateGraph();

        protected readonly ILog log;
    }
}
