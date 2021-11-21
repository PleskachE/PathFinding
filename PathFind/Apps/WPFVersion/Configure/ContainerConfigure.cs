﻿using Algorithm.Factory;
using Algorithm.Interfaces;
using Algorithm.Realizations.StepRules;
using Autofac;
using Common.Extensions;
using GraphLib.Base;
using GraphLib.Interfaces;
using GraphLib.Interfaces.Factories;
using GraphLib.Realizations.Factories;
using GraphLib.Realizations.Factories.CoordinateFactories;
using GraphLib.Realizations.Factories.GraphAssembles;
using GraphLib.Realizations.Factories.GraphFactories;
using GraphLib.Realizations.Factories.NeighboursCoordinatesFactories;
using GraphLib.Realizations.MeanCosts;
using GraphLib.Realizations.SmoothLevel;
using GraphLib.Serialization;
using GraphLib.Serialization.Interfaces;
using GraphLib.Serialization.Serializers;
using Logging.Interface;
using Logging.Loggers;
using Random.Interface;
using Random.Realizations;
using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using WPFVersion.Model;
using WPFVersion.ViewModel;

namespace WPFVersion.Configure
{
    internal static class ContainerConfigure
    {
        private const string GraphAssembleName = nameof(GraphAssembleName);

        private static SmoothedGraphAssemble RegisterSmoothedGraphAssemble(IComponentContext context)
        {
            var randomGraphAssemble = context.ResolveNamed<IGraphAssemble>(GraphAssembleName);
            var costFactory = context.Resolve<IVertexCostFactory>();
            var meanCost = context.Resolve<IMeanCost>();
            var smoothLevel = context.Resolve<ISmoothLevel>();
            return new SmoothedGraphAssemble(randomGraphAssemble, costFactory, meanCost, smoothLevel);
        }

        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<MainWindowViewModel>().AsSelf().InstancePerLifetimeScope();

            builder.RegisterType<EndPoints>().As<BaseEndPoints>().SingleInstance();
            builder.RegisterType<VertexEventHolder>().As<IVertexEventHolder>().SingleInstance();
            builder.RegisterType<GraphFieldFactory>().As<IGraphFieldFactory>().SingleInstance();

            builder.RegisterType<FileLog>().As<ILog>().SingleInstance();
            builder.RegisterType<MessageBoxLog>().As<ILog>().SingleInstance();
            builder.RegisterType<MailLog>().As<ILog>().SingleInstance();
            builder.RegisterComposite<Logs, ILog>().SingleInstance();

            builder.RegisterType<CryptoRandom>().As<IRandom>().SingleInstance();
            builder.RegisterType<GraphAssemble>().Named<IGraphAssemble>(GraphAssembleName).As<IGraphAssemble>().SingleInstance();
            builder.Register(RegisterSmoothedGraphAssemble).As<IGraphAssemble>().SingleInstance();
            builder.RegisterType<VertexFactory>().As<IVertexFactory>().SingleInstance();
            builder.RegisterType<CostFactory>().As<IVertexCostFactory>().SingleInstance();
            builder.RegisterType<Coordinate2DFactory>().As<ICoordinateFactory>().SingleInstance();
            builder.RegisterType<Graph2DFactory>().As<IGraphFactory>().SingleInstance();
            builder.RegisterType<MooreNeighborhoodFactory>().As<INeighborhoodFactory>().SingleInstance();
            builder.RegisterType<GeometricMeanCost>().As<IMeanCost>().SingleInstance();
            builder.RegisterType<HighSmoothLevel>().As<ISmoothLevel>().SingleInstance();

            builder.RegisterType<GraphSerializationModule>().AsSelf().SingleInstance();
            builder.RegisterType<PathInput>().As<IPathInput>().SingleInstance();
            builder.RegisterType<GraphSerializer>().As<IGraphSerializer>().SingleInstance();
            builder.RegisterDecorator<CryptoGraphSerializer, IGraphSerializer>();
            builder.RegisterType<BinaryFormatter>().As<IFormatter>().SingleInstance();
            builder.RegisterType<VertexFromInfoFactory>().As<IVertexFromInfoFactory>().SingleInstance();

            builder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies())
                .Where(type => type.ImplementsAll(typeof(IAlgorithmFactory)))
                .As<IAlgorithmFactory>().SingleInstance();

            builder.RegisterType<LandscapeStepRule>().As<IStepRule>().SingleInstance();
            builder.RegisterDecorator<WalkStepRule, IStepRule>();

            return builder.Build();
        }
    }
}