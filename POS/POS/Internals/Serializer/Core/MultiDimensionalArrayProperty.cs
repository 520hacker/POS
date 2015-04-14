using System;
using System.Collections.Generic;

namespace Polenter.Serialization.Core
{
    /// <summary>
    ///   Represents multidimensional array. Array properties are in DimensionInfos
    /// </summary>
    public sealed class MultiDimensionalArrayProperty : ReferenceTargetProperty
    {
        private IList<DimensionInfo> _dimensionInfos;
        private IList<MultiDimensionalArrayItem> _items;

        ///<summary>
        ///</summary>
        ///<param name = "name"></param>
        ///<param name = "type"></param>
        public MultiDimensionalArrayProperty(string name, Type type) : base(name, type)
        {
        }

        ///<summary>
        ///</summary>
        public IList<MultiDimensionalArrayItem> Items
        {
            get
            {
                if (this._items == null)
                {
                    this._items = new List<MultiDimensionalArrayItem>();
                }
                return this._items;
            }
            set
            {
                this._items = value;
            }
        }

        /// <summary>
        ///   Information about the array
        /// </summary>
        public IList<DimensionInfo> DimensionInfos
        {
            get
            {
                if (this._dimensionInfos == null)
                {
                    this._dimensionInfos = new List<DimensionInfo>();
                }
                return this._dimensionInfos;
            }
            set
            {
                this._dimensionInfos = value;
            }
        }

        /// <summary>
        ///   Of what type are elements. All elements in all all dimensions must be inheritors of this type.
        /// </summary>
        public Type ElementType { get; set; }

        ///<summary>
        /// Makes flat copy (only references) of vital properties
        ///</summary>
        ///<param name="source"></param>
        public override void MakeFlatCopyFrom(ReferenceTargetProperty source)
        {
            var arrayProp = source as MultiDimensionalArrayProperty;
            if (arrayProp == null)
            {
                throw new InvalidCastException(
                    string.Format("Invalid property type to make a flat copy. Expected {0}, current {1}",
                        typeof(SingleDimensionalArrayProperty), source.GetType()));
            }

            base.MakeFlatCopyFrom(source);

            this.ElementType = arrayProp.ElementType;
            this.DimensionInfos = arrayProp.DimensionInfos;
            this.Items = arrayProp.Items;
        }

        /// <summary>
        /// Gets the property art.
        /// </summary>
        /// <returns></returns>
        protected override PropertyArt GetPropertyArt()
        {
            return PropertyArt.MultiDimensionalArray;
        }
    }
}