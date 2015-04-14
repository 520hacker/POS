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
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Creek.Data.JSON.Net.Schema;
using Lib.JSON.Linq;
using Lib.JSON.Schema;
using Lib.JSON.Utilities;

namespace Lib.JSON
{
    /// <summary>
    /// Represents a reader that provides <see cref="JsonSchema"/> validation.
    /// </summary>
    public class JsonValidatingReader : JsonReader, IJsonLineInfo
    {
        private class SchemaScope
        {
            public string CurrentPropertyName { get; set; }
            
            public int ArrayItemCount { get; set; }
            
            public IList<JsonSchemaModel> Schemas { get; private set; }
            
            public Dictionary<string, bool> RequiredProperties { get; private set; }
            
            public JTokenType TokenType { get; private set; }
            
            public SchemaScope(JTokenType tokenType, IList<JsonSchemaModel> schemas)
            {
                this.TokenType = tokenType;
                this.Schemas = schemas;

                this.RequiredProperties = schemas.SelectMany<JsonSchemaModel, string>(this.GetRequiredProperties).Distinct().ToDictionary(p => p, p => false);
            }
            
            private IEnumerable<string> GetRequiredProperties(JsonSchemaModel schema)
            {
                if (schema == null || schema.Properties == null)
                {
                    return Enumerable.Empty<string>();
                }
                
                return schema.Properties.Where(p => p.Value.Required).Select(p => p.Key);
            }
        }
        
        private readonly Stack<SchemaScope> _stack;
        private JsonSchema _schema;
        private JsonSchemaModel _model;
        private SchemaScope _currentScope;
        
        /// <summary>
        /// Sets an event handler for receiving schema validation errors.
        /// </summary>
        public event ValidationEventHandler ValidationEventHandler;
        
        /// <summary>
        /// Gets the text value of the current Json token.
        /// </summary>
        /// <value></value>
        public override object Value
        {
            get
            {
                return this.Reader.Value;
            }
        }
        
        /// <summary>
        /// Gets the depth of the current token in the JSON document.
        /// </summary>
        /// <value>The depth of the current token in the JSON document.</value>
        public override int Depth
        {
            get
            {
                return this.Reader.Depth;
            }
        }
        
        /// <summary>
        /// Gets the quotation mark character used to enclose the value of a string.
        /// </summary>
        /// <value></value>
        public override char QuoteChar
        {
            get
            {
                return this.Reader.QuoteChar;
            }
            protected internal set
            {
            }
        }
        
        /// <summary>
        /// Gets the type of the current Json token.
        /// </summary>
        /// <value></value>
        public override JsonToken TokenType
        {
            get
            {
                return this.Reader.TokenType;
            }
        }
        
        /// <summary>
        /// Gets the Common Language Runtime (CLR) type for the current Json token.
        /// </summary>
        /// <value></value>
        public override Type ValueType
        {
            get
            {
                return this.Reader.ValueType;
            }
        }
        
        private void Push(SchemaScope scope)
        {
            this._stack.Push(scope);
            this._currentScope = scope;
        }
        
        private SchemaScope Pop()
        {
            SchemaScope poppedScope = this._stack.Pop();
            this._currentScope = (this._stack.Count != 0)
                                 ? this._stack.Peek()
                                 : null;

            return poppedScope;
        }
        
        private IEnumerable<JsonSchemaModel> CurrentSchemas
        {
            get
            {
                return this._currentScope.Schemas;
            }
        }
        
        private IEnumerable<JsonSchemaModel> CurrentMemberSchemas
        {
            get
            {
                if (this._currentScope == null)
                {
                    return new List<JsonSchemaModel>(new[] { this._model });
                }
                
                if (this._currentScope.Schemas == null || this._currentScope.Schemas.Count == 0)
                {
                    return Enumerable.Empty<JsonSchemaModel>();
                }
                
                switch (this._currentScope.TokenType)
                {
                    case JTokenType.None:
                        return this._currentScope.Schemas;
                    case JTokenType.Object:
                        {
                            if (this._currentScope.CurrentPropertyName == null)
                            {
                                throw new Exception("CurrentPropertyName has not been set on scope.");
                            }
                            
                            IList<JsonSchemaModel> schemas = new List<JsonSchemaModel>();
                            
                            foreach (JsonSchemaModel schema in this.CurrentSchemas)
                            {
                                JsonSchemaModel propertySchema;
                                if (schema.Properties != null && schema.Properties.TryGetValue(this._currentScope.CurrentPropertyName, out propertySchema))
                                {
                                    schemas.Add(propertySchema);
                                }
                                if (schema.PatternProperties != null)
                                {
                                    foreach (KeyValuePair<string, JsonSchemaModel> patternProperty in schema.PatternProperties)
                                    {
                                        if (Regex.IsMatch(this._currentScope.CurrentPropertyName, patternProperty.Key))
                                        {
                                            schemas.Add(patternProperty.Value);
                                        }
                                    }
                                }
                                
                                if (schemas.Count == 0 && schema.AllowAdditionalProperties && schema.AdditionalProperties != null)
                                {
                                    schemas.Add(schema.AdditionalProperties);
                                }
                            }
                            
                            return schemas;
                        }
                    case JTokenType.Array:
                        {
                            IList<JsonSchemaModel> schemas = new List<JsonSchemaModel>();
                            
                            foreach (JsonSchemaModel schema in this.CurrentSchemas)
                            {
                                if (!CollectionUtils.IsNullOrEmpty(schema.Items))
                                {
                                    if (schema.Items.Count == 1)
                                    {
                                        schemas.Add(schema.Items[0]);
                                    }
                                    else
                                    {
                                        if (schema.Items.Count > (this._currentScope.ArrayItemCount - 1))
                                        {
                                            schemas.Add(schema.Items[this._currentScope.ArrayItemCount - 1]);
                                        }
                                    }
                                }
                                
                                if (schema.AllowAdditionalProperties && schema.AdditionalProperties != null)
                                {
                                    schemas.Add(schema.AdditionalProperties);
                                }
                            }
                            
                            return schemas;
                        }
                    case JTokenType.Constructor:
                        return Enumerable.Empty<JsonSchemaModel>();
                    default:
                        throw new ArgumentOutOfRangeException("TokenType", "Unexpected token type: {0}".FormatWith(CultureInfo.InvariantCulture, this._currentScope.TokenType));
                }
            }
        }
        
        private void RaiseError(string message, JsonSchemaModel schema)
        {
            IJsonLineInfo lineInfo = this;
            
            string exceptionMessage = (lineInfo.HasLineInfo())
                                      ? string.Format("{0}{1}", message, " Line {0}, position {1}.".FormatWith(CultureInfo.InvariantCulture, lineInfo.LineNumber, lineInfo.LinePosition))
                                      : message;

            this.OnValidationEvent(new JsonSchemaException(exceptionMessage, null, lineInfo.LineNumber, lineInfo.LinePosition));
        }
        
        private void OnValidationEvent(JsonSchemaException exception)
        {
            ValidationEventHandler handler = this.ValidationEventHandler;
            if (handler != null)
            {
                handler(this, new ValidationEventArgs(exception));
            }
            else
            {
                throw exception;
            }
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonValidatingReader"/> class that
        /// validates the content returned from the given <see cref="JsonReader"/>.
        /// </summary>
        /// <param name="reader">The <see cref="JsonReader"/> to read from while validating.</param>
        public JsonValidatingReader(JsonReader reader)
        {
            ValidationUtils.ArgumentNotNull(reader, "reader");
            this.Reader = reader;
            this._stack = new Stack<SchemaScope>();
        }
        
        /// <summary>
        /// Gets or sets the schema.
        /// </summary>
        /// <value>The schema.</value>
        public JsonSchema Schema
        {
            get
            {
                return this._schema;
            }
            set
            {
                if (this.TokenType != JsonToken.None)
                {
                    throw new Exception("Cannot change schema while validating JSON.");
                }
                
                this._schema = value;
                this._model = null;
            }
        }
        
        /// <summary>
        /// Gets the <see cref="JsonReader"/> used to construct this <see cref="JsonValidatingReader"/>.
        /// </summary>
        /// <value>The <see cref="JsonReader"/> specified in the constructor.</value>
        public JsonReader Reader { get; private set; }
        
        private void ValidateInEnumAndNotDisallowed(JsonSchemaModel schema)
        {
            if (schema == null)
            {
                return;
            }
            
            JToken value = new JValue(this.Reader.Value);
            
            if (schema.Enum != null)
            {
                StringWriter sw = new StringWriter(CultureInfo.InvariantCulture);
                value.WriteTo(new JsonTextWriter(sw));
                
                if (!schema.Enum.ContainsValue(value, new JTokenEqualityComparer()))
                {
                    this.RaiseError("Value {0} is not defined in enum.".FormatWith(CultureInfo.InvariantCulture, sw.ToString()),
                        schema);
                }
            }
            
            JsonSchemaType? currentNodeType = this.GetCurrentNodeSchemaType();
            if (currentNodeType != null)
            {
                if (JsonSchemaGenerator.HasFlag(schema.Disallow, currentNodeType.Value))
                {
                    this.RaiseError("Type {0} is disallowed.".FormatWith(CultureInfo.InvariantCulture, currentNodeType), schema);
                }
            }
        }
        
        private JsonSchemaType? GetCurrentNodeSchemaType()
        {
            switch (this.Reader.TokenType)
            {
                case JsonToken.StartObject:
                    return JsonSchemaType.Object;
                case JsonToken.StartArray:
                    return JsonSchemaType.Array;
                case JsonToken.Integer:
                    return JsonSchemaType.Integer;
                case JsonToken.Float:
                    return JsonSchemaType.Float;
                case JsonToken.String:
                    return JsonSchemaType.String;
                case JsonToken.Boolean:
                    return JsonSchemaType.Boolean;
                case JsonToken.Null:
                    return JsonSchemaType.Null;
                default:
                    return null;
            }
        }
        
        /// <summary>
        /// Reads the next JSON token from the stream as a <see cref="Nullable{Int32}"/>.
        /// </summary>
        /// <returns>A <see cref="Nullable{Int32}"/>.</returns>
        public override int? ReadAsInt32()
        {
            int? i = this.Reader.ReadAsInt32();
            
            this.ValidateCurrentToken();
            return i;
        }
        
        /// <summary>
        /// Reads the next JSON token from the stream as a <see cref="T:Byte[]"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:Byte[]"/> or a null reference if the next JSON token is null.
        /// </returns>
        public override byte[] ReadAsBytes()
        {
            byte[] data = this.Reader.ReadAsBytes();
            
            this.ValidateCurrentToken();
            return data;
        }
        
        /// <summary>
        /// Reads the next JSON token from the stream as a <see cref="Nullable{Decimal}"/>.
        /// </summary>
        /// <returns>A <see cref="Nullable{Decimal}"/>.</returns>
        public override decimal? ReadAsDecimal()
        {
            decimal? d = this.Reader.ReadAsDecimal();
            
            this.ValidateCurrentToken();
            return d;
        }
        
        #if !NET20
        /// <summary>
        /// Reads the next JSON token from the stream as a <see cref="Nullable{DateTimeOffset}"/>.
        /// </summary>
        /// <returns>A <see cref="Nullable{DateTimeOffset}"/>.</returns>
        public override DateTimeOffset? ReadAsDateTimeOffset()
        {
            DateTimeOffset? dateTimeOffset = this.Reader.ReadAsDateTimeOffset();
            
            this.ValidateCurrentToken();
            return dateTimeOffset;
        }
        
        #endif
        
        /// <summary>
        /// Reads the next JSON token from the stream.
        /// </summary>
        /// <returns>
        /// true if the next token was read successfully; false if there are no more tokens to read.
        /// </returns>
        public override bool Read()
        {
            if (!this.Reader.Read())
            {
                return false;
            }
            
            if (this.Reader.TokenType == JsonToken.Comment)
            {
                return true;
            }
            
            this.ValidateCurrentToken();
            return true;
        }
        
        private void ValidateCurrentToken()
        {
            // first time validate has been called. build model
            if (this._model == null)
            {
                JsonSchemaModelBuilder builder = new JsonSchemaModelBuilder();
                this._model = builder.Build(this._schema);
            }
            
            switch (this.Reader.TokenType)
            {
                case JsonToken.StartObject:
                    this.ProcessValue();
                    IList<JsonSchemaModel> objectSchemas = this.CurrentMemberSchemas.Where(this.ValidateObject).ToList();
                    this.Push(new SchemaScope(JTokenType.Object, objectSchemas));
                    break;
                case JsonToken.StartArray:
                    this.ProcessValue();
                    IList<JsonSchemaModel> arraySchemas = this.CurrentMemberSchemas.Where(this.ValidateArray).ToList();
                    this.Push(new SchemaScope(JTokenType.Array, arraySchemas));
                    break;
                case JsonToken.StartConstructor:
                    this.Push(new SchemaScope(JTokenType.Constructor, null));
                    break;
                case JsonToken.PropertyName:
                    foreach (JsonSchemaModel schema in this.CurrentSchemas)
                    {
                        this.ValidatePropertyName(schema);
                    }
                    break;
                case JsonToken.Raw:
                    break;
                case JsonToken.Integer:
                    this.ProcessValue();
                    foreach (JsonSchemaModel schema in this.CurrentMemberSchemas)
                    {
                        this.ValidateInteger(schema);
                    }
                    break;
                case JsonToken.Float:
                    this.ProcessValue();
                    foreach (JsonSchemaModel schema in this.CurrentMemberSchemas)
                    {
                        this.ValidateFloat(schema);
                    }
                    break;
                case JsonToken.String:
                    this.ProcessValue();
                    foreach (JsonSchemaModel schema in this.CurrentMemberSchemas)
                    {
                        this.ValidateString(schema);
                    }
                    break;
                case JsonToken.Boolean:
                    this.ProcessValue();
                    foreach (JsonSchemaModel schema in this.CurrentMemberSchemas)
                    {
                        this.ValidateBoolean(schema);
                    }
                    break;
                case JsonToken.Null:
                    this.ProcessValue();
                    foreach (JsonSchemaModel schema in this.CurrentMemberSchemas)
                    {
                        this.ValidateNull(schema);
                    }
                    break;
                case JsonToken.Undefined:
                    break;
                case JsonToken.EndObject:
                    foreach (JsonSchemaModel schema in this.CurrentSchemas)
                    {
                        this.ValidateEndObject(schema);
                    }
                    this.Pop();
                    break;
                case JsonToken.EndArray:
                    foreach (JsonSchemaModel schema in this.CurrentSchemas)
                    {
                        this.ValidateEndArray(schema);
                    }
                    this.Pop();
                    break;
                case JsonToken.EndConstructor:
                    this.Pop();
                    break;
                case JsonToken.Date:
                case JsonToken.Bytes:
                    // these have no equivalent in JSON schema
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private void ValidateEndObject(JsonSchemaModel schema)
        {
            if (schema == null)
            {
                return;
            }
            
            Dictionary<string, bool> requiredProperties = this._currentScope.RequiredProperties;
            
            if (requiredProperties != null)
            {
                List<string> unmatchedRequiredProperties =
                    requiredProperties.Where(kv => !kv.Value).Select(kv => kv.Key).ToList();
                
                if (unmatchedRequiredProperties.Count > 0)
                {
                    this.RaiseError("Required properties are missing from object: {0}.".FormatWith(CultureInfo.InvariantCulture, string.Join(", ", unmatchedRequiredProperties.ToArray())), schema);
                }
            }
        }
        
        private void ValidateEndArray(JsonSchemaModel schema)
        {
            if (schema == null)
            {
                return;
            }
            
            int arrayItemCount = this._currentScope.ArrayItemCount;
            
            if (schema.MaximumItems != null && arrayItemCount > schema.MaximumItems)
            {
                this.RaiseError("Array item count {0} exceeds maximum count of {1}.".FormatWith(CultureInfo.InvariantCulture, arrayItemCount, schema.MaximumItems), schema);
            }
            
            if (schema.MinimumItems != null && arrayItemCount < schema.MinimumItems)
            {
                this.RaiseError("Array item count {0} is less than minimum count of {1}.".FormatWith(CultureInfo.InvariantCulture, arrayItemCount, schema.MinimumItems), schema);
            }
        }
        
        private void ValidateNull(JsonSchemaModel schema)
        {
            if (schema == null)
            {
                return;
            }
            
            if (!this.TestType(schema, JsonSchemaType.Null))
            {
                return;
            }

            this.ValidateInEnumAndNotDisallowed(schema);
        }
        
        private void ValidateBoolean(JsonSchemaModel schema)
        {
            if (schema == null)
            {
                return;
            }
            
            if (!this.TestType(schema, JsonSchemaType.Boolean))
            {
                return;
            }

            this.ValidateInEnumAndNotDisallowed(schema);
        }
        
        private void ValidateString(JsonSchemaModel schema)
        {
            if (schema == null)
            {
                return;
            }
            
            if (!this.TestType(schema, JsonSchemaType.String))
            {
                return;
            }
            
            this.ValidateInEnumAndNotDisallowed(schema);
            
            string value = this.Reader.Value.ToString();
            
            if (schema.MaximumLength != null && value.Length > schema.MaximumLength)
            {
                this.RaiseError("String '{0}' exceeds maximum length of {1}.".FormatWith(CultureInfo.InvariantCulture, value, schema.MaximumLength), schema);
            }
            
            if (schema.MinimumLength != null && value.Length < schema.MinimumLength)
            {
                this.RaiseError("String '{0}' is less than minimum length of {1}.".FormatWith(CultureInfo.InvariantCulture, value, schema.MinimumLength), schema);
            }
            
            if (schema.Patterns != null)
            {
                foreach (string pattern in schema.Patterns)
                {
                    if (!Regex.IsMatch(value, pattern))
                    {
                        this.RaiseError("String '{0}' does not match regex pattern '{1}'.".FormatWith(CultureInfo.InvariantCulture, value, pattern), schema);
                    }
                }
            }
        }
        
        private void ValidateInteger(JsonSchemaModel schema)
        {
            if (schema == null)
            {
                return;
            }
            
            if (!this.TestType(schema, JsonSchemaType.Integer))
            {
                return;
            }
            
            this.ValidateInEnumAndNotDisallowed(schema);
            
            long value = Convert.ToInt64(this.Reader.Value, CultureInfo.InvariantCulture);
            
            if (schema.Maximum != null)
            {
                if (value > schema.Maximum)
                {
                    this.RaiseError("Integer {0} exceeds maximum value of {1}.".FormatWith(CultureInfo.InvariantCulture, value, schema.Maximum), schema);
                }
                if (schema.ExclusiveMaximum && value == schema.Maximum)
                {
                    this.RaiseError("Integer {0} equals maximum value of {1} and exclusive maximum is true.".FormatWith(CultureInfo.InvariantCulture, value, schema.Maximum), schema);
                }
            }
            
            if (schema.Minimum != null)
            {
                if (value < schema.Minimum)
                {
                    this.RaiseError("Integer {0} is less than minimum value of {1}.".FormatWith(CultureInfo.InvariantCulture, value, schema.Minimum), schema);
                }
                if (schema.ExclusiveMinimum && value == schema.Minimum)
                {
                    this.RaiseError("Integer {0} equals minimum value of {1} and exclusive minimum is true.".FormatWith(CultureInfo.InvariantCulture, value, schema.Minimum), schema);
                }
            }
            
            if (schema.DivisibleBy != null && !IsZero(value % schema.DivisibleBy.Value))
            {
                this.RaiseError("Integer {0} is not evenly divisible by {1}.".FormatWith(CultureInfo.InvariantCulture, JsonConvert.ToString(value), schema.DivisibleBy), schema);
            }
        }
        
        private void ProcessValue()
        {
            if (this._currentScope != null && this._currentScope.TokenType == JTokenType.Array)
            {
                this._currentScope.ArrayItemCount++;
                
                foreach (JsonSchemaModel currentSchema in this.CurrentSchemas)
                {
                    if (currentSchema != null && currentSchema.Items != null && currentSchema.Items.Count > 1 && this._currentScope.ArrayItemCount >= currentSchema.Items.Count)
                    {
                        this.RaiseError("Index {0} has not been defined and the schema does not allow additional items.".FormatWith(CultureInfo.InvariantCulture, _currentScope.ArrayItemCount), currentSchema);
                    }
                }
            }
        }
        
        private void ValidateFloat(JsonSchemaModel schema)
        {
            if (schema == null)
            {
                return;
            }
            
            if (!this.TestType(schema, JsonSchemaType.Float))
            {
                return;
            }
            
            this.ValidateInEnumAndNotDisallowed(schema);
            
            double value = Convert.ToDouble(this.Reader.Value, CultureInfo.InvariantCulture);
            
            if (schema.Maximum != null)
            {
                if (value > schema.Maximum)
                {
                    this.RaiseError("Float {0} exceeds maximum value of {1}.".FormatWith(CultureInfo.InvariantCulture, JsonConvert.ToString(value), schema.Maximum), schema);
                }
                if (schema.ExclusiveMaximum && value == schema.Maximum)
                {
                    this.RaiseError("Float {0} equals maximum value of {1} and exclusive maximum is true.".FormatWith(CultureInfo.InvariantCulture, JsonConvert.ToString(value), schema.Maximum), schema);
                }
            }
            
            if (schema.Minimum != null)
            {
                if (value < schema.Minimum)
                {
                    this.RaiseError("Float {0} is less than minimum value of {1}.".FormatWith(CultureInfo.InvariantCulture, JsonConvert.ToString(value), schema.Minimum), schema);
                }
                if (schema.ExclusiveMinimum && value == schema.Minimum)
                {
                    this.RaiseError("Float {0} equals minimum value of {1} and exclusive minimum is true.".FormatWith(CultureInfo.InvariantCulture, JsonConvert.ToString(value), schema.Minimum), schema);
                }
            }
            
            if (schema.DivisibleBy != null && !IsZero(value % schema.DivisibleBy.Value))
            {
                this.RaiseError("Float {0} is not evenly divisible by {1}.".FormatWith(CultureInfo.InvariantCulture, JsonConvert.ToString(value), schema.DivisibleBy), schema);
            }
        }
        
        private static bool IsZero(double value)
        {
            const double epsilon = 2.2204460492503131e-016;

            return Math.Abs(value) < 10.0 * epsilon;
        }
        
        private void ValidatePropertyName(JsonSchemaModel schema)
        {
            if (schema == null)
            {
                return;
            }
            
            string propertyName = Convert.ToString(this.Reader.Value, CultureInfo.InvariantCulture);
            
            if (this._currentScope.RequiredProperties.ContainsKey(propertyName))
            {
                this._currentScope.RequiredProperties[propertyName] = true;
            }
            
            if (!schema.AllowAdditionalProperties)
            {
                bool propertyDefinied = this.IsPropertyDefinied(schema, propertyName);
                
                if (!propertyDefinied)
                {
                    this.RaiseError("Property '{0}' has not been defined and the schema does not allow additional properties.".FormatWith(CultureInfo.InvariantCulture, propertyName), schema);
                }
            }

            this._currentScope.CurrentPropertyName = propertyName;
        }
        
        private bool IsPropertyDefinied(JsonSchemaModel schema, string propertyName)
        {
            if (schema.Properties != null && schema.Properties.ContainsKey(propertyName))
            {
                return true;
            }
            
            if (schema.PatternProperties != null)
            {
                foreach (string pattern in schema.PatternProperties.Keys)
                {
                    if (Regex.IsMatch(propertyName, pattern))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        
        private bool ValidateArray(JsonSchemaModel schema)
        {
            if (schema == null)
            {
                return true;
            }

            return (this.TestType(schema, JsonSchemaType.Array));
        }
        
        private bool ValidateObject(JsonSchemaModel schema)
        {
            if (schema == null)
            {
                return true;
            }

            return (this.TestType(schema, JsonSchemaType.Object));
        }
        
        private bool TestType(JsonSchemaModel currentSchema, JsonSchemaType currentType)
        {
            if (!JsonSchemaGenerator.HasFlag(currentSchema.Type, currentType))
            {
                this.RaiseError("Invalid type. Expected {0} but got {1}.".FormatWith(CultureInfo.InvariantCulture, currentSchema.Type, currentType), currentSchema);
                return false;
            }

            return true;
        }
        
        bool IJsonLineInfo.HasLineInfo()
        {
            IJsonLineInfo lineInfo = this.Reader as IJsonLineInfo;
            return lineInfo != null && lineInfo.HasLineInfo();
        }
        
        int IJsonLineInfo.LineNumber
        {
            get
            {
                IJsonLineInfo lineInfo = this.Reader as IJsonLineInfo;
                return (lineInfo != null) ? lineInfo.LineNumber : 0;
            }
        }
        
        int IJsonLineInfo.LinePosition
        {
            get
            {
                IJsonLineInfo lineInfo = this.Reader as IJsonLineInfo;
                return (lineInfo != null) ? lineInfo.LinePosition : 0;
            }
        }
    }
}