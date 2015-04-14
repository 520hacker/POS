
#region License

// Copyright (c) 2007 James Newton-King
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.

#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
#if !(NET35 || NET20 || WINDOWS_PHONE)
using System.Dynamic;
#endif
using System.Globalization;
using System.Linq;
using Creek.Data.JSON.Net;
using Lib.JSON.Linq;
using Lib.JSON.Utilities;
using System.Runtime.Serialization;
using System.Security;

namespace Lib.JSON.Serialization
{
    internal class JsonSerializerInternalWriter : JsonSerializerInternalBase
    {
        private readonly List<object> _serializeStack = new List<object>();
        private JsonSerializerProxy _internalSerializer;
        
        public JsonSerializerInternalWriter(JsonSerializer serializer) : base(serializer)
        {
        }
        
        public void Serialize(JsonWriter jsonWriter, object value)
        {
            if (jsonWriter == null)
            {
                throw new ArgumentNullException("jsonWriter");
            }

            this.SerializeValue(jsonWriter, value, this.GetContractSafe(value), null, null);
        }
        
        private JsonSerializerProxy GetInternalSerializer()
        {
            if (this._internalSerializer == null)
            {
                this._internalSerializer = new JsonSerializerProxy(this);
            }

            return this._internalSerializer;
        }
        
        private JsonContract GetContractSafe(object value)
        {
            if (value == null)
            {
                return null;
            }

            return this.Serializer.ContractResolver.ResolveContract(value.GetType());
        }
        
        private void SerializePrimitive(JsonWriter writer, object value, JsonPrimitiveContract contract, JsonProperty member, JsonContract collectionValueContract)
        {
            if (contract.UnderlyingType == typeof (byte[]))
            {
                bool includeTypeDetails = this.ShouldWriteType(TypeNameHandling.Objects, contract, member, collectionValueContract);
                if (includeTypeDetails)
                {
                    writer.WriteStartObject();
                    this.WriteTypeProperty(writer, contract.CreatedType);
                    writer.WritePropertyName(JsonTypeReflector.ValuePropertyName);
                    writer.WriteValue(value);
                    writer.WriteEndObject();
                    return;
                }
            }

            writer.WriteValue(value);
        }
        
        private void SerializeValue(JsonWriter writer, object value, JsonContract valueContract, JsonProperty member, JsonContract collectionValueContract)
        {
            JsonConverter converter = (member != null) ? member.Converter : null;
            
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            
            if ((converter != null ||
                 ((converter = valueContract.Converter) != null) ||
                 ((converter = this.Serializer.GetMatchingConverter(valueContract.UnderlyingType)) != null) ||
                 ((converter = valueContract.InternalConverter) != null)) &&
                converter.CanWrite)
            {
                this.SerializeConvertable(writer, converter, value, valueContract);
                return;
            }
            
            switch (valueContract.ContractType)
            {
                case JsonContractType.Object:
                    this.SerializeObject(writer, value, (JsonObjectContract)valueContract, member, collectionValueContract);
                    break;
                case JsonContractType.Array:
                    JsonArrayContract arrayContract = (JsonArrayContract)valueContract;
                    this.SerializeList(writer, arrayContract.CreateWrapper(value), arrayContract, member, collectionValueContract);
                    break;
                case JsonContractType.Primitive:
                    this.SerializePrimitive(writer, value, (JsonPrimitiveContract)valueContract, member, collectionValueContract);
                    break;
                case JsonContractType.String:
                    this.SerializeString(writer, value, (JsonStringContract)valueContract);
                    break;
                case JsonContractType.Dictionary:
                    JsonDictionaryContract dictionaryContract = (JsonDictionaryContract)valueContract;
                    this.SerializeDictionary(writer, dictionaryContract.CreateWrapper(value), dictionaryContract, member, collectionValueContract);
                    break;
                    #if !(NET35 || NET20 || WINDOWS_PHONE)
                case JsonContractType.Dynamic:
                    this.SerializeDynamic(writer, (IDynamicMetaObjectProvider)value, (JsonDynamicContract)valueContract);
                    break;
                    #endif
                    #if !SILVERLIGHT && !PocketPC
                case JsonContractType.Serializable:
                    this.SerializeISerializable(writer, (ISerializable)value, (JsonISerializableContract)valueContract, member, collectionValueContract);
                    break;
                    #endif
                case JsonContractType.Linq:
                    ((JToken)value).WriteTo(writer, (this.Serializer.Converters != null) ? this.Serializer.Converters.ToArray() : null);
                    break;
            }
        }
        
        private bool ShouldWriteReference(object value, JsonProperty property, JsonContract contract)
        {
            if (value == null)
            {
                return false;
            }
            if (contract.ContractType == JsonContractType.Primitive || contract.ContractType == JsonContractType.String)
            {
                return false;
            }
            
            bool? isReference = null;
            
            // value could be coming from a dictionary or array and not have a property
            if (property != null)
            {
                isReference = property.IsReference;
            }
            
            if (isReference == null)
            {
                isReference = contract.IsReference;
            }
            
            if (isReference == null)
            {
                if (contract.ContractType == JsonContractType.Array)
                {
                    isReference = this.HasFlag(Serializer.PreserveReferencesHandling, PreserveReferencesHandling.Arrays);
                }
                else
                {
                    isReference = this.HasFlag(Serializer.PreserveReferencesHandling, PreserveReferencesHandling.Objects);
                }
            }
            
            if (!isReference.Value)
            {
                return false;
            }

            return this.Serializer.ReferenceResolver.IsReferenced(this, value);
        }
        
        private void WriteMemberInfoProperty(JsonWriter writer, object memberValue, JsonProperty property, JsonContract contract)
        {
            string propertyName = property.PropertyName;
            object defaultValue = property.DefaultValue;
            
            if (property.NullValueHandling.GetValueOrDefault(this.Serializer.NullValueHandling) == NullValueHandling.Ignore &&
                memberValue == null)
            {
                return;
            }
            
            if (this.HasFlag(property.DefaultValueHandling.GetValueOrDefault(Serializer.DefaultValueHandling), DefaultValueHandling.Ignore) &&
                MiscellaneousUtils.ValueEquals(memberValue, defaultValue))
            {
                return;
            }
            
            if (this.ShouldWriteReference(memberValue, property, contract))
            {
                writer.WritePropertyName(propertyName);
                this.WriteReference(writer, memberValue);
                return;
            }
            
            if (!this.CheckForCircularReference(memberValue, property.ReferenceLoopHandling, contract))
            {
                return;
            }
            
            if (memberValue == null && property.Required == Required.Always)
            {
                throw new JsonSerializationException("Cannot write a null value for property '{0}'. Property requires a value.".FormatWith(CultureInfo.InvariantCulture, property.PropertyName));
            }
            
            writer.WritePropertyName(propertyName);
            this.SerializeValue(writer, memberValue, contract, property, null);
        }
        
        private bool CheckForCircularReference(object value, ReferenceLoopHandling? referenceLoopHandling, JsonContract contract)
        {
            if (value == null || contract.ContractType == JsonContractType.Primitive || contract.ContractType == JsonContractType.String)
            {
                return true;
            }
            
            if (this._serializeStack.IndexOf(value) != -1)
            {
                switch (referenceLoopHandling.GetValueOrDefault(this.Serializer.ReferenceLoopHandling))
                {
                    case ReferenceLoopHandling.Error:
                        throw new JsonSerializationException("Self referencing loop detected for type '{0}'.".FormatWith(CultureInfo.InvariantCulture, value.GetType()));
                    case ReferenceLoopHandling.Ignore:
                        return false;
                    case ReferenceLoopHandling.Serialize:
                        return true;
                    default:
                        throw new InvalidOperationException("Unexpected ReferenceLoopHandling value: '{0}'".FormatWith(CultureInfo.InvariantCulture, this.Serializer.ReferenceLoopHandling));
                }
            }

            return true;
        }
        
        private void WriteReference(JsonWriter writer, object value)
        {
            writer.WriteStartObject();
            writer.WritePropertyName(JsonTypeReflector.RefPropertyName);
            writer.WriteValue(this.Serializer.ReferenceResolver.GetReference(this, value));
            writer.WriteEndObject();
        }
        
        internal static bool TryConvertToString(object value, Type type, out string s)
        {
            #if !PocketPC
            TypeConverter converter = ConvertUtils.GetConverter(type);
            
            // use the objectType's TypeConverter if it has one and can convert to a string
            if (converter != null
                #if !SILVERLIGHT
                &&
                !(converter is ComponentConverter)
                #endif
                &&
                converter.GetType() != typeof(TypeConverter))
            {
                if (converter.CanConvertTo(typeof(string)))
                {
                    #if !SILVERLIGHT
                    s = converter.ConvertToInvariantString(value);
                    #else
          s = converter.ConvertToString(value);
                    #endif
                    return true;
                }
            }
            #endif
            
            #if SILVERLIGHT || PocketPC
      if (value is Guid || value is Uri || value is TimeSpan)
      {
        s = value.ToString();
        return true;
      }
            #endif
            
            if (value is Type)
            {
                s = ((Type)value).AssemblyQualifiedName;
                return true;
            }
            
            s = null;
            return false;
        }
        
        private void SerializeString(JsonWriter writer, object value, JsonStringContract contract)
        {
            contract.InvokeOnSerializing(value, this.Serializer.Context);
            
            string s;
            TryConvertToString(value, contract.UnderlyingType, out s);
            writer.WriteValue(s);

            contract.InvokeOnSerialized(value, this.Serializer.Context);
        }
        
        private void SerializeObject(JsonWriter writer, object value, JsonObjectContract contract, JsonProperty member, JsonContract collectionValueContract)
        {
            contract.InvokeOnSerializing(value, this.Serializer.Context);

            this._serializeStack.Add(value);
            writer.WriteStartObject();
            
            bool isReference = contract.IsReference ?? this.HasFlag(Serializer.PreserveReferencesHandling, PreserveReferencesHandling.Objects);
            if (isReference)
            {
                writer.WritePropertyName(JsonTypeReflector.IdPropertyName);
                writer.WriteValue(this.Serializer.ReferenceResolver.GetReference(this, value));
            }
            if (this.ShouldWriteType(TypeNameHandling.Objects, contract, member, collectionValueContract))
            {
                this.WriteTypeProperty(writer, contract.UnderlyingType);
            }
            
            int initialDepth = writer.Top;
            
            foreach (JsonProperty property in contract.Properties)
            {
                try
                {
                    if (!property.Ignored && property.Readable && this.ShouldSerialize(property, value) && this.IsSpecified(property, value))
                    {
                        if (property.PropertyContract == null)
                        {
                            property.PropertyContract = this.Serializer.ContractResolver.ResolveContract(property.PropertyType);
                        }

                        object memberValue = property.ValueProvider.GetValue(value);
                        JsonContract memberContract = (property.PropertyContract.UnderlyingType.IsSealed) ? property.PropertyContract : this.GetContractSafe(memberValue);
                        
                        this.WriteMemberInfoProperty(writer, memberValue, property, memberContract);
                    }
                }
                catch (Exception ex)
                {
                    if (this.IsErrorHandled(value, contract, property.PropertyName, ex))
                    {
                        this.HandleError(writer, initialDepth);
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            writer.WriteEndObject();
            this._serializeStack.RemoveAt(this._serializeStack.Count - 1);

            contract.InvokeOnSerialized(value, this.Serializer.Context);
        }
        
        private void WriteTypeProperty(JsonWriter writer, Type type)
        {
            writer.WritePropertyName(JsonTypeReflector.TypePropertyName);
            writer.WriteValue(ReflectionUtils.GetTypeName(type, this.Serializer.TypeNameAssemblyFormat, this.Serializer.Binder));
        }
        
        private bool HasFlag(DefaultValueHandling value, DefaultValueHandling flag)
        {
            return ((value & flag) == flag);
        }
        
        private bool HasFlag(PreserveReferencesHandling value, PreserveReferencesHandling flag)
        {
            return ((value & flag) == flag);
        }
        
        private bool HasFlag(TypeNameHandling value, TypeNameHandling flag)
        {
            return ((value & flag) == flag);
        }
        
        private void SerializeConvertable(JsonWriter writer, JsonConverter converter, object value, JsonContract contract)
        {
            if (this.ShouldWriteReference(value, null, contract))
            {
                this.WriteReference(writer, value);
            }
            else
            {
                if (!this.CheckForCircularReference(value, null, contract))
                {
                    return;
                }
                
                this._serializeStack.Add(value);
                
                converter.WriteJson(writer, value, this.GetInternalSerializer());
                
                this._serializeStack.RemoveAt(this._serializeStack.Count - 1);
            }
        }
        
        private void SerializeList(JsonWriter writer, IWrappedCollection values, JsonArrayContract contract, JsonProperty member, JsonContract collectionValueContract)
        {
            contract.InvokeOnSerializing(values.UnderlyingCollection, this.Serializer.Context);
            
            this._serializeStack.Add(values.UnderlyingCollection);

            bool isReference = contract.IsReference ?? this.HasFlag(Serializer.PreserveReferencesHandling, PreserveReferencesHandling.Arrays);
            bool includeTypeDetails = this.ShouldWriteType(TypeNameHandling.Arrays, contract, member, collectionValueContract);
            
            if (isReference || includeTypeDetails)
            {
                writer.WriteStartObject();
                
                if (isReference)
                {
                    writer.WritePropertyName(JsonTypeReflector.IdPropertyName);
                    writer.WriteValue(this.Serializer.ReferenceResolver.GetReference(this, values.UnderlyingCollection));
                }
                if (includeTypeDetails)
                {
                    this.WriteTypeProperty(writer, values.UnderlyingCollection.GetType());
                }
                writer.WritePropertyName(JsonTypeReflector.ArrayValuesPropertyName);
            }
            
            if (contract.CollectionItemContract == null)
            {
                contract.CollectionItemContract = this.Serializer.ContractResolver.ResolveContract(contract.CollectionItemType ?? typeof(object));
            }
            
            JsonContract collectionItemValueContract = (contract.CollectionItemContract.UnderlyingType.IsSealed) ? contract.CollectionItemContract : null;
            
            writer.WriteStartArray();
            
            int initialDepth = writer.Top;
            
            int index = 0;
            // note that an error in the IEnumerable won't be caught
            foreach (object value in values)
            {
                try
                {
                    JsonContract valueContract = collectionItemValueContract ?? this.GetContractSafe(value);
                    
                    if (this.ShouldWriteReference(value, null, valueContract))
                    {
                        this.WriteReference(writer, value);
                    }
                    else
                    {
                        if (this.CheckForCircularReference(value, null, contract))
                        {
                            this.SerializeValue(writer, value, valueContract, null, contract.CollectionItemContract);
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (this.IsErrorHandled(values.UnderlyingCollection, contract, index, ex))
                    {
                        this.HandleError(writer, initialDepth);
                    }
                    else
                    {
                        throw;
                    }
                }
                finally
                {
                    index++;
                }
            }
            
            writer.WriteEndArray();
            
            if (isReference || includeTypeDetails)
            {
                writer.WriteEndObject();
            }
            
            this._serializeStack.RemoveAt(this._serializeStack.Count - 1);

            contract.InvokeOnSerialized(values.UnderlyingCollection, this.Serializer.Context);
        }
        
        #if !SILVERLIGHT && !PocketPC
        #if !NET20
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Portability", "CA1903:UseOnlyApiFromTargetedFramework", MessageId = "System.Security.SecuritySafeCriticalAttribute")]
        [SecuritySafeCritical]
        #endif
        private void SerializeISerializable(JsonWriter writer, ISerializable value, JsonISerializableContract contract, JsonProperty member, JsonContract collectionValueContract)
        {
            contract.InvokeOnSerializing(value, this.Serializer.Context);
            this._serializeStack.Add(value);
            
            writer.WriteStartObject();
            
            if (this.ShouldWriteType(TypeNameHandling.Objects, contract, member, collectionValueContract))
            {
                this.WriteTypeProperty(writer, contract.UnderlyingType);
            }

            SerializationInfo serializationInfo = new SerializationInfo(contract.UnderlyingType, new FormatterConverter());
            value.GetObjectData(serializationInfo, this.Serializer.Context);
            
            foreach (SerializationEntry serializationEntry in serializationInfo)
            {
                writer.WritePropertyName(serializationEntry.Name);
                this.SerializeValue(writer, serializationEntry.Value, this.GetContractSafe(serializationEntry.Value), null, null);
            }
            
            writer.WriteEndObject();
            
            this._serializeStack.RemoveAt(this._serializeStack.Count - 1);
            contract.InvokeOnSerialized(value, this.Serializer.Context);
        }
        
        #endif
        
        #if !(NET35 || NET20 || WINDOWS_PHONE)
        /// <summary>
        /// Serializes the dynamic.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="value">The value.</param>
        /// <param name="contract">The contract.</param>
        private void SerializeDynamic(JsonWriter writer, IDynamicMetaObjectProvider value, JsonDynamicContract contract)
        {
            contract.InvokeOnSerializing(value, this.Serializer.Context);
            this._serializeStack.Add(value);
            
            writer.WriteStartObject();
            
            foreach (string memberName in value.GetDynamicMemberNames())
            {
                object memberValue;
                if (DynamicUtils.TryGetMember(value, memberName, out memberValue))
                {
                    string resolvedPropertyName = (contract.PropertyNameResolver != null)
                                                  ? contract.PropertyNameResolver(memberName)
                                                  : memberName;
                    
                    writer.WritePropertyName(resolvedPropertyName);
                    this.SerializeValue(writer, memberValue, this.GetContractSafe(memberValue), null, null);
                }
            }
            
            writer.WriteEndObject();
            
            this._serializeStack.RemoveAt(this._serializeStack.Count - 1);
            contract.InvokeOnSerialized(value, this.Serializer.Context);
        }
        
        #endif
        
        private bool ShouldWriteType(TypeNameHandling typeNameHandlingFlag, JsonContract contract, JsonProperty member, JsonContract collectionValueContract)
        {
            if (this.HasFlag(((member != null) ? member.TypeNameHandling : null) ?? Serializer.TypeNameHandling, typeNameHandlingFlag))
            {
                return true;
            }
            
            if (member != null)
            {
                if ((member.TypeNameHandling ?? this.Serializer.TypeNameHandling) == TypeNameHandling.Auto
                    // instance and property type are different
                    &&
                    contract.UnderlyingType != member.PropertyType)
                {
                    JsonContract memberTypeContract = this.Serializer.ContractResolver.ResolveContract(member.PropertyType);
                    // instance type and the property's type's contract default type are different (no need to put the type in JSON because the type will be created by default)
                    if (contract.UnderlyingType != memberTypeContract.CreatedType)
                    {
                        return true;
                    }
                }
            }
            else if (collectionValueContract != null)
            {
                if (this.Serializer.TypeNameHandling == TypeNameHandling.Auto && contract.UnderlyingType != collectionValueContract.UnderlyingType)
                {
                    return true;
                }
            }

            return false;
        }
        
        private void SerializeDictionary(JsonWriter writer, IWrappedDictionary values, JsonDictionaryContract contract, JsonProperty member, JsonContract collectionValueContract)
        {
            contract.InvokeOnSerializing(values.UnderlyingDictionary, this.Serializer.Context);

            this._serializeStack.Add(values.UnderlyingDictionary);
            writer.WriteStartObject();
            
            bool isReference = contract.IsReference ?? this.HasFlag(Serializer.PreserveReferencesHandling, PreserveReferencesHandling.Objects);
            if (isReference)
            {
                writer.WritePropertyName(JsonTypeReflector.IdPropertyName);
                writer.WriteValue(this.Serializer.ReferenceResolver.GetReference(this, values.UnderlyingDictionary));
            }
            if (this.ShouldWriteType(TypeNameHandling.Objects, contract, member, collectionValueContract))
            {
                this.WriteTypeProperty(writer, values.UnderlyingDictionary.GetType());
            }
            
            if (contract.DictionaryValueContract == null)
            {
                contract.DictionaryValueContract = this.Serializer.ContractResolver.ResolveContract(contract.DictionaryValueType ?? typeof(object));
            }
            
            JsonContract dictionaryValueContract = (contract.DictionaryValueContract.UnderlyingType.IsSealed) ? contract.DictionaryValueContract : null;
            
            int initialDepth = writer.Top;

            // Mono Unity 3.0 fix
            IDictionary d = values;
            
            foreach (DictionaryEntry entry in d)
            {
                string propertyName = this.GetPropertyName(entry);
                
                propertyName = (contract.PropertyNameResolver != null)
                               ? contract.PropertyNameResolver(propertyName)
                               : propertyName;
                
                try
                {
                    object value = entry.Value;
                    JsonContract valueContract = dictionaryValueContract ?? this.GetContractSafe(value);
                    
                    if (this.ShouldWriteReference(value, null, valueContract))
                    {
                        writer.WritePropertyName(propertyName);
                        this.WriteReference(writer, value);
                    }
                    else
                    {
                        if (!this.CheckForCircularReference(value, null, contract))
                        {
                            continue;
                        }
                        
                        writer.WritePropertyName(propertyName);
                        
                        this.SerializeValue(writer, value, valueContract, null, contract.DictionaryValueContract);
                    }
                }
                catch (Exception ex)
                {
                    if (this.IsErrorHandled(values.UnderlyingDictionary, contract, propertyName, ex))
                    {
                        this.HandleError(writer, initialDepth);
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            writer.WriteEndObject();
            this._serializeStack.RemoveAt(this._serializeStack.Count - 1);

            contract.InvokeOnSerialized(values.UnderlyingDictionary, this.Serializer.Context);
        }
        
        private string GetPropertyName(DictionaryEntry entry)
        {
            string propertyName;
            
            if (entry.Key is IConvertible)
            {
                return Convert.ToString(entry.Key, CultureInfo.InvariantCulture);
            }
            else if (TryConvertToString(entry.Key, entry.Key.GetType(), out propertyName))
            {
                return propertyName;
            }
            else
            {
                return entry.Key.ToString();
            }
        }
        
        private void HandleError(JsonWriter writer, int initialDepth)
        {
            this.ClearErrorContext();
            
            while (writer.Top > initialDepth)
            {
                writer.WriteEnd();
            }
        }
        
        private bool ShouldSerialize(JsonProperty property, object target)
        {
            if (property.ShouldSerialize == null)
            {
                return true;
            }

            return property.ShouldSerialize(target);
        }
        
        private bool IsSpecified(JsonProperty property, object target)
        {
            if (property.GetIsSpecified == null)
            {
                return true;
            }
            
            return property.GetIsSpecified(target);
        }
    }
}