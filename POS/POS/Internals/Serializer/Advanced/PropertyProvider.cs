
#region Copyright © 2010 Pawel Idzikowski [idzikowski@sharpserializer.com]

//  ***********************************************************************
//  Project: sharpSerializer
//  Web: http://www.sharpserializer.com
//  
//  This software is provided 'as-is', without any express or implied warranty.
//  In no event will the author(s) be held liable for any damages arising from
//  the use of this software.
//  
//  Permission is granted to anyone to use this software for any purpose,
//  including commercial applications, and to alter it and redistribute it
//  freely, subject to the following restrictions:
//  
//      1. The origin of this software must not be misrepresented; you must not
//        claim that you wrote the original software. If you use this software
//        in a product, an acknowledgment in the product documentation would be
//        appreciated but is not required.
//  
//      2. Altered source versions must be plainly marked as such, and must not
//        be misrepresented as being the original software.
//  
//      3. This notice may not be removed or altered from any source distribution.
//  
//  ***********************************************************************

#endregion

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

namespace Polenter.Serialization.Advanced
{
    /// <summary>
    /// Provides properties to serialize from source object. Implements the strategy 
    /// which subproperties to use and 
    /// wich to ignore and
    /// how to travese the source object to get subproperties
    ///   
    /// Its methods GetAllProperties and IgnoreProperty can be
    ///   overwritten in an inherited class to customize its functionality. 
    ///   Its property PropertiesToIgnore contains properties, which are ignored during the serialization.
    /// </summary>
    public class PropertyProvider
    {
        private PropertiesToIgnore _propertiesToIgnore;
        private IList<Type> _attributesToIgnore;
        #if !PORTABLE
        [ThreadStatic]
        #endif
        private static PropertyCache _cache;
        
        /// <summary>
        ///   Which properties should be ignored
        /// </summary>
        /// <remarks>
        /// Sometimes you want to ignore some properties during the serialization.
        /// If they are parts of your own business objects, you can mark these properties with ExcludeFromSerializationAttribute. 
        /// However it is not possible to mark them in the built in .NET classes
        /// In such a case you add these properties to the list PropertiesToIgnore.
        /// I.e. System.Collections.Generic.List"string" has the "Capacity" property which is irrelevant for
        /// the whole Serialization and should be ignored
        /// PropertyProvider.PropertiesToIgnore.Add(typeof(List"string"), "Capacity")
        /// </remarks>
        public PropertiesToIgnore PropertiesToIgnore
        {
            get
            {
                if (this._propertiesToIgnore == null)
                {
                    this._propertiesToIgnore = new PropertiesToIgnore();
                }
                return this._propertiesToIgnore;
            }
            set
            {
                this._propertiesToIgnore = value;
            }
        }
        
        /// <summary>
        /// All Properties markt with one of the contained attribute-types will be ignored on save.
        /// </summary>
        public IList<Type> AttributesToIgnore
        {
            get
            {
                if (this._attributesToIgnore == null)
                {
                    this._attributesToIgnore = new List<Type>();
                }
                return this._attributesToIgnore;
            }
            set
            {
                this._attributesToIgnore = value;
            }
        }
        
        /// <summary>
        ///   Gives all properties back which:
        ///   - are public
        ///   - are not static
        ///   - does not contain ExcludeFromSerializationAttribute
        ///   - have their set and get accessors
        ///   - are not indexers
        /// </summary>
        /// <param name = "typeInfo"></param>
        /// <returns></returns>
        public IList<PropertyInfo> GetProperties(Polenter.Serialization.Serializing.TypeInfo typeInfo)
        {
            // Search in cache
            var propertyInfos = Cache.TryGetPropertyInfos(typeInfo.Type);
            if (propertyInfos != null)
            {
                return propertyInfos;
            }
            
            // Creating infos
            PropertyInfo[] properties = this.GetAllProperties(typeInfo.Type);
            var result = new List<PropertyInfo>();
            
            foreach (PropertyInfo property in properties)
            {
                if (!this.IgnoreProperty(typeInfo, property))
                {
                    result.Add(property);
                }
            }
            
            // adding result to Cache
            Cache.Add(typeInfo.Type, result);
            
            return result;
        }
        
        /// <summary>
        ///   Should the property be removed from serialization?
        /// </summary>
        /// <param name = "info"></param>
        /// <param name = "property"></param>
        /// <returns>
        ///   true if the property:
        ///   - is in the PropertiesToIgnore,
        ///   - contains ExcludeFromSerializationAttribute,
        ///   - does not have it's set or get accessor
        ///   - is indexer
        /// </returns>
        protected virtual bool IgnoreProperty(Polenter.Serialization.Serializing.TypeInfo info, PropertyInfo property)
        {
            // Soll die Eigenschaft ignoriert werden
            if (this.PropertiesToIgnore.Contains(info.Type, property.Name))
            {
                return true;
            }
            
            if (this.ContainsExcludeFromSerializationAttribute(property))
            {
                return true;
            }
            
            if (!property.CanRead || !property.CanWrite)
            {
                return true;
            }
            
            ParameterInfo[] indexParameters = property.GetIndexParameters();
            if (indexParameters.Length > 0)
            {
                // Indexer
                return true;
            }
            
            return false;
        }
        
        /// <summary>
        /// Determines whether <paramref name="property"/> is excluded from serialization or not.
        /// </summary>
        /// <param name="property">The property to be checked.</param>
        /// <returns>
        /// 	<c>true</c> if no serialization
        /// </returns>
        protected bool ContainsExcludeFromSerializationAttribute(PropertyInfo property)
        {
            foreach (Type attrType in this.AttributesToIgnore)
            {
                object[] attributes = property.GetCustomAttributes(attrType, false);
                if (attributes.Length > 0) 
                {
                    return true;
                }
            }
            return false;
        }
        
        /// <summary>
        ///   Gives all properties back which:
        ///   - are public
        ///   - are not static (instance properties)
        /// </summary>
        /// <param name = "type"></param>
        /// <returns></returns>
        protected virtual PropertyInfo[] GetAllProperties(Type type)
        {
            return type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
        }
        
        private static PropertyCache Cache
        {
            get
            {
                if (_cache == null)
                {
                    _cache = new PropertyCache();
                }
                return _cache;
            }
        }
    }
}