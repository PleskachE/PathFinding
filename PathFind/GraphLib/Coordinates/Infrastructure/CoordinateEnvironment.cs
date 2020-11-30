﻿using GraphLib.Coordinates.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphLib.Coordinates.Infrastructure
{
    public sealed class CoordinateEnvironment
    {
        public CoordinateEnvironment(ICoordinate coordinate)
        {
            selfCoordinates = coordinate.Coordinates.ToArray();
            neighbourCoordinates = new int[selfCoordinates.Length];
            coordinateType = coordinate.GetType();
            environment = new List<ICoordinate>();
            middleCoordinate = coordinate;
            limitDepth = selfCoordinates.Length;
        }

        public IEnumerable<ICoordinate> GetEnvironment()
        {
            FormEnvironment();
            return environment;
        }

        // recursive method
        private void FormEnvironment(int currentDepth = 0)
        {
            int leftNeighbour = selfCoordinates[currentDepth] - 1;
            int rightNeighbour = selfCoordinates[currentDepth] + 1;
            for (int i = leftNeighbour; i <= rightNeighbour; i++)
            {
                neighbourCoordinates[currentDepth] = i;
                if (CanMoveDeeper(currentDepth))
                    FormEnvironment(currentDepth + 1);
                else
                    AddNeighbourToEnvironment();
            }
        }

        private void AddNeighbourToEnvironment()
        {
            if (!neighbourCoordinates.Any(value => value < 0))
            {
                var coordinate = CreateCoordinate();

                if (!middleCoordinate.Equals(coordinate))
                {
                    environment.Add(coordinate);
                }
            }
        }

        private ICoordinate CreateCoordinate()
        {
            return (ICoordinate)Activator.
                CreateInstance(coordinateType, neighbourCoordinates);
        }

        private bool CanMoveDeeper(int currentDepth)
        {
            return currentDepth < limitDepth - 1;
        }
       
        private readonly Type coordinateType;
        private readonly ICoordinate middleCoordinate;

        private readonly int[] neighbourCoordinates;
        private readonly int[] selfCoordinates;

        private readonly List<ICoordinate> environment;
        private readonly int limitDepth;
    }
}
