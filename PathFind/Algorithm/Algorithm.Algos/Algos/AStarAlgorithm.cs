﻿using Algorithm.Interfaces;
using Algorithm.Realizations.Heuristic;
using Algorithm.Realizations.StepRules;
using Algorithm.Сompanions;
using Algorithm.Сompanions.Interface;
using GraphLib.Extensions;
using GraphLib.Interfaces;
using Interruptable.Interface;
using System;
using System.Collections.Generic;

namespace Algorithm.Algos.Algos
{
    /// <summary>
    /// A realization of the A* algorithm
    /// </summary>
    /// <remarks><see cref="https://en.wikipedia.org/wiki/A*_search_algorithm"/></remarks>
    public class AStarAlgorithm : DijkstraAlgorithm,
        IAlgorithm, IInterruptableProcess, IInterruptable, IDisposable
    {
        public AStarAlgorithm(IGraph graph, IIntermediateEndPoints endPoints)
            : this(graph, endPoints, new DefaultStepRule(), new ChebyshevDistance())
        {

        }

        public AStarAlgorithm(IGraph graph, IIntermediateEndPoints endPoints,
            IStepRule stepRule, IHeuristic function)
            : base(graph, endPoints, stepRule)
        {
            heuristic = function;
        }

        protected override void Reset()
        {
            base.Reset();
            heuristicAccumulatedCosts?.Clear();
        }

        protected override void PrepareForLocalPathfinding(IEnumerable<IVertex> vertices)
        {
            base.PrepareForLocalPathfinding(vertices);
            heuristicAccumulatedCosts = new AccumulatedCosts(vertices, double.PositiveInfinity);
            double value = heuristic.Calculate(CurrentEndPoints.Source, CurrentEndPoints.Target);
            heuristicAccumulatedCosts.Reevaluate(CurrentEndPoints.Source, value);
        }

        protected override double OrderFunction(IVertex vertex)
        {
            return heuristicAccumulatedCosts.GetAccumulatedCost(vertex);
        }

        protected override void Reevaluate(IVertex vertex, double value)
        {
            base.Reevaluate(vertex, value);
            value += heuristic.Calculate(vertex, CurrentEndPoints.Target);
            heuristicAccumulatedCosts.Reevaluate(vertex, value);
        }

        private IAccumulatedCosts heuristicAccumulatedCosts;
        protected readonly IHeuristic heuristic;
    }
}
