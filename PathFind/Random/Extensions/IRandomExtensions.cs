﻿using Random.Interface;
using System.Collections.Generic;
using System.Linq;
using ValueRange;

namespace Random.Extensions
{
    public static class IRandomExtensions
    {
        /// <summary>
        /// Returns random number within the <paramref name="range"/> inclusivly
        /// </summary>
        /// <param name="self"></param>
        /// <param name="range">A range, that sets the boundaries 
        /// of the range of random values</param>
        /// <returns>A random number within <paramref name="range"/></returns>
        public static int Next(this IRandom self, InclusiveValueRange<int> range)
        {
            return self.Next(range.LowerValueOfRange, range.UpperValueOfRange);
        }

        public static int Next(this IRandom random)
        {
            return random.Next(0, int.MaxValue);
        }

        public static IEnumerable<T> Shuffle<T>(this IRandom self, IEnumerable<T> collection)
        {
            return collection.OrderBy(_ => self.Next());
        }
    }
}
