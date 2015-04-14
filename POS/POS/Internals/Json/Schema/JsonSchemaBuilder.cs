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
using System.Linq;
using Lib.JSON.Linq;
using Lib.JSON.Utilities;

namespace Lib.JSON.Schema
{
    internal class JsonSchemaBuilder
    {
        private JsonReader _reader;
        private readonly IList<JsonSchema> _stack;
        private readonly JsonSchemaResolver _resolver;
        
        private void Push(JsonSchema value)
        {
            this.CurrentSchema = value;
            this._stack.Add(value);
            this._resolver.LoadedSchemas.Add(value);
        }
        
        private JsonSchema Pop()
        {
            JsonSchema poppedSchema = this.CurrentSchema;
            this._stack.RemoveAt(this._stack.Count - 1);
            this.CurrentSchema = this._stack.LastOrDefault();

            return poppedSchema;
        }
        
        private JsonSchema CurrentSchema { get; set; }
        
        public JsonSchemaBuilder(JsonSchemaResolver resolver)
        {
            this._stack = new List<JsonSchema>();
            this._resolver = resolver;
        }
        
        internal JsonSchema Parse(JsonReader reader)
        {
            this._reader = reader;
            
            if (reader.TokenType == JsonToken.None)
            {
                this._reader.Read();
            }

            return this.BuildSchema();
        }
        
        private JsonSchema BuildSchema()
        {
            if (this._reader.TokenType != JsonToken.StartObject)
            {
                throw new Exception("Expected StartObject while parsing schema object, got {0}.".FormatWith(CultureInfo.InvariantCulture, this._reader.TokenType));
            }
            
            this._reader.Read();
            // empty schema object
            if (this._reader.TokenType == JsonToken.EndObject)
            {
                this.Push(new JsonSchema());
                return this.Pop();
            }

            string propertyName = Convert.ToString(this._reader.Value, CultureInfo.InvariantCulture);
            this._reader.Read();
            
            // schema reference
            if (propertyName == JsonSchemaConstants.ReferencePropertyName)
            {
                string id = (string)this._reader.Value;
                
                // skip to the end of the current object
                while (this._reader.Read() && this._reader.TokenType != JsonToken.EndObject)
                {
                    if (this._reader.TokenType == JsonToken.StartObject)
                    {
                        throw new Exception("Found StartObject within the schema reference with the Id '{0}'"
                                                                                                             .FormatWith(CultureInfo.InvariantCulture, id));
                    }
                }
                
                JsonSchema referencedSchema = this._resolver.GetSchema(id);
                if (referencedSchema == null)
                {
                    throw new Exception("Could not resolve schema reference for Id '{0}'.".FormatWith(CultureInfo.InvariantCulture, id));
                }

                return referencedSchema;
            }

            // regular ol' schema object
            this.Push(new JsonSchema());
            
            this.ProcessSchemaProperty(propertyName);
            
            while (this._reader.Read() && this._reader.TokenType != JsonToken.EndObject)
            {
                propertyName = Convert.ToString(this._reader.Value, CultureInfo.InvariantCulture);
                this._reader.Read();

                this.ProcessSchemaProperty(propertyName);
            }

            return this.Pop();
        }
        
        private void ProcessSchemaProperty(string propertyName)
        {
            switch (propertyName)
            {
                case JsonSchemaConstants.TypePropertyName:
                    this.CurrentSchema.Type = this.ProcessType();
                    break;
                case JsonSchemaConstants.IdPropertyName:
                    this.CurrentSchema.Id = (string)this._reader.Value;
                    break;
                case JsonSchemaConstants.TitlePropertyName:
                    this.CurrentSchema.Title = (string)this._reader.Value;
                    break;
                case JsonSchemaConstants.DescriptionPropertyName:
                    this.CurrentSchema.Description = (string)this._reader.Value;
                    break;
                case JsonSchemaConstants.PropertiesPropertyName:
                    this.ProcessProperties();
                    break;
                case JsonSchemaConstants.ItemsPropertyName:
                    this.ProcessItems();
                    break;
                case JsonSchemaConstants.AdditionalPropertiesPropertyName:
                    this.ProcessAdditionalProperties();
                    break;
                case JsonSchemaConstants.PatternPropertiesPropertyName:
                    this.ProcessPatternProperties();
                    break;
                case JsonSchemaConstants.RequiredPropertyName:
                    this.CurrentSchema.Required = (bool)this._reader.Value;
                    break;
                case JsonSchemaConstants.RequiresPropertyName:
                    this.CurrentSchema.Requires = (string)this._reader.Value;
                    break;
                case JsonSchemaConstants.IdentityPropertyName:
                    this.ProcessIdentity();
                    break;
                case JsonSchemaConstants.MinimumPropertyName:
                    this.CurrentSchema.Minimum = Convert.ToDouble(this._reader.Value, CultureInfo.InvariantCulture);
                    break;
                case JsonSchemaConstants.MaximumPropertyName:
                    this.CurrentSchema.Maximum = Convert.ToDouble(this._reader.Value, CultureInfo.InvariantCulture);
                    break;
                case JsonSchemaConstants.ExclusiveMinimumPropertyName:
                    this.CurrentSchema.ExclusiveMinimum = (bool)this._reader.Value;
                    break;
                case JsonSchemaConstants.ExclusiveMaximumPropertyName:
                    this.CurrentSchema.ExclusiveMaximum = (bool)this._reader.Value;
                    break;
                case JsonSchemaConstants.MaximumLengthPropertyName:
                    this.CurrentSchema.MaximumLength = Convert.ToInt32(this._reader.Value, CultureInfo.InvariantCulture);
                    break;
                case JsonSchemaConstants.MinimumLengthPropertyName:
                    this.CurrentSchema.MinimumLength = Convert.ToInt32(this._reader.Value, CultureInfo.InvariantCulture);
                    break;
                case JsonSchemaConstants.MaximumItemsPropertyName:
                    this.CurrentSchema.MaximumItems = Convert.ToInt32(this._reader.Value, CultureInfo.InvariantCulture);
                    break;
                case JsonSchemaConstants.MinimumItemsPropertyName:
                    this.CurrentSchema.MinimumItems = Convert.ToInt32(this._reader.Value, CultureInfo.InvariantCulture);
                    break;
                case JsonSchemaConstants.DivisibleByPropertyName:
                    this.CurrentSchema.DivisibleBy = Convert.ToDouble(this._reader.Value, CultureInfo.InvariantCulture);
                    break;
                case JsonSchemaConstants.DisallowPropertyName:
                    this.CurrentSchema.Disallow = this.ProcessType();
                    break;
                case JsonSchemaConstants.DefaultPropertyName:
                    this.ProcessDefault();
                    break;
                case JsonSchemaConstants.HiddenPropertyName:
                    this.CurrentSchema.Hidden = (bool)this._reader.Value;
                    break;
                case JsonSchemaConstants.ReadOnlyPropertyName:
                    this.CurrentSchema.ReadOnly = (bool)this._reader.Value;
                    break;
                case JsonSchemaConstants.FormatPropertyName:
                    this.CurrentSchema.Format = (string)this._reader.Value;
                    break;
                case JsonSchemaConstants.PatternPropertyName:
                    this.CurrentSchema.Pattern = (string)this._reader.Value;
                    break;
                case JsonSchemaConstants.OptionsPropertyName:
                    this.ProcessOptions();
                    break;
                case JsonSchemaConstants.EnumPropertyName:
                    this.ProcessEnum();
                    break;
                case JsonSchemaConstants.ExtendsPropertyName:
                    this.ProcessExtends();
                    break;
                default:
                    this._reader.Skip();
                    break;
            }
        }
        
        private void ProcessExtends()
        {
            this.CurrentSchema.Extends = this.BuildSchema();
        }
        
        private void ProcessEnum()
        {
            if (this._reader.TokenType != JsonToken.StartArray)
            {
                throw new Exception("Expected StartArray token while parsing enum values, got {0}.".FormatWith(CultureInfo.InvariantCulture, this._reader.TokenType));
            }
            
            this.CurrentSchema.Enum = new List<JToken>();
            
            while (this._reader.Read() && this._reader.TokenType != JsonToken.EndArray)
            {
                JToken value = JToken.ReadFrom(this._reader);
                this.CurrentSchema.Enum.Add(value);
            }
        }
        
        private void ProcessOptions()
        {
            this.CurrentSchema.Options = new Dictionary<JToken, string>(new JTokenEqualityComparer());
            
            switch (this._reader.TokenType)
            {
                case JsonToken.StartArray:
                    while (this._reader.Read() && this._reader.TokenType != JsonToken.EndArray)
                    {
                        if (this._reader.TokenType != JsonToken.StartObject)
                        {
                            throw new Exception("Expect object token, got {0}.".FormatWith(CultureInfo.InvariantCulture, this._reader.TokenType));
                        }

                        string label = null;
                        JToken value = null;
                        
                        while (this._reader.Read() && this._reader.TokenType != JsonToken.EndObject)
                        {
                            string propertyName = Convert.ToString(this._reader.Value, CultureInfo.InvariantCulture);
                            this._reader.Read();
                            
                            switch (propertyName)
                            {
                                case JsonSchemaConstants.OptionValuePropertyName:
                                    value = JToken.ReadFrom(this._reader);
                                    break;
                                case JsonSchemaConstants.OptionLabelPropertyName:
                                    label = (string)this._reader.Value;
                                    break;
                                default:
                                    throw new Exception("Unexpected property in JSON schema option: {0}.".FormatWith(CultureInfo.InvariantCulture, propertyName));
                            }
                        }
                        
                        if (value == null)
                        {
                            throw new Exception("No value specified for JSON schema option.");
                        }
                        
                        if (this.CurrentSchema.Options.ContainsKey(value))
                        {
                            throw new Exception("Duplicate value in JSON schema option collection: {0}".FormatWith(CultureInfo.InvariantCulture, value));
                        }
                        
                        this.CurrentSchema.Options.Add(value, label);
                    }
                    break;
                default:
                    throw new Exception("Expected array token, got {0}.".FormatWith(CultureInfo.InvariantCulture, this._reader.TokenType));
            }
        }
        
        private void ProcessDefault()
        {
            this.CurrentSchema.Default = JToken.ReadFrom(this._reader);
        }
        
        private void ProcessIdentity()
        {
            this.CurrentSchema.Identity = new List<string>();
            
            switch (this._reader.TokenType)
            {
                case JsonToken.String:
                    this.CurrentSchema.Identity.Add(this._reader.Value.ToString());
                    break;
                case JsonToken.StartArray:
                    while (this._reader.Read() && this._reader.TokenType != JsonToken.EndArray)
                    {
                        if (this._reader.TokenType != JsonToken.String)
                        {
                            throw new Exception("Exception JSON property name string token, got {0}.".FormatWith(CultureInfo.InvariantCulture, this._reader.TokenType));
                        }
                        
                        this.CurrentSchema.Identity.Add(this._reader.Value.ToString());
                    }
                    break;
                default:
                    throw new Exception("Expected array or JSON property name string token, got {0}.".FormatWith(CultureInfo.InvariantCulture, this._reader.TokenType));
            }
        }
        
        private void ProcessAdditionalProperties()
        {
            if (this._reader.TokenType == JsonToken.Boolean)
            {
                this.CurrentSchema.AllowAdditionalProperties = (bool)this._reader.Value;
            }
            else
            {
                this.CurrentSchema.AdditionalProperties = this.BuildSchema();
            }
        }
        
        private void ProcessPatternProperties()
        {
            Dictionary<string, JsonSchema> patternProperties = new Dictionary<string, JsonSchema>();
            
            if (this._reader.TokenType != JsonToken.StartObject)
            {
                throw new Exception("Expected start object token.");
            }
            
            while (this._reader.Read() && this._reader.TokenType != JsonToken.EndObject)
            {
                string propertyName = Convert.ToString(this._reader.Value, CultureInfo.InvariantCulture);
                this._reader.Read();
                
                if (patternProperties.ContainsKey(propertyName))
                {
                    throw new Exception("Property {0} has already been defined in schema.".FormatWith(CultureInfo.InvariantCulture, propertyName));
                }

                patternProperties.Add(propertyName, this.BuildSchema());
            }

            this.CurrentSchema.PatternProperties = patternProperties;
        }
        
        private void ProcessItems()
        {
            this.CurrentSchema.Items = new List<JsonSchema>();
            
            switch (this._reader.TokenType)
            {
                case JsonToken.StartObject:
                    this.CurrentSchema.Items.Add(this.BuildSchema());
                    break;
                case JsonToken.StartArray:
                    while (this._reader.Read() && this._reader.TokenType != JsonToken.EndArray)
                    {
                        this.CurrentSchema.Items.Add(this.BuildSchema());
                    }
                    break;
                default:
                    throw new Exception("Expected array or JSON schema object token, got {0}.".FormatWith(CultureInfo.InvariantCulture, this._reader.TokenType));
            }
        }
        
        private void ProcessProperties()
        {
            IDictionary<string, JsonSchema> properties = new Dictionary<string, JsonSchema>();
            
            if (this._reader.TokenType != JsonToken.StartObject)
            {
                throw new Exception("Expected StartObject token while parsing schema properties, got {0}.".FormatWith(CultureInfo.InvariantCulture, this._reader.TokenType));
            }
            
            while (this._reader.Read() && this._reader.TokenType != JsonToken.EndObject)
            {
                string propertyName = Convert.ToString(this._reader.Value, CultureInfo.InvariantCulture);
                this._reader.Read();
                
                if (properties.ContainsKey(propertyName))
                {
                    throw new Exception("Property {0} has already been defined in schema.".FormatWith(CultureInfo.InvariantCulture, propertyName));
                }

                properties.Add(propertyName, this.BuildSchema());
            }

            this.CurrentSchema.Properties = properties;
        }
        
        private JsonSchemaType? ProcessType()
        {
            switch (this._reader.TokenType)
            {
                case JsonToken.String:
                    return MapType(this._reader.Value.ToString());
                case JsonToken.StartArray:
                    // ensure type is in blank state before ORing values
                    JsonSchemaType? type = JsonSchemaType.None;
                    
                    while (this._reader.Read() && this._reader.TokenType != JsonToken.EndArray)
                    {
                        if (this._reader.TokenType != JsonToken.String)
                        {
                            throw new Exception("Exception JSON schema type string token, got {0}.".FormatWith(CultureInfo.InvariantCulture, this._reader.TokenType));
                        }

                        type = type | MapType(this._reader.Value.ToString());
                    }
                    
                    return type;
                default:
                    throw new Exception("Expected array or JSON schema type string token, got {0}.".FormatWith(CultureInfo.InvariantCulture, this._reader.TokenType));
            }
        }
        
        internal static JsonSchemaType MapType(string type)
        {
            JsonSchemaType mappedType;
            if (!JsonSchemaConstants.JsonSchemaTypeMapping.TryGetValue(type, out mappedType))
            {
                throw new Exception("Invalid JSON schema type: {0}".FormatWith(CultureInfo.InvariantCulture, type));
            }

            return mappedType;
        }
        
        internal static string MapType(JsonSchemaType type)
        {
            return JsonSchemaConstants.JsonSchemaTypeMapping.Single(kv => kv.Value == type).Key;
        }
    }
}