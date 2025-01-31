﻿using ConsoleVersion.Interface;
using ConsoleVersion.Views;

namespace ConsoleVersion.Model.FramedAxes
{
    /// <summary>
    /// An abstract class, that provides methods for
    /// generating framed axis of a graph field
    /// </summary>
    internal abstract class FramedAxis : IFramedAxis, IDisplayable
    {
        public abstract string GetFramedAxis();

        public abstract void Display();

        protected abstract string Offset { get; }

        protected const char Space = ' ';

        protected int LateralDistance => MainView.GetLateralDistanceBetweenVertices();
    }
}
