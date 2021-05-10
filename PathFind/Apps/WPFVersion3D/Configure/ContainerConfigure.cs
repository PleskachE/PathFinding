﻿using Algorithm.Realizations;
using AssembleClassesLib.Interface;
using AssembleClassesLib.Realizations;
using AssembleClassesLib.Realizations.LoadMethods;
using Autofac;
using Common.Interface;
using Common.Logging;
using GraphLib.Base;
using GraphLib.Interfaces;
using GraphLib.Interfaces.Factories;
using GraphLib.Realizations.Factories;
using GraphLib.Serialization;
using GraphLib.Serialization.Interfaces;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using WPFVersion3D.Interface;
using WPFVersion3D.Model;
using WPFVersion3D.ViewModel;

namespace WPFVersion3D.Configure
{
    internal static class ContainerConfigure
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<FileLog>().As<ILog>().SingleInstance();
            builder.RegisterType<MessageBoxLog>().As<ILog>().SingleInstance();
            builder.RegisterType<Logs>().AsSelf().SingleInstance();
            builder.RegisterType<EndPoints>().As<BaseEndPoints>().SingleInstance();
            builder.RegisterType<MainWindowViewModel>().AsSelf().InstancePerLifetimeScope().PropertiesAutowired();
            builder.RegisterType<Vertex3DFactory>().As<IVertexFactory>().SingleInstance();
            builder.RegisterType<Vertex3DCostFactory>().As<IVertexCostFactory>().SingleInstance();
            builder.RegisterType<Coordinate3DFactory>().As<ICoordinateFactory>().SingleInstance();
            builder.RegisterType<PathInput>().As<IPathInput>().SingleInstance();
            builder.RegisterType<Graph3DFactory>().As<IGraphFactory>().SingleInstance();
            builder.RegisterType<GraphAssemble>().As<IGraphAssemble>().SingleInstance();
            builder.RegisterType<GraphSerializer>().As<IGraphSerializer>().SingleInstance();
            builder.RegisterType<Vertex3DEventHolder>().As<IVertexEventHolder>().SingleInstance().PropertiesAutowired();
            builder.RegisterType<GraphField3DFactory>().As<BaseGraphFieldFactory>().SingleInstance();
            builder.RegisterType<BinaryFormatter>().As<IFormatter>().SingleInstance();
            builder.RegisterType<CardinalCoordinateAroundRadarFactory>().As<ICoordinateRadarFactory>().SingleInstance();
            builder.RegisterType<Vertex3DSerializationInfoConverter>().As<IVertexSerializationInfoConverter>().SingleInstance();
            builder.RegisterType<CubicModel3DFactory>().As<IModel3DFactory>().SingleInstance();
            builder.RegisterType<ConcreteAssembleAlgorithmClasses>().As<IAssembleClasses>().SingleInstance();
            builder.RegisterType<AssembleLoadPath>().As<IAssembleLoadPath>().SingleInstance();
            builder.RegisterType<AllDirectories>().As<IAssembleSearchOption>().SingleInstance();
            builder.RegisterType<LoadFrom>().As<IAssembleLoadMethod>().SingleInstance();

            return builder.Build();
        }
    }
}
