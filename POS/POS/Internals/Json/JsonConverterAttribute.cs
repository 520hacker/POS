using System;
using System.Globalization;
using System.Linq;
using Lib.JSON.Utilities;

namespace Lib.JSON
{
    /// <summary>
    /// Instructs the <see cref="JsonSerializer"/> to use the specified <see cref="JsonConverter"/> when serializing the member or class.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface | AttributeTargets.Enum | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class JsonConverterAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonConverterAttribute"/> class.
        /// </summary>
        /// <param name="converterType">Type of the converter.</param>
        public JsonConverterAttribute(Type converterType)
        {
            if (converterType == null)
            {
                throw new ArgumentNullException("converterType");
            }

            this.ConverterType = converterType;
        }

        /// <summary>
        /// Gets the type of the converter.
        /// </summary>
        /// <value>The type of the converter.</value>
        public Type ConverterType { get; private set; }

        internal static JsonConverter CreateJsonConverterInstance(Type converterType)
        {
            try
            {
                return (JsonConverter)Activator.CreateInstance(converterType);
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating {0}".FormatWith(CultureInfo.InvariantCulture, converterType), ex);
            }
        }
    }
}