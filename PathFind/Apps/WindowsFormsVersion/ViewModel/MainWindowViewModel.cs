﻿using Algorithm.Factory;
using Common.Interface;
using GalaSoft.MvvmLight.Messaging;
using GraphLib.Base;
using GraphLib.Extensions;
using GraphLib.Interfaces;
using GraphLib.Interfaces.Factories;
using GraphLib.Realizations.Graphs;
using GraphLib.Serialization;
using GraphViewModel;
using GraphViewModel.Interfaces;
using Logging.Interface;
using NullObject.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using WindowsFormsVersion.EventArguments;
using WindowsFormsVersion.EventHandlers;
using WindowsFormsVersion.Extensions;
using WindowsFormsVersion.Forms;
using WindowsFormsVersion.Messeges;
using WindowsFormsVersion.View;

namespace WindowsFormsVersion.ViewModel
{
    internal class MainWindowViewModel : MainModel, IMainModel, IModel, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event StatisticsChangedEventHandler StatisticsChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool IsPathfindingStarted { private get; set; }

        private string graphParametres;
        public override string GraphParametres
        {
            get => graphParametres;
            set { graphParametres = value; OnPropertyChanged(); }
        }

        private string statistics;
        public string PathFindingStatistics
        {
            get => statistics;
            set
            {
                statistics = value;
                StatisticsChanged?.Invoke(this, new StatisticsChangedEventArgs(value));
            }
        }

        private IGraphField graphField;
        public override IGraphField GraphField
        {
            get => graphField;
            set
            {
                graphField = value;
                if (Graph is Graph2D graph2D && graphField is WinFormsGraphField field)
                {
                    int width = (graph2D.Width + Constants.VertexSize) * Constants.VertexSize;
                    int height = (graph2D.Length + Constants.VertexSize) * Constants.VertexSize;
                    field.Size = new Size(width, height);
                    MainWindow.Controls.RemoveBy(ctrl => ctrl.IsGraphField());
                    MainWindow.Controls.Add(field);
                }
            }
        }

        public MainWindow MainWindow { get; set; }

        public MainWindowViewModel(IGraphFieldFactory fieldFactory, IVertexEventHolder eventHolder, GraphSerializationModule serializationModule,
            IEnumerable<IGraphAssemble> graphAssembles, BaseEndPoints endPoints, IEnumerable<IAlgorithmFactory> algorithmFactories, ILog log)
            : base(fieldFactory, eventHolder, serializationModule, graphAssembles, endPoints, algorithmFactories, log)
        {
            Messenger.Default.Register<AlgorithmStatusMessage>(this, MessageTokens.MainModel, SetAlgorithmStatus);
            Messenger.Default.Register<UpdateStatisticsMessage>(this, MessageTokens.MainModel, SetStatisticsMessage);
            Messenger.Default.Register<GraphCreatedMessage>(this, MessageTokens.MainModel, SetGraph);
        }

        private void SetAlgorithmStatus(AlgorithmStatusMessage message)
        {
            IsPathfindingStarted = message.IsAlgorithmStarted;
        }

        private void SetStatisticsMessage(UpdateStatisticsMessage message)
        {
            PathFindingStatistics = message.Statistics;
        }

        public override void FindPath()
        {
            if (CanStartPathFinding())
            {
                try
                {
                    var model = new PathFindingViewModel(log, Graph, endPoints, algorithmFactories);
                    var form = new PathFindingWindow(model);
                    PrepareWindow(model, form);
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
            }
        }

        public override void CreateNewGraph()
        {
            if (!IsPathfindingStarted)
            {
                try
                {
                    var model = new GraphCreatingViewModel(log, graphAssembles);
                    var form = new GraphCreatingWindow(model);
                    PrepareWindow(model, form);
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
            }
        }

        public void SaveGraph(object sender, EventArgs e)
        {
            if (!Graph.IsNull())
            {
                base.SaveGraph();
            }
        }

        public void LoadGraph(object sender, EventArgs e)
        {
            if (!IsPathfindingStarted)
            {
                base.LoadGraph();
            }
        }

        public void ClearGraph(object sender, EventArgs e)
        {
            if (!IsPathfindingStarted)
            {
                base.ClearGraph();
                PathFindingStatistics = string.Empty;
            }
        }

        public override void ConnectNewGraph(IGraph graph)
        {
            base.ConnectNewGraph(graph);
            PathFindingStatistics = string.Empty;
        }

        public void MakeWeighted(object sender, EventArgs e)
        {
            if (!IsPathfindingStarted)
            {
                Graph.ToWeighted();
            }
        }

        public void MakeUnweighted(object sender, EventArgs e)
        {
            if (!IsPathfindingStarted)
            {
                Graph.ToUnweighted();
            }
        }

        public void StartPathFind(object sender, EventArgs e)
        {
            FindPath();
        }

        public void CreateNewGraph(object sender, EventArgs e)
        {
            CreateNewGraph();
        }

        private void PrepareWindow(IViewModel model, Form window)
        {
            model.WindowClosed += (sender, args) => window.Close();
            window.StartPosition = FormStartPosition.CenterScreen;
            window.Show();
        }

        private void SetGraph(GraphCreatedMessage message)
        {
            ConnectNewGraph(message.Graph);
        }

        private bool CanStartPathFinding()
        {
            return !endPoints.HasIsolators() && !IsPathfindingStarted;
        }
    }
}
