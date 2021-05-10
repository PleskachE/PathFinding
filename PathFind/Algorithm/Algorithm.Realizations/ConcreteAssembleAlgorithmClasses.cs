﻿using Algorithm.Common;
using Algorithm.Interfaces;
using AssembleClassesLib.Interface;
using AssembleClassesLib.Realizations.AssembleClassesImpl;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Algorithm.Realizations
{
    public sealed class ConcreteAssembleAlgorithmClasses : AssembleClasses
    {
        /// <summary>
        /// Returns algorithm according to 
        /// <paramref name="key"></paramref>
        /// </summary>
        /// <param name="key"></param>
        /// <param name="parametres"></param>
        /// <returns>An instance of algorithm if 
        /// <paramref name="key"></paramref> exists and
        /// <see cref="NullAlgorithm"></see> when doesn't</returns>
        public override object Get(string key, params object[] parametres)
        {
            return base.Get(key, parametres) ?? new NullAlgorithm();
        }

        public ConcreteAssembleAlgorithmClasses(IAssembleLoadPath path,
            IAssembleSearchOption searchOption, IAssembleLoadMethod loadMethod)
            : base(path, searchOption, loadMethod)
        {
            baseType = typeof(IAlgorithm);
        }

        protected override void LoadClassesFromAssemble()
        {
            base.LoadClassesFromAssemble();
            types = types
                .Where(IsConcreteAlgorithm)
                .ToDictionary(ClassName, Type);
        }

        private string ClassName(KeyValuePair<string, Type> algo)
        {
            return algo.Key;
        }

        private Type Type(KeyValuePair<string, Type> algo)
        {
            return algo.Value;
        }

        private bool IsConcreteAlgorithm(KeyValuePair<string, Type> algo)
        {
            return !algo.Value.IsAbstract
                   && baseType.IsAssignableFrom(algo.Value);
        }

        private readonly Type baseType;
    }
}