﻿using Common.Extensions.EnumerableExtensions;
using GalaSoft.MvvmLight.Messaging;
using GraphViewModel.Interfaces;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using WPFVersion.Enums;
using WPFVersion.Extensions;
using WPFVersion.Infrastructure;
using WPFVersion.Messages;
using WPFVersion.Model;

namespace WPFVersion.ViewModel
{
    internal class AlgorithmStatisticsViewModel : INotifyPropertyChanged, IModel
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ICommand InterruptSelelctedAlgorithmCommand { get; }
        public ICommand RemoveSelelctedAlgorithmCommand { get; }

        private AlgorithmViewModel selected;
        public AlgorithmViewModel SelectedAlgorithm
        {
            get => selected;
            set
            {
                selected = value;
                if (selected?.Algorithm != null && IsAllFinished)
                {
                    visualizationModel?.Visualize(selected.Algorithm);
                }
            }
        }

        public ObservableCollection<AlgorithmViewModel> Statistics { get; set; }

        public AlgorithmStatisticsViewModel()
        {
            Statistics = new ObservableCollection<AlgorithmViewModel>();
            InterruptSelelctedAlgorithmCommand = new RelayCommand(ExecuteInterruptSelectedAlgorithmCommand, CanExecuteInterruptSelectedAlgorithmCommand);
            RemoveSelelctedAlgorithmCommand = new RelayCommand(ExecuteRemoveFromStatisticsCommand, CanExecuteRemoveFromStatisticsCommand);
            Messenger.Default.Register<AlgorithmStartedMessage>(this, MessageTokens.AlgorithmStatisticsModel, OnAlgorithmStarted);
            Messenger.Default.Register<UpdateStatisticsMessage>(this, MessageTokens.AlgorithmStatisticsModel, UpdateAlgorithmStatistics);
            Messenger.Default.Register<InterruptAllAlgorithmsMessage>(this, MessageTokens.AlgorithmStatisticsModel, OnAllAlgorithmInterrupted);
            Messenger.Default.Register<ClearStatisticsMessage>(this, MessageTokens.AlgorithmStatisticsModel, OnClearStatistics);
            Messenger.Default.Register<AlgorithmStatusMessage>(this, MessageTokens.AlgorithmStatisticsModel, SetAlgorithmStatistics);
            Messenger.Default.Register<GraphCreatedMessage>(this, MessageTokens.AlgorithmStatisticsModel, NewGraphCreated);
        }

        private void SetAlgorithmStatistics(AlgorithmStatusMessage message)
        {
            if (Statistics[message.Index].Status != AlgorithmStatus.Interrupted)
            {
                Statistics[message.Index].Status = message.Status;
                SendIsAllAlgorithmsFinishedMessage();
            }
        }

        private void OnAlgorithmStarted(AlgorithmStartedMessage message)
        {
            int index = Statistics.Count;
            var msg = new AlgorithmIndexMessage(index);
            Messenger.Default.Send(msg, MessageTokens.PathfindingModel);
            var viewModel = new AlgorithmViewModel(message, index);
            Application.Current.Dispatcher.Invoke(() => Statistics.Add(viewModel));
            SendIsAllAlgorithmsFinishedMessage();
        }

        private void UpdateAlgorithmStatistics(UpdateStatisticsMessage message)
        {
            Application.Current.Dispatcher.Invoke(() => Statistics[message.Index].RecieveMessage(message));
        }

        private void OnAllAlgorithmInterrupted(InterruptAllAlgorithmsMessage message)
        {
            Statistics.ForEach(stat => stat.TryInterrupt());
        }

        private void NewGraphCreated(GraphCreatedMessage message)
        {
            if (visualizationModel != null)
            {
                visualizationModel.Dispose();
            }
            visualizationModel = new PathfindingVisualizationModel(message.Graph);
        }

        private void OnClearStatistics(ClearStatisticsMessage message)
        {
            Statistics.Clear();
            visualizationModel?.Clear();
            SendIsAllAlgorithmsFinishedMessage();
        }

        private void ExecuteRemoveFromStatisticsCommand(object param)
        {
            visualizationModel?.Remove(SelectedAlgorithm.Algorithm);
            Statistics.Remove(SelectedAlgorithm);
            SendIsAllAlgorithmsFinishedMessage();
        }

        private void ExecuteInterruptSelectedAlgorithmCommand(object param)
        {
            SelectedAlgorithm?.TryInterrupt();
        }

        private bool CanExecuteRemoveFromStatisticsCommand(object param)
        {
            return IsAllFinished;
        }

        private bool CanExecuteInterruptSelectedAlgorithmCommand(object param)
        {
            return SelectedAlgorithm?.IsStarted() == true;
        }

        private void SendIsAllAlgorithmsFinishedMessage()
        {
            var message = new IsAllAlgorithmsFinishedMessage(IsAllFinished);
            Messenger.Default.Send(message, MessageTokens.MainModel);
        }

        private bool IsAllFinished => Statistics.All(stat => !stat.IsStarted());

        private PathfindingVisualizationModel visualizationModel;
    }
}