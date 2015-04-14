
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
using System.IO;
using System.Reflection;
using Polenter.Serialization.Advanced.Binary;
using Polenter.Serialization.Advanced.Deserializing;
using Polenter.Serialization.Core;
using Polenter.Serialization.Core.Binary;

namespace Polenter.Serialization.Advanced
{
    /// <summary>
    ///   Contains logic to deserialize data from a binary format. Format can vary according to the used IBinaryWriter. 
    ///   For data serialized with BurstBinaryWriter you use BurstBinaryReader and for SizeOptimizedBinaryWriter you use SizeOptimizedBinaryReader
    /// </summary>
    public sealed class BinaryPropertyDeserializer : IPropertyDeserializer
    {
        private readonly IBinaryReader _reader;
        
        /// <summary>
        /// Properties already processed. Used for reference resolution.
        /// </summary>
        private readonly Dictionary<int, ReferenceTargetProperty> _propertyCache =
            new Dictionary<int, ReferenceTargetProperty>();
        
        ///<summary>
        ///</summary>
        ///<param name = "reader"></param>
        public BinaryPropertyDeserializer(IBinaryReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }
            this._reader = reader;
        }
        
        #region IPropertyDeserializer Members
        
        /// <summary>
        ///   Open the stream to read
        /// </summary>
        /// <param name = "stream"></param>
        public void Open(Stream stream)
        {
            this._reader.Open(stream);
        }
        
        /// <summary>
        ///   Reading the property
        /// </summary>
        /// <returns></returns>
        public Property Deserialize()
        {
            byte elementId = this._reader.ReadElementId();
            return this.deserialize(elementId, null);
        }
        
        /// <summary>
        ///   Cleans all
        /// </summary>
        public void Close()
        {
            this._reader.Close();
        }
        
        #endregion
        
        private Property deserialize(byte elementId, Type expectedType)
        {
            // Estimate property name
            string propertyName = this._reader.ReadName();
            return this.deserialize(elementId, propertyName, expectedType);
        }
        
        private Property deserialize(byte elementId, string propertyName, Type expectedType)
        {
            // Estimate property type
            Type propertyType = this._reader.ReadType();
            
            // id propertyType is not defined, we'll take the expectedType
            if (propertyType == null)
            {
                propertyType = expectedType;
            }
            
            int referenceId = 0;
            if (elementId == Elements.Reference || Elements.IsElementWithId(elementId))
            {
                referenceId = this._reader.ReadNumber();
                
                if (elementId == Elements.Reference)
                {
                    // This is reference
                    // Get property from the cache
                    return this.createProperty(referenceId, propertyName, propertyType);                    
                }
            }
            
            // create the property
            Property property = createProperty(elementId, propertyName, propertyType);
            if (property == null)
            {
                return null;
            }
            
            // Null property?
            var nullProperty = property as NullProperty;
            if (nullProperty != null)
            {
                return nullProperty;
            }
            
            // is it simple property?
            var simpleProperty = property as SimpleProperty;
            if (simpleProperty != null)
            {
                this.parseSimpleProperty(simpleProperty);
                return simpleProperty;
            }
            
            var referenceProperty = property as ReferenceTargetProperty;
            if (referenceProperty != null)
            {
                if (referenceId > 0)
                {
                    // object is used multiple times
                    referenceProperty.Reference = new ReferenceInfo();
                    referenceProperty.Reference.Id = referenceId;
                    referenceProperty.Reference.IsProcessed = true;
                    this._propertyCache.Add(referenceId, referenceProperty);
                }
            }
            
            var multiDimensionalArrayProperty = property as MultiDimensionalArrayProperty;
            if (multiDimensionalArrayProperty != null)
            {
                this.parseMultiDimensionalArrayProperty(multiDimensionalArrayProperty);
                return multiDimensionalArrayProperty;
            }
            
            var singleDimensionalArrayProperty = property as SingleDimensionalArrayProperty;
            if (singleDimensionalArrayProperty != null)
            {
                this.parseSingleDimensionalArrayProperty(singleDimensionalArrayProperty);
                return singleDimensionalArrayProperty;
            }
            
            var dictionaryProperty = property as DictionaryProperty;
            if (dictionaryProperty != null)
            {
                this.parseDictionaryProperty(dictionaryProperty);
                return dictionaryProperty;
            }
            
            var collectionProperty = property as CollectionProperty;
            if (collectionProperty != null)
            {
                this.parseCollectionProperty(collectionProperty);
                return collectionProperty;
            }
            
            var complexProperty = property as ComplexProperty;
            if (complexProperty != null)
            {
                this.parseComplexProperty(complexProperty);
                return complexProperty;
            }
            
            return property;
        }
        
        private void parseComplexProperty(ComplexProperty property)
        {
            // There are properties
            this.readProperties(property.Properties, property.Type);
        }
        
        private void readProperties(PropertyCollection properties, Type ownerType)
        {
            int count = this._reader.ReadNumber();
            for (int i = 0; i < count; i++)
            {
                byte elementId = this._reader.ReadElementId();
                
                string propertyName = this._reader.ReadName();
                
                // estimating the propertyInfo
                PropertyInfo subPropertyInfo = ownerType.GetProperty(propertyName);
                var propertyType = subPropertyInfo != null ? subPropertyInfo.PropertyType : null;
                Property subProperty = this.deserialize(elementId, propertyName, propertyType);
                properties.Add(subProperty);
            }
        }
        
        private void parseCollectionProperty(CollectionProperty property)
        {
            // Element type
            property.ElementType = this._reader.ReadType();
            
            // Properties
            this.readProperties(property.Properties, property.Type);
            
            // Items
            this.readItems(property.Items, property.ElementType);
        }
        
        private void parseDictionaryProperty(DictionaryProperty property)
        {
            // expected key type
            property.KeyType = this._reader.ReadType();
            
            // expected value type
            property.ValueType = this._reader.ReadType();
            
            // Properties
            this.readProperties(property.Properties, property.Type);
            
            // Items
            this.readDictionaryItems(property.Items, property.KeyType, property.ValueType);
        }
        
        private void readDictionaryItems(IList<KeyValueItem> items, Type expectedKeyType, Type expectedValueType)
        {
            // count
            int count = this._reader.ReadNumber();
            
            // items
            for (int i = 0; i < count; i++)
            {
                this.readDictionaryItem(items, expectedKeyType, expectedValueType);
            }
        }
        
        private void readDictionaryItem(IList<KeyValueItem> items, Type expectedKeyType, Type expectedValueType)
        {
            // key
            byte elementId = this._reader.ReadElementId();
            Property keyProperty = this.deserialize(elementId, expectedKeyType);
            
            // value
            elementId = this._reader.ReadElementId();
            Property valueProperty = this.deserialize(elementId, expectedValueType);
            
            // add the item
            var item = new KeyValueItem(keyProperty, valueProperty);
            items.Add(item);
        }
        
        private void parseSingleDimensionalArrayProperty(SingleDimensionalArrayProperty property)
        {
            // Element type
            property.ElementType = this._reader.ReadType();
            
            // Lowerbound
            property.LowerBound = this._reader.ReadNumber();
            
            this.readItems(property.Items, property.ElementType);
        }
        
        private void readItems(ICollection<Property> items, Type expectedElementType)
        {
            int count = this._reader.ReadNumber();
            for (int i = 0; i < count; i++)
            {
                byte elementId = this._reader.ReadElementId();
                Property subProperty = this.deserialize(elementId, expectedElementType);
                items.Add(subProperty);
            }
        }
        
        private void parseMultiDimensionalArrayProperty(MultiDimensionalArrayProperty property)
        {
            // Element Type
            property.ElementType = this._reader.ReadType();
            
            // Dimension Infos
            this.readDimensionInfos(property.DimensionInfos);
            
            // Items
            this.readMultiDimensionalArrayItems(property.Items, property.ElementType);
        }
        
        private void readMultiDimensionalArrayItems(IList<MultiDimensionalArrayItem> items, Type expectedElementType)
        {
            // count
            int count = this._reader.ReadNumber();
            
            // items
            for (int i = 0; i < count; i++)
            {
                this.readMultiDimensionalArrayItem(items, expectedElementType);
            }
        }
        
        private void readMultiDimensionalArrayItem(IList<MultiDimensionalArrayItem> items, Type expectedElementType)
        {
            // Coordinates
            int[] indexes = this._reader.ReadNumbers();
            
            // item itself
            byte elementId = this._reader.ReadElementId();
            Property value = this.deserialize(elementId, expectedElementType);
            var item = new MultiDimensionalArrayItem(indexes, value);
            items.Add(item);
        }
        
        private void readDimensionInfos(IList<DimensionInfo> dimensionInfos)
        {
            // count
            int count = this._reader.ReadNumber();
            
            // Dimensions
            for (int i = 0; i < count; i++)
            {
                this.readDimensionInfo(dimensionInfos);
            }
        }
        
        private void readDimensionInfo(IList<DimensionInfo> dimensionInfos)
        {
            var info = new DimensionInfo();
            info.Length = this._reader.ReadNumber();
            info.LowerBound = this._reader.ReadNumber();
            
            dimensionInfos.Add(info);
        }
        
        private void parseSimpleProperty(SimpleProperty property)
        {
            // There is value
            property.Value = this._reader.ReadValue(property.Type);
        }
        
        private static Property createProperty(byte elementId, string propertyName, Type propertyType)
        {
            switch (elementId)
            {
                case Elements.SimpleObject:
                    return new SimpleProperty(propertyName, propertyType);
                case Elements.ComplexObject:
                case Elements.ComplexObjectWithId:
                    return new ComplexProperty(propertyName, propertyType);
                case Elements.Collection:
                case Elements.CollectionWithId:
                    return new CollectionProperty(propertyName, propertyType);
                case Elements.Dictionary:
                case Elements.DictionaryWithId:
                    return new DictionaryProperty(propertyName, propertyType);
                case Elements.SingleArray:
                case Elements.SingleArrayWithId:
                    return new SingleDimensionalArrayProperty(propertyName, propertyType);
                case Elements.MultiArray:
                case Elements.MultiArrayWithId:
                    return new MultiDimensionalArrayProperty(propertyName, propertyType);
                case Elements.Null:
                    return new NullProperty(propertyName);
                default:
                    return null;
            }
        }
        
        private Property createProperty(int referenceId, string propertyName, Type propertyType)
        {
            var cachedProperty = this._propertyCache[referenceId];
            var property = (ReferenceTargetProperty)Property.CreateInstance(cachedProperty.Art, propertyName, propertyType);
            cachedProperty.Reference.Count++;
            property.MakeFlatCopyFrom(cachedProperty);
            // Reference must be recreated, cause IsProcessed differs for reference and the full property
            property.Reference = new ReferenceInfo() { Id = referenceId };
            return property;
        }
    }
}