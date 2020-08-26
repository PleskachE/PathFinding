﻿using ConsoleVersion.Forms;
using ConsoleVersion.InputClass;
using ConsoleVersion.RoleChanger;
using GraphLibrary.AlgorithmEnum;
using GraphLibrary.Extensions;
using GraphLibrary.Model;
using System;
using System.Drawing;

namespace ConsoleVersion.ViewModel
{
    public class PathFindViewModel : AbstractPathFindModel
    {
        private readonly ConsoleVertexRoleChanger changer;
        public string AlgoList { get; set; }

        public PathFindViewModel(IMainModel model) : base(model)
        {
            changer = new ConsoleVertexRoleChanger(model.Graph);
            badResultMessage = Res.BadResultMsg;
        }

        public override void PathFind()
        {
            model.Graph.Refresh();
            GraphShower.DisplayGraph(model);
            ChooseStart();
            ChooseEnd();
            GraphShower.DisplayGraph(model);
            Algorithm = GetAlgorithmEnum();
            base.PathFind();
        }

        protected override void FindPreparations()
        {
            pathFindAlgorythm.PauseEvent = () => { };
        }

        protected override void ShowMessage(string message)
        {
            GraphShower.DisplayGraph(model);
            Console.WriteLine(message);
            Console.ReadLine();
        }

        private Algorithms GetAlgorithmEnum()
        {
            return (Algorithms)Input.InputNumber(AlgoList + Res.ChooseAlrorithm,
                (int)Algorithms.ValueGreedyAlgorithm, (int)Algorithms.WidePathFind);
        }

        private void ChooseStart()
        {
            ChooseRange("\n" + Res.StartPoint, changer.SetStartPoint);
        }

        private void ChooseEnd()
        {
            ChooseRange(Res.DestinationPoint, changer.SetDestinationPoint);
        }

        private void ChooseRange(string message, EventHandler method)
        {
            Console.WriteLine(message);
            Point point = ChoosePoint();
            method(graph[point.X, point.Y], new EventArgs());
        }

        private Point ChoosePoint()
        {
            Point point = Input.InputPoint(graph.Width, graph.Height);
            while (!graph[point.X, point.Y].IsValidToBeRange())
                point = Input.InputPoint(graph.Width, graph.Height);
            return point;
        }
    }
}
