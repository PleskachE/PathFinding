﻿using GraphLib.Coordinates.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphLib.Coordinates.Infrastructure
{
    internal sealed class CoordinateEnvironment
    {
        public CoordinateEnvironment(ICoordinate coordinate)
        {
            middleCoordinate = coordinate;
            coordinateType = coordinate.GetType();
            selfCoordinates = coordinate.Coordinates.ToArray();
            neighbourCoordinates = new int[selfCoordinates.Length];
        }

        public IEnumerable<ICoordinate> GetEnvironment()
        {
            int limitDepth = selfCoordinates.Count();
            return GetEnvironment(currentDepth: 0, limitDepth);
        }

        private IEnumerable<ICoordinate> GetEnvironment(int currentDepth, int limitDepth)
        {
            var environment = new List<ICoordinate>();

            if (currentDepth < limitDepth)
            {
                for (int i = selfCoordinates[currentDepth] - 1; i <= selfCoordinates[currentDepth] + 1; i++)
                {
                    neighbourCoordinates[currentDepth] = i;

                    if (CanMoveNextDimension(currentDepth, limitDepth))
                    {
                        var portion = GetEnvironment(currentDepth + 1, limitDepth);
                        environment.AddRange(portion);
                    }
                    else
                    {
                        ExpandEnvironment(environment);
                    }                       
                }
            }

            return environment;
        }

        private void ExpandEnvironment(List<ICoordinate> environment)
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

        private bool CanMoveNextDimension(int currentDepth, int limitDepth)
        {
            return currentDepth < limitDepth - 1;
        }

        private readonly Type coordinateType;
        private readonly ICoordinate middleCoordinate;

        private readonly int[] neighbourCoordinates;
        private readonly int[] selfCoordinates;
    }
}
