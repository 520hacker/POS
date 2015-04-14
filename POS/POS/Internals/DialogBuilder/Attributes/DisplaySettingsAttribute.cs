using System;

namespace POS.Internals.DialogBuilder.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DisplaySettingsAttribute : Attribute
    {
        public DisplaySettingsAttribute()
        {
            this.Visible = true;
        }

        public string Label { get; set; }

        public bool ReadOnly { get; set; }

        public bool Visible { get; set; }

        public int Width { get; set; }
    }
}