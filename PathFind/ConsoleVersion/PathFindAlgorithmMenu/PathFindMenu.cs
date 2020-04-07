﻿using ConsoleVersion.InputClass;
using SearchAlgorythms.Algorithm;
using SearchAlgorythms.DistanceCalculator;
using SearchAlgorythms.Graph;
using SearchAlgorythms.RoleChanger;
using System;
using System.Drawing;
using System.Linq;

namespace ConsoleVersion.PathFindAlgorithmMenu
{
    public class PathFindMenu
    {
        private ConsoleGraph graph = null;
        private readonly ConsoleVertexRoleChanger changer;

        public enum Algorithms
        {
            WidePathFind = 1,
            BestFirstPathFind,
            DijkstraAlgorithm,
            AStarAlgorithm,
            GreedyAlgorithm
        };

        public PathFindMenu(ConsoleGraph graph)
        {
            this.graph = graph;
            changer = new ConsoleVertexRoleChanger(this.graph);
        }

        public void ChooseStart() => ChooseRange("\nStart point: ", changer.SetStartPoint);

        public void ChooseEnd() => ChooseRange("Destination point: ", changer.SetDestinationPoint);

        private void ChooseRange(string message, EventHandler method)
        {
            Console.WriteLine(message);
            Point point = ChoosePoint();
            method(graph[point.X, point.Y], new EventArgs());
        }

        private Point ChoosePoint()
        {
            Point point = Input.InputPoint(graph.Width, graph.Height);
            while (graph[point.X, point.Y].IsObstacle || !graph[point.X, point.Y].IsSimpleVertex ||
                !graph[point.X, point.Y].Neighbours.Any())          
                point = Input.InputPoint(graph.Width, graph.Height);
            return point;
        }

        private string ShowAlgorithms()
        {
            return "\n1. Wide path find\n" +
                    "2. Best first path find\n" +
                    "3. Dijkstra algorithm\n" +
                    "4. A* algorithm\n" +
                    "5. Greedy algorithm\n";
        }

        public IPathFindAlgorithm ChoosePathFindAlgorithm()
        {
            Algorithms algorithms = (Algorithms)Input.InputNumber( ShowAlgorithms() + "Choose algorithm: ", 
                (int)Algorithms.GreedyAlgorithm, (int)Algorithms.WidePathFind);
            switch (algorithms)
            {
                case Algorithms.WidePathFind: return new WidePathFindAlgorithm(graph);
                case Algorithms.BestFirstPathFind: return new BestFirstAlgorithm(graph);
                case Algorithms.DijkstraAlgorithm: return new DijkstraAlgorithm(graph);
                case Algorithms.AStarAlgorithm: return new AStarAlgorithm(graph) { HeuristicFunction = 
                    DistanceCalculator.GetChebyshevDistance };              
                case Algorithms.GreedyAlgorithm: return new GreedyAlgorithm(graph);
                default: return null;
            }
        }
    }
}
