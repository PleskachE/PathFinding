﻿using Algorithm.EventArguments;

namespace Algorithm.Handlers
{
    /// <summary>
    /// Represents a method that will handle events that occur during the pathfinding process
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void AlgorithmEventHandler(object sender, AlgorithmEventArgs e);
}
