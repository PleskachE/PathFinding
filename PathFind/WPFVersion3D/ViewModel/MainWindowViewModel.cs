﻿using Common.Interfaces;
using GraphLib.Coordinates.Infrastructure.Factories;
using GraphLib.Extensions;
using GraphLib.GraphField;
using GraphLib.Graphs;
using GraphLib.Graphs.Factories;
using GraphLib.Graphs.Serialization;
using GraphViewModel;
using Microsoft.Win32;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using WPFVersion3D.Enums;
using WPFVersion3D.Infrastructure;
using WPFVersion3D.Infrastructure.Animations.Interface;
using WPFVersion3D.Model;
using WPFVersion3D.Resources;
using WPFVersion3D.View;

namespace WPFVersion3D.ViewModel
{
    internal class MainWindowViewModel : MainModel, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string graphParametres;
        public override string GraphParametres
        {
            get { return graphParametres; }
            set { graphParametres = value; OnPropertyChanged(); }
        }

        private string statistics;
        public override string PathFindingStatistics
        {
            get { return statistics; }
            set { statistics = value; OnPropertyChanged(); }
        }

        private IGraphField graphField;
        public override IGraphField GraphField
        {
            get { return graphField; }
            set
            {
                graphField = value;
                var field = graphField as GraphField3D;
                field.CenterGraph();
                var currentWindow = Application.Current.MainWindow as MainWindow;
                currentWindow?.GraphField?.Children.Clear();
                currentWindow?.GraphField?.Children.Add(field);
            }
        }

        public ICommand StartPathFindCommand { get; }
        public ICommand CreateNewGraphCommand { get; }
        public ICommand ClearGraphCommand { get; }
        public ICommand SaveGraphCommand { get; }
        public ICommand LoadGraphCommand { get; }
        public ICommand ChangeOpacityCommand { get; }
        public ICommand AnimatedAxisRotateCommand { get; }

        public MainWindowViewModel()
        {
            GraphField = new GraphField3D();
            VertexEventHolder = new Vertex3DEventHolder();
            FieldFactory = new GraphField3DFactory();
            SerializationInfoConverter = (serializationInfo) => new Vertex3D(serializationInfo);

            StartPathFindCommand = new RelayCommand(ExecuteStartPathFindCommand, CanExecuteStartFindPathCommand);
            CreateNewGraphCommand = new RelayCommand(ExecuteCreateNewGraphCommand);
            ClearGraphCommand = new RelayCommand(ExecuteClearGraphCommand, CanExecuteGraphOperation);
            SaveGraphCommand = new RelayCommand(ExecuteSaveGraphCommand, CanExecuteGraphOperation);
            LoadGraphCommand = new RelayCommand(ExecuteLoadGraphCommand);
            ChangeOpacityCommand = new RelayCommand(ExecuteChangeOpacity, CanExecuteGraphOperation);
            AnimatedAxisRotateCommand = new RelayCommand(ExecuteAnimatedAxisRotateCommand);

            Serializer = new GraphSerializer<Graph3D>();

            graphParamFormat = Resource.GraphParamFormat;
        }

        public override void FindPath()
        {
            var viewModel = new PathFindingViewModel(this);

            viewModel.OnPathNotFound += OnPathNotFound;

            PrepareWindow(viewModel, new PathFindWindow());
        }

        public override void CreateNewGraph()
        {
            var vertexFactory = new Vertex3DFactory();
            var coordinateFactory = new Coordinate3DFactory();
            var graphFactory = new GraphFactory<Graph3D>(vertexFactory, coordinateFactory);
            PrepareWindow(new GraphCreatingViewModel(this, graphFactory), new GraphCreateWindow());
        }

        public void StretchAlongXAxis(object sender, RoutedPropertyChangedEventArgs<double>e)
        {
            (graphField as GraphField3D).StretchAlongAxis(Axis.Abscissa, e.NewValue, 1, 0, 0);
        }

        public void StretchAlongYAxis(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            (graphField as GraphField3D).StretchAlongAxis(Axis.Ordinate, e.NewValue, 0, 1, 0);
        }

        public void StretchAlongZAxis(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            (graphField as GraphField3D).StretchAlongAxis(Axis.Applicate, e.NewValue, 0, 0, 1);
        }

        private void ChangeVerticesOpacity()
        {
            PrepareWindow(new OpacityChangeViewModel(), new OpacityChangeWindow());
        }

        public void Dispose()
        {
            OnDispose();
        }

        protected virtual void OnDispose()
        {
            return;
        }

        protected override string GetSavingPath()
        {
            return GetPath(new SaveFileDialog());
        }

        protected override string GetLoadingPath()
        {
            return GetPath(new OpenFileDialog());
        }

        private void ExecuteSaveGraphCommand(object param)
        {
            base.SaveGraph();
        }

        private void ExecuteChangeOpacity(object param)
        {
            ChangeVerticesOpacity();
        }

        private bool CanExecuteStartFindPathCommand(object param)
        {
            return Graph.IsReadyForPathfinding();
        }

        private void ExecuteLoadGraphCommand(object param)
        {
            base.LoadGraph();
        }

        private void ExecuteClearGraphCommand(object param)
        {
            base.ClearGraph();
        }

        private void ExecuteStartPathFindCommand(object param)
        {
            FindPath();
        }

        private void ExecuteCreateNewGraphCommand(object param)
        {
            CreateNewGraph();
        }

        private void PrepareWindow(IViewModel model, Window window)
        {
            model.OnWindowClosed += (sender, args) => window.Close();
            window.DataContext = model;
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.Show();
        }

        private string GetPath(FileDialog dialog)
        {
            return dialog.ShowDialog() == true
                ? dialog.FileName
                : string.Empty;
        }

        private void ExecuteAnimatedAxisRotateCommand(object param)
        {
            var rotator = (IAnimator)param;
            rotator.ApplyAnimation();
        }

        private void OnPathNotFound(string message)
        {
            MessageBox.Show(message);
        }

        private bool CanExecuteGraphOperation(object param)
        {
            return !Graph.IsDefault;
        }
    }
}
