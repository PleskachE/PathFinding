﻿using Algorithm.Base;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WPFVersion3D.Enums;

namespace WPFVersion3D.ViewModel
{
    internal class AlgorithmViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string AlgorithmName { get; }

        private string time;
        public string Time
        {
            get => time;
            set { time = value; OnPropertyChanged(); }
        }

        private int pathLength;
        public int PathLength
        {
            get => pathLength;
            set { pathLength = value; OnPropertyChanged(); }
        }

        private double pathCost;
        public double PathCost
        {
            get => pathCost;
            set { pathCost = value; OnPropertyChanged(); }
        }

        private int visitedVerticesCount;
        public int VisitedVerticesCount
        {
            get => visitedVerticesCount;
            set { visitedVerticesCount = value; OnPropertyChanged(); }
        }

        private AlgorithmStatuses status;
        public AlgorithmStatuses Status
        {
            get => status;
            set { status = value; OnPropertyChanged(); }
        }

        public AlgorithmViewModel(PathfindingAlgorithm algorithm, string algorithmName)
        {
            this.algorithm = algorithm;
            AlgorithmName = algorithmName;
        }

        public void Interrupt()
        {
            algorithm.Interrupt();
        }

        private readonly PathfindingAlgorithm algorithm;
    }
}
