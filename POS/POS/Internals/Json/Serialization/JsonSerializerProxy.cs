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
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using Creek.Data.JSON.Net;
using Lib.JSON.Utilities;

namespace Lib.JSON.Serialization
{
    internal class JsonSerializerProxy : JsonSerializer
    {
        private readonly JsonSerializerInternalReader _serializerReader;
        private readonly JsonSerializerInternalWriter _serializerWriter;
        private readonly JsonSerializer _serializer;
        
        public override event EventHandler<ErrorEventArgs> Error
        {
            add
            {
                this._serializer.Error += value;
            }
            remove
            {
                this._serializer.Error -= value;
            }
        }
        
        public override IReferenceResolver ReferenceResolver
        {
            get
            {
                return this._serializer.ReferenceResolver;
            }
            set
            {
                this._serializer.ReferenceResolver = value;
            }
        }
        
        public override JsonConverterCollection Converters
        {
            get
            {
                return this._serializer.Converters;
            }
        }
        
        public override DefaultValueHandling DefaultValueHandling
        {
            get
            {
                return this._serializer.DefaultValueHandling;
            }
            set
            {
                this._serializer.DefaultValueHandling = value;
            }
        }
        
        public override IContractResolver ContractResolver
        {
            get
            {
                return this._serializer.ContractResolver;
            }
            set
            {
                this._serializer.ContractResolver = value;
            }
        }
        
        public override MissingMemberHandling MissingMemberHandling
        {
            get
            {
                return this._serializer.MissingMemberHandling;
            }
            set
            {
                this._serializer.MissingMemberHandling = value;
            }
        }
        
        public override NullValueHandling NullValueHandling
        {
            get
            {
                return this._serializer.NullValueHandling;
            }
            set
            {
                this._serializer.NullValueHandling = value;
            }
        }
        
        public override ObjectCreationHandling ObjectCreationHandling
        {
            get
            {
                return this._serializer.ObjectCreationHandling;
            }
            set
            {
                this._serializer.ObjectCreationHandling = value;
            }
        }
        
        public override ReferenceLoopHandling ReferenceLoopHandling
        {
            get
            {
                return this._serializer.ReferenceLoopHandling;
            }
            set
            {
                this._serializer.ReferenceLoopHandling = value;
            }
        }
        
        public override PreserveReferencesHandling PreserveReferencesHandling
        {
            get
            {
                return this._serializer.PreserveReferencesHandling;
            }
            set
            {
                this._serializer.PreserveReferencesHandling = value;
            }
        }
        
        public override TypeNameHandling TypeNameHandling
        {
            get
            {
                return this._serializer.TypeNameHandling;
            }
            set
            {
                this._serializer.TypeNameHandling = value;
            }
        }
        
        public override FormatterAssemblyStyle TypeNameAssemblyFormat
        {
            get
            {
                return this._serializer.TypeNameAssemblyFormat;
            }
            set
            {
                this._serializer.TypeNameAssemblyFormat = value;
            }
        }
        
        public override ConstructorHandling ConstructorHandling
        {
            get
            {
                return this._serializer.ConstructorHandling;
            }
            set
            {
                this._serializer.ConstructorHandling = value;
            }
        }
        
        public override SerializationBinder Binder
        {
            get
            {
                return this._serializer.Binder;
            }
            set
            {
                this._serializer.Binder = value;
            }
        }
        
        public override StreamingContext Context
        {
            get
            {
                return this._serializer.Context;
            }
            set
            {
                this._serializer.Context = value;
            }
        }
        
        internal JsonSerializerInternalBase GetInternalSerializer()
        {
            if (this._serializerReader != null)
            {
                return this._serializerReader;
            }
            else
            {
                return this._serializerWriter;
            }
        }
        
        public JsonSerializerProxy(JsonSerializerInternalReader serializerReader)
        {
            ValidationUtils.ArgumentNotNull(serializerReader, "serializerReader");
            
            this._serializerReader = serializerReader;
            this._serializer = serializerReader.Serializer;
        }
        
        public JsonSerializerProxy(JsonSerializerInternalWriter serializerWriter)
        {
            ValidationUtils.ArgumentNotNull(serializerWriter, "serializerWriter");
            
            this._serializerWriter = serializerWriter;
            this._serializer = serializerWriter.Serializer;
        }
        
        internal override object DeserializeInternal(JsonReader reader, Type objectType)
        {
            if (this._serializerReader != null)
            {
                return this._serializerReader.Deserialize(reader, objectType);
            }
            else
            {
                return this._serializer.Deserialize(reader, objectType);
            }
        }
        
        internal override void PopulateInternal(JsonReader reader, object target)
        {
            if (this._serializerReader != null)
            {
                this._serializerReader.Populate(reader, target);
            }
            else
            {
                this._serializer.Populate(reader, target);
            }
        }
        
        internal override void SerializeInternal(JsonWriter jsonWriter, object value)
        {
            if (this._serializerWriter != null)
            {
                this._serializerWriter.Serialize(jsonWriter, value);
            }
            else
            {
                this._serializer.Serialize(jsonWriter, value);
            }
        }
    }
}