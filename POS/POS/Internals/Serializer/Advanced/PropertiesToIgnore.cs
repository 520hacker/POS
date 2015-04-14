using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Polenter.Serialization.Advanced
{
    /// <summary>
    ///   Cache which contains type as a key, and all associated property names
    /// </summary>
    public sealed class PropertiesToIgnore
    {
        private readonly TypePropertiesToIgnoreCollection _propertiesToIgnore = new TypePropertiesToIgnoreCollection();

        ///<summary>
        ///</summary>
        ///<param name = "type"></param>
        ///<param name = "propertyName"></param>
        public void Add(Type type, string propertyName)
        {
            TypePropertiesToIgnore item = this.getPropertiesToIgnore(type);
            if (!item.PropertyNames.Contains(propertyName))
            {
                item.PropertyNames.Add(propertyName);
            }
        }

        private TypePropertiesToIgnore getPropertiesToIgnore(Type type)
        {
            TypePropertiesToIgnore item = this._propertiesToIgnore.TryFind(type);
            if (item == null)
            {
                item = new TypePropertiesToIgnore(type);
                this._propertiesToIgnore.Add(item);
            }
            return item;
        }

        ///<summary>
        ///</summary>
        ///<param name = "type"></param>
        ///<param name = "propertyName"></param>
        ///<returns></returns>
        public bool Contains(Type type, string propertyName)
        {
            return this._propertiesToIgnore.ContainsProperty(type, propertyName);
        }

        #region Nested type: TypePropertiesToIgnore

        private sealed class TypePropertiesToIgnore
        {
            private IList<string> _propertyNames;

            public TypePropertiesToIgnore(Type type)
            {
                this.Type = type;
            }

            public Type Type { get; set; }

            public IList<string> PropertyNames
            {
                get
                {
                    if (this._propertyNames == null)
                    {
                        this._propertyNames = new List<string>();
                    }
                    return this._propertyNames;
                }
                set
                {
                    this._propertyNames = value;
                }
            }
        }

        #endregion

        #region Nested type: TypePropertiesToIgnoreCollection

        private sealed class TypePropertiesToIgnoreCollection : KeyedCollection<Type, TypePropertiesToIgnore>
        {
            protected override Type GetKeyForItem(TypePropertiesToIgnore item)
            {
                return item.Type;
            }

            public TypePropertiesToIgnore TryFind(Type type)
            {
                foreach (TypePropertiesToIgnore item in this.Items)
                {
                    if (item.Type == type)
                    {
                        return item;
                    }
                }
                return null;
            }

            public bool ContainsProperty(Type type, string propertyName)
            {
                TypePropertiesToIgnore propertiesToIgnore = this.TryFind(type);
                if (propertiesToIgnore == null)
                {
                    return false;
                }
                return propertiesToIgnore.PropertyNames.Contains(propertyName);
            }
        }
        
        #endregion
    }
}