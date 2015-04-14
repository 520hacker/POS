using System;

namespace POS.Internals.DialogBuilder.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CustomControlAttribute : Attribute
    {
        public CustomControlAttribute(Type t)
        {
            this.Type = t;
        }

        public Type Type { get; set; }
    }
}