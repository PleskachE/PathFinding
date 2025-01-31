﻿using Autofac;
using Common.Extensions.EnumerableExtensions;
using GraphLib.Extensions;
using GraphLib.Interfaces;
using GraphLib.Realizations.Coordinates;
using GraphLib.Realizations.Graphs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Media3D;
using WPFVersion3D.Axes;
using WPFVersion3D.DependencyInjection;

namespace WPFVersion3D.Model
{
    internal sealed class GraphField3D : ModelVisual3D, IGraphField
    {
        public IReadOnlyCollection<IVertex> Vertices { get; }
        public double DistanceBetweenVerticesAtXAxis { get; set; } = default;
        public double DistanceBetweenVerticesAtYAxis { get; set; } = default;
        public double DistanceBetweenVerticesAtZAxis { get; set; } = default;

        public GraphField3D(Graph3D graph)
        {
            CoordinateSystem = DI.Container
                .Resolve<IEnumerable<IAxis>>()
                .OrderBy(axis => axis.Order)
                .ToArray();
            DimensionSizes = graph.DimensionsSizes;
            Vertices = graph.Vertices;
            Vertices.ForEach(Add);
        }

        public GraphField3D()
        {

        }

        private void Add(IVertex vertex)
        {
            if (vertex is Vertex3D vertex3D && vertex.Position is Coordinate3D)
            {
                LocateVertex(CoordinateSystem, vertex3D);
                Children.Add(vertex3D);
            }
        }

        public void CenterGraph(params double[] offsets)
        {
            var centerOffsets = CoordinateSystem.Select(axis => CalculateAxisOffset(offsets, axis)).ToArray();
            Vertices.ForEach(vertex => LocateVertex(CoordinateSystem, (Vertex3D)vertex, centerOffsets));
        }

        public void StretchAlongAxis(IAxis axis, double distanceBetween, params double[] additionalOffset)
        {
            axis.SetDistanceBeetween(distanceBetween, this);
            StretchAlongAxis(axis);
            CenterGraph(distanceBetween == 0 ? Array.Empty<double>() : additionalOffset);
        }

        private double CalculateAxisOffset(double[] additionalOffset, IAxis axis)
        {
            return (additionalOffset.ElementAtOrDefault(axis.Order) - DimensionSizes[axis.Order]) * GetAdjustedVertexSize(axis) / 2;
        }

        private void StretchAlongAxis(IAxis axis)
        {
            Vertices.ForEach(vertex => LocateVertex(axis, (Vertex3D)vertex));
        }

        private void LocateVertex(IAxis[] axes, Vertex3D vertex, params double[] additionalOffset)
        {
            axes.ForEach(axis => LocateVertex(axis, vertex, additionalOffset));
        }

        private void LocateVertex(IAxis axis, Vertex3D vertex, params double[] additionalOffset)
        {
            var coordinates = vertex.GetCoordinates();
            double offset = GetAdjustedVertexSize(axis) * coordinates[axis.Order]
                + additionalOffset.ElementAtOrDefault(axis.Order);
            axis.Offset(vertex.Transform as TranslateTransform3D, offset);
        }

        private double GetAdjustedVertexSize(IAxis axis) => Constants.InitialVertexSize + DistancesBetween[axis.Order];

        private int[] DimensionSizes { get; }
        private IAxis[] CoordinateSystem { get; }

        private double[] DistancesBetween => new[]
        {
            DistanceBetweenVerticesAtZAxis,
            DistanceBetweenVerticesAtYAxis,
            DistanceBetweenVerticesAtXAxis
        };
    }
}