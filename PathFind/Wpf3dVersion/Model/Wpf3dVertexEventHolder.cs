﻿using GraphLib.EventHolder;
using GraphLib.Vertex.Interface;
using System;

namespace Wpf3dVersion.Model
{
    public class Wpf3dVertexEventHolder : BaseVertexEventHolder
    {
        protected override int GetWheelDelta(EventArgs e)
        {
            return 0;
        }

        protected override void SubscribeToEvents(IVertex vertex)
        {
            (vertex as WpfVertex3D).MouseLeftButtonDown += ChooseExtremeVertices;
            (vertex as WpfVertex3D).MouseRightButtonDown += Reverse;
            (vertex as WpfVertex3D).MouseWheel += ChangeVertexCost;
        }
    }
}
