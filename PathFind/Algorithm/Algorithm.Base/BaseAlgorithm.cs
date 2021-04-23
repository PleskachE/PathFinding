﻿using Algorithm.Base.CompanionClasses;
using Algorithm.Common;
using Algorithm.Infrastructure.EventArguments;
using Algorithm.Infrastructure.Handlers;
using Algorithm.Interfaces;
using Common.Extensions;
using GraphLib.Extensions;
using GraphLib.Interfaces;
using System;
using Algorithm.Сompanions;

namespace Algorithm.Base
{
    public abstract class BaseAlgorithm : IAlgorithm
    {
        public static IAlgorithm Default => new DefaultAlgorithm();

        public event AlgorithmEventHandler OnStarted;
        public event AlgorithmEventHandler OnVertexVisited;
        public event AlgorithmEventHandler OnFinished;
        public event AlgorithmEventHandler OnVertexEnqueued;
        public event EventHandler OnInterrupted;

        public abstract IGraphPath FindPath();

        public virtual void Interrupt()
        {
            isInterruptRequested = true;
            var args = CreateEventArgs(CurrentVertex);
            OnInterrupted?.Invoke(this, args);
        }

        protected BaseAlgorithm(IGraph graph, IEndPoints endPoints) 
            : this()
        {
            this.graph = graph;
            this.endPoints = new EndPoints(endPoints);
        }

        protected virtual void Reset()
        {
            OnStarted = null;
            OnFinished = null;
            OnVertexEnqueued = null;
            OnVertexVisited = null;
            OnInterrupted = null;
            visitedVertices.Clear();
            parentVertices.Clear();
            isInterruptRequested = false;
        }

        protected IVertex CurrentVertex { get; set; }

        protected abstract IVertex NextVertex { get; }

        protected virtual bool IsDestination()
        {
            return CurrentVertex.IsEqual(endPoints.End)
                   || CurrentVertex.IsDefault()
                   || isInterruptRequested;
        }

        protected void RaiseOnAlgorithmStartedEvent(AlgorithmEventArgs e)
        {
            OnStarted?.Invoke(this, e);
        }

        protected void RaiseOnAlgorithmFinishedEvent(AlgorithmEventArgs e)
        {
            OnFinished?.Invoke(this, e);
        }

        protected void RaiseOnVertexVisitedEvent(AlgorithmEventArgs e)
        {
            OnVertexVisited?.Invoke(this, e);
        }

        protected void RaiseOnVertexEnqueuedEvent(AlgorithmEventArgs e)
        {
            OnVertexEnqueued?.Invoke(this, e);
        }

        protected virtual void PrepareForPathfinding()
        {
            if (graph.Contains(endPoints))
            {
                CurrentVertex = endPoints.Start;
                visitedVertices.Add(CurrentVertex);
                var args = CreateEventArgs(CurrentVertex);
                RaiseOnAlgorithmStartedEvent(args);
                return;
            }

            throw new ArgumentException($"{nameof(endPoints)} don't belong to {nameof(graph)}");
        }

        protected virtual void CompletePathfinding()
        {
            var args = CreateEventArgs(CurrentVertex);
            RaiseOnAlgorithmFinishedEvent(args);
        }

        protected AlgorithmEventArgs CreateEventArgs(IVertex vertex)
        {
            return new AlgorithmEventArgs(visitedVertices.Count, endPoints, vertex);
        }

        protected ICoordinate Position(IVertex vertex)
        {
            return vertex.Position;
        }

        public void Dispose()
        {
            Reset();
        }

        protected readonly VisitedVertices visitedVertices;
        protected readonly ParentVertices parentVertices;
        protected IAccumulatedCosts accumulatedCosts;

        protected readonly IGraph graph;
        protected readonly EndPoints endPoints;

        private BaseAlgorithm()
        {
            visitedVertices = new VisitedVertices();
            parentVertices = new ParentVertices();
        }

        private bool isInterruptRequested;
    }
}