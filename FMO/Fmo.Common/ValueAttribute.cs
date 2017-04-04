using System;

namespace Fmo.Common
{
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class ValueAttribute : Attribute
    {
        public ValueAttribute(string value)
        {
            Value = value;
        }

        public string Value { get; private set; }
    }
}
