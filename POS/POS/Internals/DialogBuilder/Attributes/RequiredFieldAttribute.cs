using System;

namespace POS.Internals.DialogBuilder.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class RequiredFieldAttribute : Attribute
    {
        public string Message { get; set; }
    }
}