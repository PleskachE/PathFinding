﻿using Common.ValueRanges;
using ConsoleVersion.InputClass;
using GraphLib.Vertex.Interface;
using GraphLib.ViewModel;
using GraphViewModel.Interfaces;
using System;

namespace ConsoleVersion.ViewModel
{
    internal class GraphCreatingViewModel : GraphCreatingModel
    {
        public Tuple<string, string, string> Messages { get; set; }

        public GraphCreatingViewModel(IMainModel model) : base(model)
        {

        }

        public override void CreateGraph(Func<IVertex> generator)
        {
            ObstaclePercent = Input.InputNumber(Messages.Item1,
                Range.ObstaclePercentValueRange.UpperRange,
                Range.ObstaclePercentValueRange.LowerRange);

            Width = Input.InputNumber(Messages.Item2,
                Range.WidthValueRange.UpperRange,
                Range.WidthValueRange.LowerRange);

            Height = Input.InputNumber(Messages.Item3,
                Range.HeightValueRange.UpperRange,
                Range.HeightValueRange.LowerRange);

            base.CreateGraph(generator);
        }
    }
}
