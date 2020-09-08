﻿using GraphLibrary.Common.Constants;
using GraphLibrary.DTO;
using GraphLibrary.Extensions;
using GraphLibrary.Vertex.Interface;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfVersion.Model.Vertex
{
    internal class WpfVertex : Label, IVertex
    {
        public static SolidColorBrush AfterVisitVertexColor { get; set; }
        public static SolidColorBrush PathVertexColor { get; set; }
        public static SolidColorBrush StartVertexColor { get; set; }
        public static SolidColorBrush EndVertexColor { get; set; }

        static WpfVertex()
        {
            AfterVisitVertexColor = new SolidColorBrush(Colors.CadetBlue);
            PathVertexColor = new SolidColorBrush(Colors.Yellow);
            StartVertexColor = new SolidColorBrush(Colors.Green);
            EndVertexColor = new SolidColorBrush(Colors.Red);
        }

        public WpfVertex() : base()
        {
            this.Initialize();
            Width = Height = VertexSize.VERTEX_SIZE;
            FontSize = 12;
            Template = (ControlTemplate)TryFindResource("vertexTemplate");
        }

        public WpfVertex(VertexInfo info) : this() => this.Initialize(info);

        public bool IsEnd { get; set; }
        public bool IsObstacle { get; set; }
        public bool IsStart { get; set; }
        public bool IsVisited { get; set; }

        public int Cost 
        {
            get { return int.Parse(Content.ToString()); }
            set { Content = value.ToString(); }
        }

        public List<IVertex> Neighbours { get; set; }
        public IVertex ParentVertex { get; set; }
        public double AccumulatedCost { get; set; }
        public Point Location { get; set; }

        public VertexInfo Info => new VertexInfo(this);

        public void MarkAsEnd() => Background = EndVertexColor;

        public void MarkAsObstacle()
        {
            this.WashVertex();
            Background = new SolidColorBrush(Colors.Black);
        }

        public void MarkAsPath() => Background = PathVertexColor;

        public void MarkAsSimpleVertex()
        {
            if (!IsObstacle)
                Background = new SolidColorBrush(Colors.White);
        }

        public void MarkAsStart() => Background = StartVertexColor;

        public void MarkAsVisited() => Background = AfterVisitVertexColor;
    }
}
