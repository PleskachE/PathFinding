﻿using Common.Interfaces;
using GraphLib.ViewModel;
using GraphViewModel.Interfaces;
using System;
using WindowsFormsVersion.Model;

namespace WindowsFormsVersion.ViewModel
{
    internal class GraphCreatingViewModel : GraphCreatingModel, IViewModel
    {
        public event EventHandler OnWindowClosed;

        public GraphCreatingViewModel(IMainModel model) : base(model)
        {

        }

        public void CreateGraph(object sender, EventArgs e)
        {
            CreateGraph(() => new Vertex());
            OnWindowClosed?.Invoke(this, new EventArgs());
            OnWindowClosed = null;
        }

        public void CancelCreateGraph(object sender, EventArgs e)
        {
            OnWindowClosed?.Invoke(this, new EventArgs());
            OnWindowClosed = null;
        }
    }
}
