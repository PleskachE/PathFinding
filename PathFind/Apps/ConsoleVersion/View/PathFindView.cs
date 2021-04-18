﻿using ConsoleVersion.Resource;
using ConsoleVersion.View.Interface;
using ConsoleVersion.ViewModel;
using System.Linq;

namespace ConsoleVersion.View
{
    internal sealed class PathFindView : IView
    {
        public PathFindingViewModel Model { get; }

        public PathFindView(PathFindingViewModel model)
        {
            Model = model;
            var algorithmMenu = Menu.CreateMenu(model.AlgorithmKeys.ToArray());
            Model.AlgorithmKeyInputMessage = algorithmMenu + Resources.ChooseAlrorithm;
            Model.StartVertexInputMessage = "\n" + Resources.StartVertexPointInputMsg;
            Model.EndVertexInputMessage = Resources.EndVertexCoordinateInputMsg;
        }

        public void Start()
        {
            Model.FindPath();
        }
    }
}
