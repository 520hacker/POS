using System;
using System.Collections.Generic;

namespace Polenter.Serialization.Core
{
    /// <summary>
    ///   Represents type which is ICollection
    /// </summary>
    public sealed class CollectionProperty : ComplexProperty
    {
        private IList<Property> _items;

        ///<summary>
        ///</summary>
        ///<param name = "name"></param>
        ///<param name = "type"></param>
        public CollectionProperty(string name, Type type) : base(name, type)
        {
        }

        ///<summary>
        ///</summary>
        public IList<Property> Items
        {
            get
            {
                if (this._items == null)
                {
                    this._items = new List<Property>();
                }
                return this._items;
            }
            set
            {
                this._items = value;
            }
        }

        /// <summary>
        ///   Of what type are items. It's important for polymorphic collection
        /// </summary>
        public Type ElementType { get; set; }

        ///<summary>
        /// Makes flat copy (only references) of vital properties
        ///</summary>
        ///<param name="source"></param>
        public override void MakeFlatCopyFrom(ReferenceTargetProperty source)
        {
            var collectionSource = source as CollectionProperty;
            if (collectionSource == null)
            {
                throw new InvalidCastException(
                    string.Format("Invalid property type to make a flat copy. Expected {0}, current {1}",
                        typeof(CollectionProperty), source.GetType()));
            }

            base.MakeFlatCopyFrom(source);

            this.ElementType = collectionSource.ElementType;
            this.Items = collectionSource.Items;
        }

        /// <summary>
        /// Gets the property art.
        /// </summary>
        /// <returns></returns>
        protected override PropertyArt GetPropertyArt()
        {
            return PropertyArt.Collection;
        }
    }
}