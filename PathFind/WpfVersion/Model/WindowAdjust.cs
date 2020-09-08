﻿using GraphLibrary.Common.Constants;
using System.Windows;
using GraphLibrary.Graphs;

namespace WpfVersion.Model
{
    internal static class WindowAdjust
    {
        public static void Adjust(Graph graph)
        {
            if (graph == null)
                return;
            Application.Current.MainWindow.Width = (graph.Width + 1) * VertexSize.SIZE_BETWEEN_VERTICES + VertexSize.SIZE_BETWEEN_VERTICES;
            Application.Current.MainWindow.Height = (1 + graph.Height) * VertexSize.SIZE_BETWEEN_VERTICES +
                Application.Current.MainWindow.DesiredSize.Height;
        }
    }
}
