﻿using GraphLib.Base.EndPoints.EndPointsInspection;
using GraphLib.Interfaces;

namespace GraphLib.Base.EndPoints.Commands.EndPointsCommands
{
    internal sealed class ReplaceIntermediateVertexCondition
        : BaseIntermediateEndPointsInspection, IVertexCommand
    {
        public ReplaceIntermediateVertexCondition(BaseEndPoints endPoints) : base(endPoints)
        {

        }

        public void Execute(IVertex vertex)
        {
            if (endPoints.markedToReplaceIntermediates.Count > 0)
            {
                var toReplace = endPoints.markedToReplaceIntermediates.Dequeue();
                int toReplaceIndex = endPoints.intermediates.IndexOf(toReplace);
                if (endPoints.intermediates.Remove(toReplace) && toReplaceIndex > -1)
                {
                    (toReplace as IVisualizable)?.VisualizeAsRegular();
                    endPoints.intermediates.Insert(toReplaceIndex, vertex);
                    (vertex as IVisualizable)?.VisualizeAsIntermediate();
                }
            }
        }

        public bool IsTrue(IVertex vertex)
        {
            return endPoints.markedToReplaceIntermediates.Count > 0
                && !endPoints.IsEndPoint(vertex);
        }
    }
}
