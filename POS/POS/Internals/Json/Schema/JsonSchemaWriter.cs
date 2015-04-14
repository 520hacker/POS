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
using System.Linq;
using Lib.JSON.Linq;
using Lib.JSON.Utilities;

namespace Lib.JSON.Schema
{
    internal class JsonSchemaWriter
    {
        private readonly JsonWriter _writer;
        private readonly JsonSchemaResolver _resolver;
        
        public JsonSchemaWriter(JsonWriter writer, JsonSchemaResolver resolver)
        {
            ValidationUtils.ArgumentNotNull(writer, "writer");
            this._writer = writer;
            this._resolver = resolver;
        }
        
        private void ReferenceOrWriteSchema(JsonSchema schema)
        {
            if (schema.Id != null && this._resolver.GetSchema(schema.Id) != null)
            {
                this._writer.WriteStartObject();
                this._writer.WritePropertyName(JsonSchemaConstants.ReferencePropertyName);
                this._writer.WriteValue(schema.Id);
                this._writer.WriteEndObject();
            }
            else
            {
                this.WriteSchema(schema);
            }
        }
        
        public void WriteSchema(JsonSchema schema)
        {
            ValidationUtils.ArgumentNotNull(schema, "schema");
            
            if (!this._resolver.LoadedSchemas.Contains(schema))
            {
                this._resolver.LoadedSchemas.Add(schema);
            }
            
            this._writer.WriteStartObject();
            this.WritePropertyIfNotNull(_writer, JsonSchemaConstants.IdPropertyName, schema.Id);
            this.WritePropertyIfNotNull(_writer, JsonSchemaConstants.TitlePropertyName, schema.Title);
            this.WritePropertyIfNotNull(_writer, JsonSchemaConstants.DescriptionPropertyName, schema.Description);
            this.WritePropertyIfNotNull(_writer, JsonSchemaConstants.RequiredPropertyName, schema.Required);
            this.WritePropertyIfNotNull(_writer, JsonSchemaConstants.ReadOnlyPropertyName, schema.ReadOnly);
            this.WritePropertyIfNotNull(_writer, JsonSchemaConstants.HiddenPropertyName, schema.Hidden);
            this.WritePropertyIfNotNull(_writer, JsonSchemaConstants.TransientPropertyName, schema.Transient);
            if (schema.Type != null)
            {
                this.WriteType(JsonSchemaConstants.TypePropertyName, _writer, schema.Type.Value);
            }
            if (!schema.AllowAdditionalProperties)
            {
                this._writer.WritePropertyName(JsonSchemaConstants.AdditionalPropertiesPropertyName);
                this._writer.WriteValue(schema.AllowAdditionalProperties);
            }
            else
            {
                if (schema.AdditionalProperties != null)
                {
                    this._writer.WritePropertyName(JsonSchemaConstants.AdditionalPropertiesPropertyName);
                    this.ReferenceOrWriteSchema(schema.AdditionalProperties);
                }
            }
            this.WriteSchemaDictionaryIfNotNull(_writer, JsonSchemaConstants.PropertiesPropertyName, schema.Properties);
            this.WriteSchemaDictionaryIfNotNull(_writer, JsonSchemaConstants.PatternPropertiesPropertyName, schema.PatternProperties);
            this.WriteItems(schema);
            this.WritePropertyIfNotNull(_writer, JsonSchemaConstants.MinimumPropertyName, schema.Minimum);
            this.WritePropertyIfNotNull(_writer, JsonSchemaConstants.MaximumPropertyName, schema.Maximum);
            this.WritePropertyIfNotNull(_writer, JsonSchemaConstants.ExclusiveMinimumPropertyName, schema.ExclusiveMinimum);
            this.WritePropertyIfNotNull(_writer, JsonSchemaConstants.ExclusiveMaximumPropertyName, schema.ExclusiveMaximum);
            this.WritePropertyIfNotNull(_writer, JsonSchemaConstants.MinimumLengthPropertyName, schema.MinimumLength);
            this.WritePropertyIfNotNull(_writer, JsonSchemaConstants.MaximumLengthPropertyName, schema.MaximumLength);
            this.WritePropertyIfNotNull(_writer, JsonSchemaConstants.MinimumItemsPropertyName, schema.MinimumItems);
            this.WritePropertyIfNotNull(_writer, JsonSchemaConstants.MaximumItemsPropertyName, schema.MaximumItems);
            this.WritePropertyIfNotNull(_writer, JsonSchemaConstants.DivisibleByPropertyName, schema.DivisibleBy);
            this.WritePropertyIfNotNull(_writer, JsonSchemaConstants.FormatPropertyName, schema.Format);
            this.WritePropertyIfNotNull(_writer, JsonSchemaConstants.PatternPropertyName, schema.Pattern);
            if (schema.Enum != null)
            {
                this._writer.WritePropertyName(JsonSchemaConstants.EnumPropertyName);
                this._writer.WriteStartArray();
                foreach (JToken token in schema.Enum)
                {
                    token.WriteTo(this._writer);
                }
                this._writer.WriteEndArray();
            }
            if (schema.Default != null)
            {
                this._writer.WritePropertyName(JsonSchemaConstants.DefaultPropertyName);
                schema.Default.WriteTo(this._writer);
            }
            if (schema.Options != null)
            {
                this._writer.WritePropertyName(JsonSchemaConstants.OptionsPropertyName);
                this._writer.WriteStartArray();
                foreach (KeyValuePair<JToken, string> option in schema.Options)
                {
                    this._writer.WriteStartObject();
                    this._writer.WritePropertyName(JsonSchemaConstants.OptionValuePropertyName);
                    option.Key.WriteTo(this._writer);
                    if (option.Value != null)
                    {
                        this._writer.WritePropertyName(JsonSchemaConstants.OptionLabelPropertyName);
                        this._writer.WriteValue(option.Value);
                    }
                    this._writer.WriteEndObject();
                }
                this._writer.WriteEndArray();
            }
            if (schema.Disallow != null)
            {
                this.WriteType(JsonSchemaConstants.DisallowPropertyName, _writer, schema.Disallow.Value);
            }
            if (schema.Extends != null)
            {
                this._writer.WritePropertyName(JsonSchemaConstants.ExtendsPropertyName);
                this.ReferenceOrWriteSchema(schema.Extends);
            }
            this._writer.WriteEndObject();
        }
        
        private void WriteSchemaDictionaryIfNotNull(JsonWriter writer, string propertyName, IDictionary<string, JsonSchema> properties)
        {
            if (properties != null)
            {
                writer.WritePropertyName(propertyName);
                writer.WriteStartObject();
                foreach (KeyValuePair<string, JsonSchema> property in properties)
                {
                    writer.WritePropertyName(property.Key);
                    this.ReferenceOrWriteSchema(property.Value);
                }
                writer.WriteEndObject();
            }
        }
        
        private void WriteItems(JsonSchema schema)
        {
            if (CollectionUtils.IsNullOrEmpty(schema.Items))
            {
                return;
            }
            
            this._writer.WritePropertyName(JsonSchemaConstants.ItemsPropertyName);
            
            if (schema.Items.Count == 1)
            {
                this.ReferenceOrWriteSchema(schema.Items[0]);
                return;
            }
            
            this._writer.WriteStartArray();
            foreach (JsonSchema itemSchema in schema.Items)
            {
                this.ReferenceOrWriteSchema(itemSchema);
            }
            this._writer.WriteEndArray();
        }
        
        private void WriteType(string propertyName, JsonWriter writer, JsonSchemaType type)
        {
            IList<JsonSchemaType> types;
            if (System.Enum.IsDefined(typeof(JsonSchemaType), type))
            {
                types = new List<JsonSchemaType> { type };
            }
            else
            {
                types = EnumUtils.GetFlagsValues(type).Where(v => v != JsonSchemaType.None).ToList();
            }
            
            if (types.Count == 0)
            {
                return;
            }
            
            writer.WritePropertyName(propertyName);
            
            if (types.Count == 1)
            {
                writer.WriteValue(JsonSchemaBuilder.MapType(types[0]));
                return;
            }
            
            writer.WriteStartArray();
            foreach (JsonSchemaType jsonSchemaType in types)
            {
                writer.WriteValue(JsonSchemaBuilder.MapType(jsonSchemaType));
            }
            writer.WriteEndArray();
        }
        
        private void WritePropertyIfNotNull(JsonWriter writer, string propertyName, object value)
        {
            if (value != null)
            {
                writer.WritePropertyName(propertyName);
                writer.WriteValue(value);
            }
        }
    }
}