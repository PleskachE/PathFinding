﻿using System;

namespace Common.Attrbiutes
{
    [AttributeUsage(AttributeTargets.All)]
    public abstract class GroupAttribute : Attribute
    {
        public abstract override bool Equals(object obj);

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
