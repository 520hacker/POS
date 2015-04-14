using System;
using System.Collections.Generic;
using System.Reflection;

namespace Polenter.Serialization.Advanced
{
    ///<summary>
    ///</summary>
    internal class PropertyCache
    {
        private readonly Dictionary<Type, IList<PropertyInfo>> _cache = new Dictionary<Type, IList<PropertyInfo>>();

        /// <summary>
        /// </summary>
        /// <returns>null if the key was not found</returns>
        public IList<PropertyInfo> TryGetPropertyInfos(Type type)
        {
            if (!this._cache.ContainsKey(type))
            {
                return null;
            }
            return this._cache[type];
        }

        public void Add(Type key, IList<PropertyInfo> value)
        {
            this._cache.Add(key, value);
        }
    }
}