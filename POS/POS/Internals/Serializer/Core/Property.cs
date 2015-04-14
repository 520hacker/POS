
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

namespace Polenter.Serialization.Core
{
    /// <summary>
    ///   Base class for all properties. Every object can be defined with inheritors of the Property class.
    /// </summary>
    public abstract class Property
    {
        /// <summary>
        /// </summary>
        /// <param name = "name"></param>
        /// <param name = "type"></param>
        protected Property(string name, Type type)
        {
            this.Name = name;
            this.Type = type;
        }
        
        /// <summary>
        ///   Not all properties have name (i.e. items of a collection)
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        ///   Of what type is the property or its value
        /// </summary>
        public Type Type { get; set; }
        
        /// <summary>
        ///   If the properties are nested, i.e. collection items are nested in the collection
        /// </summary>
        public Property Parent { get; set; }
        
        ///<summary>
        /// Of what art is the property.
        ///</summary>
        public PropertyArt Art
        {
            get
            {
                return this.GetPropertyArt();
            }
        }
        
        /// <summary>
        /// Gets the property art.
        /// </summary>
        /// <returns></returns>
        protected abstract PropertyArt GetPropertyArt();
        
        ///<summary>
        /// Creates property from PropertyArt
        ///</summary>
        ///<param name="art"></param>
        ///<param name="propertyName"></param>
        ///<param name="propertyType"></param>
        ///<returns>null if PropertyArt.Reference is requested</returns>
        ///<exception cref="InvalidOperationException">If unknown PropertyArt requested</exception>
        public static Property CreateInstance(PropertyArt art, string propertyName, Type propertyType)
        {
            switch (art)
            {
                case PropertyArt.Collection:
                    return new CollectionProperty(propertyName, propertyType);
                case PropertyArt.Complex:
                    return new ComplexProperty(propertyName, propertyType);
                case PropertyArt.Dictionary:
                    return new DictionaryProperty(propertyName, propertyType);
                case PropertyArt.MultiDimensionalArray:
                    return new MultiDimensionalArrayProperty(propertyName, propertyType);
                case PropertyArt.Null:
                    return new NullProperty(propertyName);
                case PropertyArt.Reference:
                    return null;
                case PropertyArt.Simple:
                    return new SimpleProperty(propertyName, propertyType);
                case PropertyArt.SingleDimensionalArray:
                    return new SingleDimensionalArrayProperty(propertyName, propertyType);
                default:
                    throw new InvalidOperationException(string.Format("Unknown PropertyArt {0}", art));
            }
        }
        
        ///<summary>
        ///</summary>
        ///<returns></returns>
        ///<exception cref="NotImplementedException"></exception>
        public override string ToString()
        {
            string name = this.Name ?? "null";
            string type = this.Type == null ? "null" : this.Type.Name;
            string parent = this.Parent == null ? "null" : this.Parent.GetType().Name;            
            return string.Format("{0}, Name={1}, Type={2}, Parent={3}", this.GetType().Name, name, type, parent);
        }
    }
}