using System;
using System.Collections.Generic;

namespace Polenter.Serialization.Core
{
    /// <summary>
    ///   Represents dictionary. Every item is composed of the key and value
    /// </summary>
    public sealed class DictionaryProperty : ComplexProperty
    {
        private IList<KeyValueItem> _items;

        ///<summary>
        ///</summary>
        ///<param name = "name"></param>
        ///<param name = "type"></param>
        public DictionaryProperty(string name, Type type) : base(name, type)
        {
        }

        ///<summary>
        ///</summary>
        public IList<KeyValueItem> Items
        {
            get
            {
                if (this._items == null)
                {
                    this._items = new List<KeyValueItem>();
                }
                return this._items;
            }
            set
            {
                this._items = value;
            }
        }

        /// <summary>
        ///   Of what type are keys
        /// </summary>
        public Type KeyType { get; set; }

        /// <summary>
        ///   Of what type are values
        /// </summary>
        public Type ValueType { get; set; }

        ///<summary>
        /// Makes flat copy (only references) of vital properties
        ///</summary>
        ///<param name="source"></param>
        public override void MakeFlatCopyFrom(ReferenceTargetProperty source)
        {
            var dictionarySource = source as DictionaryProperty;
            if (dictionarySource == null)
            {
                throw new InvalidCastException(
                    string.Format("Invalid property type to make a flat copy. Expected {0}, current {1}",
                        typeof(DictionaryProperty), source.GetType()));
            }

            base.MakeFlatCopyFrom(source);

            this.KeyType = dictionarySource.KeyType;
            this.ValueType = dictionarySource.ValueType;
            this.Items = dictionarySource.Items;
        }

        /// <summary>
        /// Gets the property art.
        /// </summary>
        /// <returns></returns>
        protected override PropertyArt GetPropertyArt()
        {
            return PropertyArt.Dictionary;
        }
    }
}