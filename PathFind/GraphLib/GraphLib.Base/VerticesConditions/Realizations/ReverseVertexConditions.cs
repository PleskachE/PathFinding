﻿using GraphLib.Base.EndPointsConditions.Interfaces;
using GraphLib.Base.VertexCondition.Interface;
using GraphLib.Base.VertexCondition.Realizations.VertexRevertCondition;
using GraphLib.Interfaces;
using NullObject.Extensions;
using System.Linq;

namespace GraphLib.Base.VerticesConditions.Realizations
{
    internal sealed class ReverseVertexConditions : IVerticesConditions
    {
        public ReverseVertexConditions()
        {
            conditions = new IVertexCondition[]
            {
                new SetVertexAsObstacleCondition(),
                new SetVertexAsRegularCondition()
            };
        }

        public void ExecuteTheFirstTrue(IVertex vertex)
        {
            if (!vertex.IsNull())
            {
                conditions
                  .FirstOrDefault(condition => condition.IsTrue(vertex))
                  ?.Execute(vertex);
            }
        }

        public void ResetAllExecutings()
        {

        }

        private readonly IVertexCondition[] conditions;
    }
}
