using System;

namespace RM.CommonLibrary.HelperMiddleware
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