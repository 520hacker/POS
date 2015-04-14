using System;

namespace POS.Internals.DialogBuilder.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CustomControlAttribute : Attribute
    {
        public Type Type { get; set; }

        public CustomControlAttribute(Type t)
        {
            Type = t;
        }
    }
}