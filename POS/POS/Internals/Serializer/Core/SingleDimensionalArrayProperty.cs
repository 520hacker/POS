using System;

namespace Polenter.Serialization.Core
{
    /// <summary>
    ///   Represents one dimensional array
    /// </summary>
    public sealed class SingleDimensionalArrayProperty : ReferenceTargetProperty
    {
        private PropertyCollection _items;

        ///<summary>
        ///</summary>
        ///<param name = "name"></param>
        ///<param name = "type"></param>
        public SingleDimensionalArrayProperty(string name, Type type) : base(name, type)
        {
        }

        ///<summary>
        ///</summary>
        public PropertyCollection Items
        {
            get
            {
                if (this._items == null)
                {
                    this._items = new PropertyCollection { Parent = this };
                }
                return this._items;
            }
            set
            {
                this._items = value;
            }
        }

        /// <summary>
        ///   As default is 0, but there can be higher start index
        /// </summary>
        public int LowerBound { get; set; }

        /// <summary>
        ///   Of what type are elements
        /// </summary>
        public Type ElementType { get; set; }

        ///<summary>
        /// Makes flat copy (only references) of vital properties
        ///</summary>
        ///<param name="source"></param>
        public override void MakeFlatCopyFrom(ReferenceTargetProperty source)
        {
            var arrayProp = source as SingleDimensionalArrayProperty;
            if (arrayProp == null)
            {
                throw new InvalidCastException(
                    string.Format("Invalid property type to make a flat copy. Expected {0}, current {1}",
                        typeof(SingleDimensionalArrayProperty), source.GetType()));
            }

            base.MakeFlatCopyFrom(source);

            this.LowerBound = arrayProp.LowerBound;
            this.ElementType = arrayProp.ElementType;
            this.Items = arrayProp.Items;
        }

        /// <summary>
        /// Gets the property art.
        /// </summary>
        /// <returns></returns>
        protected override PropertyArt GetPropertyArt()
        {
            return PropertyArt.SingleDimensionalArray;
        }
    }
}