﻿using System;

namespace AssembleClassesLib.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Struct, Inherited = false)]
    public sealed class ClassNameAttribute : Attribute
    {
        public string Name { get; }

        public ClassNameAttribute(string name)
        {
            Name = name;
        }
    }
}