using System;

namespace Polenter.Serialization.Core
{
    /// <summary>
    ///   Represents complex type which contains properties.
    /// </summary>
    public class ComplexProperty : ReferenceTargetProperty
    {
        private PropertyCollection _properties;

        ///<summary>
        ///</summary>
        ///<param name = "name"></param>
        ///<param name = "type"></param>
        public ComplexProperty(string name, Type type) : base(name, type)
        {
        }

        ///<summary>
        ///</summary>
        public PropertyCollection Properties
        {
            get
            {
                if (this._properties == null)
                {
                    this._properties = new PropertyCollection { Parent = this };
                }
                return this._properties;
            }
            set
            {
                this._properties = value;
            }
        }

        ///<summary>
        /// Makes flat copy (only references) of vital properties
        ///</summary>
        ///<param name="source"></param>
        public override void MakeFlatCopyFrom(ReferenceTargetProperty source)
        {
            var complexProperty = source as ComplexProperty;
            if (complexProperty == null)
            {
                throw new InvalidCastException(
                    string.Format("Invalid property type to make a flat copy. Expected {0}, current {1}",
                        typeof(ComplexProperty), source.GetType()));
            }

            base.MakeFlatCopyFrom(source);

            this.Properties = complexProperty.Properties;
        }

        /// <summary>
        /// Gets the property art.
        /// </summary>
        /// <returns></returns>
        protected override PropertyArt GetPropertyArt()
        {
            return PropertyArt.Complex;
        }
    }
}