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

namespace Lib.JSON
{
    /// <summary>
    /// Represents a writer that provides a fast, non-cached, forward-only way of generating Json data.
    /// </summary>
    public abstract class JsonWriter : IDisposable
    {
        internal enum State
        {
            Start,
            Property,
            ObjectStart,
            Object,
            ArrayStart,
            Array,
            ConstructorStart,
            Constructor,
            Bytes,
            Closed,
            Error
        }

        // array that gives a new state based on the current state an the token being written
        private static readonly State[][] StateArray;
        
        internal static readonly State[][] StateArrayTempate = new[]
        {
            //                                      Start                   PropertyName            ObjectStart         Object            ArrayStart              Array                   ConstructorStart        Constructor             Closed          Error
            //                        
            /* None                        */new[] { State.Error, State.Error, State.Error, State.Error, State.Error, State.Error, State.Error, State.Error, State.Error, State.Error },
            /* StartObject                 */new[] { State.ObjectStart, State.ObjectStart, State.Error, State.Error, State.ObjectStart, State.ObjectStart, State.ObjectStart, State.ObjectStart, State.Error, State.Error },
            /* StartArray                  */new[] { State.ArrayStart, State.ArrayStart, State.Error, State.Error, State.ArrayStart, State.ArrayStart, State.ArrayStart, State.ArrayStart, State.Error, State.Error },
            /* StartConstructor            */new[] { State.ConstructorStart, State.ConstructorStart, State.Error, State.Error, State.ConstructorStart, State.ConstructorStart, State.ConstructorStart, State.ConstructorStart, State.Error, State.Error },
            /* StartProperty               */new[] { State.Property, State.Error, State.Property, State.Property, State.Error, State.Error, State.Error, State.Error, State.Error, State.Error },
            /* Comment                     */new[] { State.Start, State.Property, State.ObjectStart, State.Object, State.ArrayStart, State.Array, State.Constructor, State.Constructor, State.Error, State.Error },
            /* Raw                         */new[] { State.Start, State.Property, State.ObjectStart, State.Object, State.ArrayStart, State.Array, State.Constructor, State.Constructor, State.Error, State.Error },
            /* Value (this will be copied) */new[] { State.Start, State.Object, State.Error, State.Error, State.Array, State.Array, State.Constructor, State.Constructor, State.Error, State.Error }
        };
        
        internal static State[][] BuildStateArray()
        {
            var allStates = StateArrayTempate.ToList();
            var errorStates = StateArrayTempate[0];
            var valueStates = StateArrayTempate[7];
            
            foreach (JsonToken valueToken in EnumUtils.GetValues(typeof(JsonToken)))
            {
                if (allStates.Count <= (int)valueToken)
                {
                    switch (valueToken)
                    {
                        case JsonToken.Integer:
                        case JsonToken.Float:
                        case JsonToken.String:
                        case JsonToken.Boolean:
                        case JsonToken.Null:
                        case JsonToken.Undefined:
                        case JsonToken.Date:
                        case JsonToken.Bytes:
                            allStates.Add(valueStates);
                            break;
                        default:
                            allStates.Add(errorStates);
                            break;
                    }
                }
            }

            return allStates.ToArray();
        }
        
        static JsonWriter()
        {
            StateArray = BuildStateArray();
        }

        private readonly List<JTokenType> _stack;
        private State _currentState;
        
        /// <summary>
        /// Gets or sets a value indicating whether the underlying stream or
        /// <see cref="TextReader"/> should be closed when the writer is closed.
        /// </summary>
        /// <value>
        /// true to close the underlying stream or <see cref="TextReader"/> when
        /// the writer is closed; otherwise false. The default is true.
        /// </value>
        public bool CloseOutput { get; set; }
        
        /// <summary>
        /// Gets the top.
        /// </summary>
        /// <value>The top.</value>
        protected internal int Top { get; private set; }
        
        /// <summary>
        /// Gets the state of the writer.
        /// </summary>
        public WriteState WriteState
        {
            get
            {
                switch (this._currentState)
                {
                    case State.Error:
                        return WriteState.Error;
                    case State.Closed:
                        return WriteState.Closed;
                    case State.Object:
                    case State.ObjectStart:
                        return WriteState.Object;
                    case State.Array:
                    case State.ArrayStart:
                        return WriteState.Array;
                    case State.Constructor:
                    case State.ConstructorStart:
                        return WriteState.Constructor;
                    case State.Property:
                        return WriteState.Property;
                    case State.Start:
                        return WriteState.Start;
                    default:
                        throw new JsonWriterException(string.Format("Invalid state: {0}", this._currentState));
                }
            }
        }
        
        /// <summary>
        /// Indicates how the output is formatted.
        /// </summary>
        public Formatting Formatting { get; set; }
        
        /// <summary>
        /// Creates an instance of the <c>JsonWriter</c> class. 
        /// </summary>
        protected JsonWriter()
        {
            this._stack = new List<JTokenType>(8);
            this._stack.Add(JTokenType.None);
            this._currentState = State.Start;
            this.Formatting = Formatting.None;

            this.CloseOutput = true;
        }
        
        private void Push(JTokenType value)
        {
            this.Top++;
            if (this._stack.Count <= this.Top)
            {
                this._stack.Add(value);
            }
            else
            {
                this._stack[this.Top] = value;
            }
        }
        
        private JTokenType Pop()
        {
            JTokenType value = this.Peek();
            this.Top--;

            return value;
        }
        
        private JTokenType Peek()
        {
            return this._stack[this.Top];
        }
        
        /// <summary>
        /// Flushes whatever is in the buffer to the underlying streams and also flushes the underlying stream.
        /// </summary>
        public abstract void Flush();
        
        /// <summary>
        /// Closes this stream and the underlying stream.
        /// </summary>
        public virtual void Close()
        {
            this.AutoCompleteAll();
        }
        
        /// <summary>
        /// Writes the beginning of a Json object.
        /// </summary>
        public virtual void WriteStartObject()
        {
            this.AutoComplete(JsonToken.StartObject);
            this.Push(JTokenType.Object);
        }
        
        /// <summary>
        /// Writes the end of a Json object.
        /// </summary>
        public virtual void WriteEndObject()
        {
            this.AutoCompleteClose(JsonToken.EndObject);
        }
        
        /// <summary>
        /// Writes the beginning of a Json array.
        /// </summary>
        public virtual void WriteStartArray()
        {
            this.AutoComplete(JsonToken.StartArray);
            this.Push(JTokenType.Array);
        }
        
        /// <summary>
        /// Writes the end of an array.
        /// </summary>
        public virtual void WriteEndArray()
        {
            this.AutoCompleteClose(JsonToken.EndArray);
        }
        
        /// <summary>
        /// Writes the start of a constructor with the given name.
        /// </summary>
        /// <param name="name">The name of the constructor.</param>
        public virtual void WriteStartConstructor(string name)
        {
            this.AutoComplete(JsonToken.StartConstructor);
            this.Push(JTokenType.Constructor);
        }
        
        /// <summary>
        /// Writes the end constructor.
        /// </summary>
        public virtual void WriteEndConstructor()
        {
            this.AutoCompleteClose(JsonToken.EndConstructor);
        }
        
        /// <summary>
        /// Writes the property name of a name/value pair on a Json object.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        public virtual void WritePropertyName(string name)
        {
            this.AutoComplete(JsonToken.PropertyName);
        }
        
        /// <summary>
        /// Writes the end of the current Json object or array.
        /// </summary>
        public virtual void WriteEnd()
        {
            this.WriteEnd(this.Peek());
        }
        
        /// <summary>
        /// Writes the current <see cref="JsonReader"/> token.
        /// </summary>
        /// <param name="reader">The <see cref="JsonReader"/> to read the token from.</param>
        public void WriteToken(JsonReader reader)
        {
            ValidationUtils.ArgumentNotNull(reader, "reader");
            
            int initialDepth;
            
            if (reader.TokenType == JsonToken.None)
            {
                initialDepth = -1;
            }
            else if (!this.IsStartToken(reader.TokenType))
            {
                initialDepth = reader.Depth + 1;
            }
            else
            {
                initialDepth = reader.Depth;
            }

            this.WriteToken(reader, initialDepth);
        }
        
        internal void WriteToken(JsonReader reader, int initialDepth)
        {
            do
            {
                switch (reader.TokenType)
                {
                    case JsonToken.None:
                        // read to next
                        break;
                    case JsonToken.StartObject:
                        this.WriteStartObject();
                        break;
                    case JsonToken.StartArray:
                        this.WriteStartArray();
                        break;
                    case JsonToken.StartConstructor:
                        string constructorName = reader.Value.ToString();
                        // write a JValue date when the constructor is for a date
                        if (string.Equals(constructorName, "Date", StringComparison.Ordinal))
                        {
                            this.WriteConstructorDate(reader);
                        }
                        else
                        {
                            this.WriteStartConstructor(reader.Value.ToString());
                        }
                        break;
                    case JsonToken.PropertyName:
                        this.WritePropertyName(reader.Value.ToString());
                        break;
                    case JsonToken.Comment:
                        this.WriteComment(reader.Value.ToString());
                        break;
                    case JsonToken.Integer:
                        this.WriteValue(Convert.ToInt64(reader.Value, CultureInfo.InvariantCulture));
                        break;
                    case JsonToken.Float:
                        this.WriteValue(Convert.ToDouble(reader.Value, CultureInfo.InvariantCulture));
                        break;
                    case JsonToken.String:
                        this.WriteValue(reader.Value.ToString());
                        break;
                    case JsonToken.Boolean:
                        this.WriteValue(Convert.ToBoolean(reader.Value, CultureInfo.InvariantCulture));
                        break;
                    case JsonToken.Null:
                        this.WriteNull();
                        break;
                    case JsonToken.Undefined:
                        this.WriteUndefined();
                        break;
                    case JsonToken.EndObject:
                        this.WriteEndObject();
                        break;
                    case JsonToken.EndArray:
                        this.WriteEndArray();
                        break;
                    case JsonToken.EndConstructor:
                        this.WriteEndConstructor();
                        break;
                    case JsonToken.Date:
                        this.WriteValue((DateTime)reader.Value);
                        break;
                    case JsonToken.Raw:
                        this.WriteRawValue((string)reader.Value);
                        break;
                    case JsonToken.Bytes:
                        this.WriteValue((byte[])reader.Value);
                        break;
                    default:
                        throw MiscellaneousUtils.CreateArgumentOutOfRangeException("TokenType", reader.TokenType, "Unexpected token type.");
                }
            }
            while (
            // stop if we have reached the end of the token being read
                   initialDepth - 1 < reader.Depth - (this.IsEndToken(reader.TokenType) ? 1 : 0) &&
                   reader.Read());
        }
        
        private void WriteConstructorDate(JsonReader reader)
        {
            if (!reader.Read())
            {
                throw new Exception("Unexpected end when reading date constructor.");
            }
            if (reader.TokenType != JsonToken.Integer)
            {
                throw new Exception(string.Format("Unexpected token when reading date constructor. Expected Integer, got {0}", reader.TokenType));
            }

            long ticks = (long)reader.Value;
            DateTime date = JsonConvert.ConvertJavaScriptTicksToDateTime(ticks);
            
            if (!reader.Read())
            {
                throw new Exception("Unexpected end when reading date constructor.");
            }
            if (reader.TokenType != JsonToken.EndConstructor)
            {
                throw new Exception(string.Format("Unexpected token when reading date constructor. Expected EndConstructor, got {0}", reader.TokenType));
            }

            this.WriteValue(date);
        }
        
        private bool IsEndToken(JsonToken token)
        {
            switch (token)
            {
                case JsonToken.EndObject:
                case JsonToken.EndArray:
                case JsonToken.EndConstructor:
                    return true;
                default:
                    return false;
            }
        }
        
        private bool IsStartToken(JsonToken token)
        {
            switch (token)
            {
                case JsonToken.StartObject:
                case JsonToken.StartArray:
                case JsonToken.StartConstructor:
                    return true;
                default:
                    return false;
            }
        }
        
        private void WriteEnd(JTokenType type)
        {
            switch (type)
            {
                case JTokenType.Object:
                    this.WriteEndObject();
                    break;
                case JTokenType.Array:
                    this.WriteEndArray();
                    break;
                case JTokenType.Constructor:
                    this.WriteEndConstructor();
                    break;
                default:
                    throw new JsonWriterException(string.Format("Unexpected type when writing end: {0}", type));
            }
        }
        
        private void AutoCompleteAll()
        {
            while (this.Top > 0)
            {
                this.WriteEnd();
            }
        }
        
        private JTokenType GetTypeForCloseToken(JsonToken token)
        {
            switch (token)
            {
                case JsonToken.EndObject:
                    return JTokenType.Object;
                case JsonToken.EndArray:
                    return JTokenType.Array;
                case JsonToken.EndConstructor:
                    return JTokenType.Constructor;
                default:
                    throw new JsonWriterException(string.Format("No type for token: {0}", token));
            }
        }
        
        private JsonToken GetCloseTokenForType(JTokenType type)
        {
            switch (type)
            {
                case JTokenType.Object:
                    return JsonToken.EndObject;
                case JTokenType.Array:
                    return JsonToken.EndArray;
                case JTokenType.Constructor:
                    return JsonToken.EndConstructor;
                default:
                    throw new JsonWriterException(string.Format("No close token for type: {0}", type));
            }
        }
        
        private void AutoCompleteClose(JsonToken tokenBeingClosed)
        {
            // write closing symbol and calculate new state
            int levelsToComplete = 0;
            
            for (int i = 0; i < this.Top; i++)
            {
                int currentLevel = this.Top - i;
                
                if (this._stack[currentLevel] == this.GetTypeForCloseToken(tokenBeingClosed))
                {
                    levelsToComplete = i + 1;
                    break;
                }
            }
            
            if (levelsToComplete == 0)
            {
                throw new JsonWriterException("No token to close.");
            }
            
            for (int i = 0; i < levelsToComplete; i++)
            {
                JsonToken token = this.GetCloseTokenForType(this.Pop());
                
                if (this.Formatting == Formatting.Indented)
                {
                    if (this._currentState != State.ObjectStart && this._currentState != State.ArrayStart)
                    {
                        this.WriteIndent();
                    }
                }

                this.WriteEnd(token);
            }
            
            JTokenType currentLevelType = this.Peek();
            
            switch (currentLevelType)
            {
                case JTokenType.Object:
                    this._currentState = State.Object;
                    break;
                case JTokenType.Array:
                    this._currentState = State.Array;
                    break;
                case JTokenType.Constructor:
                    this._currentState = State.Array;
                    break;
                case JTokenType.None:
                    this._currentState = State.Start;
                    break;
                default:
                    throw new JsonWriterException(string.Format("Unknown JsonType: {0}", currentLevelType));
            }
        }
        
        /// <summary>
        /// Writes the specified end token.
        /// </summary>
        /// <param name="token">The end token to write.</param>
        protected virtual void WriteEnd(JsonToken token)
        {
        }
        
        /// <summary>
        /// Writes indent characters.
        /// </summary>
        protected virtual void WriteIndent()
        {
        }
        
        /// <summary>
        /// Writes the JSON value delimiter.
        /// </summary>
        protected virtual void WriteValueDelimiter()
        {
        }
        
        /// <summary>
        /// Writes an indent space.
        /// </summary>
        protected virtual void WriteIndentSpace()
        {
        }
        
        internal void AutoComplete(JsonToken tokenBeingWritten)
        {
            // gets new state based on the current state and what is being written
            State newState = StateArray[(int)tokenBeingWritten][(int)this._currentState];
            
            if (newState == State.Error)
            {
                throw new JsonWriterException("Token {0} in state {1} would result in an invalid JSON object.".FormatWith(CultureInfo.InvariantCulture, tokenBeingWritten.ToString(), this._currentState.ToString()));
            }
            
            if ((this._currentState == State.Object || this._currentState == State.Array || this._currentState == State.Constructor) && tokenBeingWritten != JsonToken.Comment)
            {
                this.WriteValueDelimiter();
            }
            else if (this._currentState == State.Property)
            {
                if (this.Formatting == Formatting.Indented)
                {
                    this.WriteIndentSpace();
                }
            }
            
            if (this.Formatting == Formatting.Indented)
            {
                WriteState writeState = this.WriteState;
                
                // don't indent a property when it is the first token to be written (i.e. at the start)
                if ((tokenBeingWritten == JsonToken.PropertyName && writeState != WriteState.Start) ||
                    writeState == WriteState.Array || writeState == WriteState.Constructor)
                {
                    this.WriteIndent();
                }
            }

            this._currentState = newState;
        }
        
        #region WriteValue methods
        
        /// <summary>
        /// Writes a null value.
        /// </summary>
        public virtual void WriteNull()
        {
            this.AutoComplete(JsonToken.Null);
        }
        
        /// <summary>
        /// Writes an undefined value.
        /// </summary>
        public virtual void WriteUndefined()
        {
            this.AutoComplete(JsonToken.Undefined);
        }
        
        /// <summary>
        /// Writes raw JSON without changing the writer's state.
        /// </summary>
        /// <param name="json">The raw JSON to write.</param>
        public virtual void WriteRaw(string json)
        {
        }
        
        /// <summary>
        /// Writes raw JSON where a value is expected and updates the writer's state.
        /// </summary>
        /// <param name="json">The raw JSON to write.</param>
        public virtual void WriteRawValue(string json)
        {
            // hack. want writer to change state as if a value had been written
            this.AutoComplete(JsonToken.Undefined);
            this.WriteRaw(json);
        }
        
        /// <summary>
        /// Writes a <see cref="String"/> value.
        /// </summary>
        /// <param name="value">The <see cref="String"/> value to write.</param>
        public virtual void WriteValue(string value)
        {
            this.AutoComplete(JsonToken.String);
        }
        
        /// <summary>
        /// Writes a <see cref="Int32"/> value.
        /// </summary>
        /// <param name="value">The <see cref="Int32"/> value to write.</param>
        public virtual void WriteValue(int value)
        {
            this.AutoComplete(JsonToken.Integer);
        }
        
        /// <summary>
        /// Writes a <see cref="UInt32"/> value.
        /// </summary>
        /// <param name="value">The <see cref="UInt32"/> value to write.</param>
        public virtual void WriteValue(uint value)
        {
            this.AutoComplete(JsonToken.Integer);
        }
        
        /// <summary>
        /// Writes a <see cref="Int64"/> value.
        /// </summary>
        /// <param name="value">The <see cref="Int64"/> value to write.</param>
        public virtual void WriteValue(long value)
        {
            this.AutoComplete(JsonToken.Integer);
        }
        
        /// <summary>
        /// Writes a <see cref="UInt64"/> value.
        /// </summary>
        /// <param name="value">The <see cref="UInt64"/> value to write.</param>
        public virtual void WriteValue(ulong value)
        {
            this.AutoComplete(JsonToken.Integer);
        }
        
        /// <summary>
        /// Writes a <see cref="Single"/> value.
        /// </summary>
        /// <param name="value">The <see cref="Single"/> value to write.</param>
        public virtual void WriteValue(float value)
        {
            this.AutoComplete(JsonToken.Float);
        }
        
        /// <summary>
        /// Writes a <see cref="Double"/> value.
        /// </summary>
        /// <param name="value">The <see cref="Double"/> value to write.</param>
        public virtual void WriteValue(double value)
        {
            this.AutoComplete(JsonToken.Float);
        }
        
        /// <summary>
        /// Writes a <see cref="Boolean"/> value.
        /// </summary>
        /// <param name="value">The <see cref="Boolean"/> value to write.</param>
        public virtual void WriteValue(bool value)
        {
            this.AutoComplete(JsonToken.Boolean);
        }
        
        /// <summary>
        /// Writes a <see cref="Int16"/> value.
        /// </summary>
        /// <param name="value">The <see cref="Int16"/> value to write.</param>
        public virtual void WriteValue(short value)
        {
            this.AutoComplete(JsonToken.Integer);
        }
        
        /// <summary>
        /// Writes a <see cref="UInt16"/> value.
        /// </summary>
        /// <param name="value">The <see cref="UInt16"/> value to write.</param>
        public virtual void WriteValue(ushort value)
        {
            this.AutoComplete(JsonToken.Integer);
        }
        
        /// <summary>
        /// Writes a <see cref="Char"/> value.
        /// </summary>
        /// <param name="value">The <see cref="Char"/> value to write.</param>
        public virtual void WriteValue(char value)
        {
            this.AutoComplete(JsonToken.String);
        }
        
        /// <summary>
        /// Writes a <see cref="Byte"/> value.
        /// </summary>
        /// <param name="value">The <see cref="Byte"/> value to write.</param>
        public virtual void WriteValue(byte value)
        {
            this.AutoComplete(JsonToken.Integer);
        }
        
        /// <summary>
        /// Writes a <see cref="SByte"/> value.
        /// </summary>
        /// <param name="value">The <see cref="SByte"/> value to write.</param>
        public virtual void WriteValue(sbyte value)
        {
            this.AutoComplete(JsonToken.Integer);
        }
        
        /// <summary>
        /// Writes a <see cref="Decimal"/> value.
        /// </summary>
        /// <param name="value">The <see cref="Decimal"/> value to write.</param>
        public virtual void WriteValue(decimal value)
        {
            this.AutoComplete(JsonToken.Float);
        }
        
        /// <summary>
        /// Writes a <see cref="DateTime"/> value.
        /// </summary>
        /// <param name="value">The <see cref="DateTime"/> value to write.</param>
        public virtual void WriteValue(DateTime value)
        {
            this.AutoComplete(JsonToken.Date);
        }
        
        #if !PocketPC && !NET20
        /// <summary>
        /// Writes a <see cref="DateTimeOffset"/> value.
        /// </summary>
        /// <param name="value">The <see cref="DateTimeOffset"/> value to write.</param>
        public virtual void WriteValue(DateTimeOffset value)
        {
            this.AutoComplete(JsonToken.Date);
        }
        
        #endif
        
        /// <summary>
        /// Writes a <see cref="Guid"/> value.
        /// </summary>
        /// <param name="value">The <see cref="Guid"/> value to write.</param>
        public virtual void WriteValue(Guid value)
        {
            this.AutoComplete(JsonToken.String);
        }
        
        /// <summary>
        /// Writes a <see cref="TimeSpan"/> value.
        /// </summary>
        /// <param name="value">The <see cref="TimeSpan"/> value to write.</param>
        public virtual void WriteValue(TimeSpan value)
        {
            this.AutoComplete(JsonToken.String);
        }
        
        /// <summary>
        /// Writes a <see cref="Nullable{Int32}"/> value.
        /// </summary>
        /// <param name="value">The <see cref="Nullable{Int32}"/> value to write.</param>
        public virtual void WriteValue(int? value)
        {
            if (value == null)
            {
                this.WriteNull();
            }
            else
            {
                this.WriteValue(value.Value);
            }
        }
        
        /// <summary>
        /// Writes a <see cref="Nullable{UInt32}"/> value.
        /// </summary>
        /// <param name="value">The <see cref="Nullable{UInt32}"/> value to write.</param>
        public virtual void WriteValue(uint? value)
        {
            if (value == null)
            {
                this.WriteNull();
            }
            else
            {
                this.WriteValue(value.Value);
            }
        }
        
        /// <summary>
        /// Writes a <see cref="Nullable{Int64}"/> value.
        /// </summary>
        /// <param name="value">The <see cref="Nullable{Int64}"/> value to write.</param>
        public virtual void WriteValue(long? value)
        {
            if (value == null)
            {
                this.WriteNull();
            }
            else
            {
                this.WriteValue(value.Value);
            }
        }
        
        /// <summary>
        /// Writes a <see cref="Nullable{UInt64}"/> value.
        /// </summary>
        /// <param name="value">The <see cref="Nullable{UInt64}"/> value to write.</param>
        public virtual void WriteValue(ulong? value)
        {
            if (value == null)
            {
                this.WriteNull();
            }
            else
            {
                this.WriteValue(value.Value);
            }
        }
        
        /// <summary>
        /// Writes a <see cref="Nullable{Single}"/> value.
        /// </summary>
        /// <param name="value">The <see cref="Nullable{Single}"/> value to write.</param>
        public virtual void WriteValue(float? value)
        {
            if (value == null)
            {
                this.WriteNull();
            }
            else
            {
                this.WriteValue(value.Value);
            }
        }
        
        /// <summary>
        /// Writes a <see cref="Nullable{Double}"/> value.
        /// </summary>
        /// <param name="value">The <see cref="Nullable{Double}"/> value to write.</param>
        public virtual void WriteValue(double? value)
        {
            if (value == null)
            {
                this.WriteNull();
            }
            else
            {
                this.WriteValue(value.Value);
            }
        }
        
        /// <summary>
        /// Writes a <see cref="Nullable{Boolean}"/> value.
        /// </summary>
        /// <param name="value">The <see cref="Nullable{Boolean}"/> value to write.</param>
        public virtual void WriteValue(bool? value)
        {
            if (value == null)
            {
                this.WriteNull();
            }
            else
            {
                this.WriteValue(value.Value);
            }
        }
        
        /// <summary>
        /// Writes a <see cref="Nullable{Int16}"/> value.
        /// </summary>
        /// <param name="value">The <see cref="Nullable{Int16}"/> value to write.</param>
        public virtual void WriteValue(short? value)
        {
            if (value == null)
            {
                this.WriteNull();
            }
            else
            {
                this.WriteValue(value.Value);
            }
        }
        
        /// <summary>
        /// Writes a <see cref="Nullable{UInt16}"/> value.
        /// </summary>
        /// <param name="value">The <see cref="Nullable{UInt16}"/> value to write.</param>
        public virtual void WriteValue(ushort? value)
        {
            if (value == null)
            {
                this.WriteNull();
            }
            else
            {
                this.WriteValue(value.Value);
            }
        }
        
        /// <summary>
        /// Writes a <see cref="Nullable{Char}"/> value.
        /// </summary>
        /// <param name="value">The <see cref="Nullable{Char}"/> value to write.</param>
        public virtual void WriteValue(char? value)
        {
            if (value == null)
            {
                this.WriteNull();
            }
            else
            {
                this.WriteValue(value.Value);
            }
        }
        
        /// <summary>
        /// Writes a <see cref="Nullable{Byte}"/> value.
        /// </summary>
        /// <param name="value">The <see cref="Nullable{Byte}"/> value to write.</param>
        public virtual void WriteValue(byte? value)
        {
            if (value == null)
            {
                this.WriteNull();
            }
            else
            {
                this.WriteValue(value.Value);
            }
        }
        
        /// <summary>
        /// Writes a <see cref="Nullable{SByte}"/> value.
        /// </summary>
        /// <param name="value">The <see cref="Nullable{SByte}"/> value to write.</param>
        public virtual void WriteValue(sbyte? value)
        {
            if (value == null)
            {
                this.WriteNull();
            }
            else
            {
                this.WriteValue(value.Value);
            }
        }
        
        /// <summary>
        /// Writes a <see cref="Nullable{Decimal}"/> value.
        /// </summary>
        /// <param name="value">The <see cref="Nullable{Decimal}"/> value to write.</param>
        public virtual void WriteValue(decimal? value)
        {
            if (value == null)
            {
                this.WriteNull();
            }
            else
            {
                this.WriteValue(value.Value);
            }
        }
        
        /// <summary>
        /// Writes a <see cref="Nullable{DateTime}"/> value.
        /// </summary>
        /// <param name="value">The <see cref="Nullable{DateTime}"/> value to write.</param>
        public virtual void WriteValue(DateTime? value)
        {
            if (value == null)
            {
                this.WriteNull();
            }
            else
            {
                this.WriteValue(value.Value);
            }
        }
        
        #if !PocketPC && !NET20
        /// <summary>
        /// Writes a <see cref="Nullable{DateTimeOffset}"/> value.
        /// </summary>
        /// <param name="value">The <see cref="Nullable{DateTimeOffset}"/> value to write.</param>
        public virtual void WriteValue(DateTimeOffset? value)
        {
            if (value == null)
            {
                this.WriteNull();
            }
            else
            {
                this.WriteValue(value.Value);
            }
        }
        
        #endif
        
        /// <summary>
        /// Writes a <see cref="Nullable{Guid}"/> value.
        /// </summary>
        /// <param name="value">The <see cref="Nullable{Guid}"/> value to write.</param>
        public virtual void WriteValue(Guid? value)
        {
            if (value == null)
            {
                this.WriteNull();
            }
            else
            {
                this.WriteValue(value.Value);
            }
        }
        
        /// <summary>
        /// Writes a <see cref="Nullable{TimeSpan}"/> value.
        /// </summary>
        /// <param name="value">The <see cref="Nullable{TimeSpan}"/> value to write.</param>
        public virtual void WriteValue(TimeSpan? value)
        {
            if (value == null)
            {
                this.WriteNull();
            }
            else
            {
                this.WriteValue(value.Value);
            }
        }
        
        /// <summary>
        /// Writes a <see cref="T:Byte[]"/> value.
        /// </summary>
        /// <param name="value">The <see cref="T:Byte[]"/> value to write.</param>
        public virtual void WriteValue(byte[] value)
        {
            if (value == null)
            {
                this.WriteNull();
            }
            else
            {
                this.AutoComplete(JsonToken.Bytes);
            }
        }
        
        /// <summary>
        /// Writes a <see cref="Uri"/> value.
        /// </summary>
        /// <param name="value">The <see cref="Uri"/> value to write.</param>
        public virtual void WriteValue(Uri value)
        {
            if (value == null)
            {
                this.WriteNull();
            }
            else
            {
                this.AutoComplete(JsonToken.String);
            }
        }
        
        /// <summary>
        /// Writes a <see cref="Object"/> value.
        /// An error will raised if the value cannot be written as a single JSON token.
        /// </summary>
        /// <param name="value">The <see cref="Object"/> value to write.</param>
        public virtual void WriteValue(object value)
        {
            if (value == null)
            {
                this.WriteNull();
                return;
            }
            else if (value is IConvertible)
            {
                IConvertible convertible = value as IConvertible;
                
                switch (convertible.GetTypeCode())
                {
                    case TypeCode.String:
                        this.WriteValue(convertible.ToString(CultureInfo.InvariantCulture));
                        return;
                    case TypeCode.Char:
                        this.WriteValue(convertible.ToChar(CultureInfo.InvariantCulture));
                        return;
                    case TypeCode.Boolean:
                        this.WriteValue(convertible.ToBoolean(CultureInfo.InvariantCulture));
                        return;
                    case TypeCode.SByte:
                        this.WriteValue(convertible.ToSByte(CultureInfo.InvariantCulture));
                        return;
                    case TypeCode.Int16:
                        this.WriteValue(convertible.ToInt16(CultureInfo.InvariantCulture));
                        return;
                    case TypeCode.UInt16:
                        this.WriteValue(convertible.ToUInt16(CultureInfo.InvariantCulture));
                        return;
                    case TypeCode.Int32:
                        this.WriteValue(convertible.ToInt32(CultureInfo.InvariantCulture));
                        return;
                    case TypeCode.Byte:
                        this.WriteValue(convertible.ToByte(CultureInfo.InvariantCulture));
                        return;
                    case TypeCode.UInt32:
                        this.WriteValue(convertible.ToUInt32(CultureInfo.InvariantCulture));
                        return;
                    case TypeCode.Int64:
                        this.WriteValue(convertible.ToInt64(CultureInfo.InvariantCulture));
                        return;
                    case TypeCode.UInt64:
                        this.WriteValue(convertible.ToUInt64(CultureInfo.InvariantCulture));
                        return;
                    case TypeCode.Single:
                        this.WriteValue(convertible.ToSingle(CultureInfo.InvariantCulture));
                        return;
                    case TypeCode.Double:
                        this.WriteValue(convertible.ToDouble(CultureInfo.InvariantCulture));
                        return;
                    case TypeCode.DateTime:
                        this.WriteValue(convertible.ToDateTime(CultureInfo.InvariantCulture));
                        return;
                    case TypeCode.Decimal:
                        this.WriteValue(convertible.ToDecimal(CultureInfo.InvariantCulture));
                        return;
                    case TypeCode.DBNull:
                        this.WriteNull();
                        return;
                }
            }
            #if !PocketPC && !NET20
            else if (value is DateTimeOffset)
            {
                this.WriteValue((DateTimeOffset)value);
                return;
            }
            #endif
            else if (value is byte[])
            {
                this.WriteValue((byte[])value);
                return;
            }
            else if (value is Guid)
            {
                this.WriteValue((Guid)value);
                return;
            }
            else if (value is Uri)
            {
                this.WriteValue((Uri)value);
                return;
            }
            else if (value is TimeSpan)
            {
                this.WriteValue((TimeSpan)value);
                return;
            }
            
            throw new ArgumentException("Unsupported type: {0}. Use the JsonSerializer class to get the object's JSON representation.".FormatWith(CultureInfo.InvariantCulture, value.GetType()));
        }
        
        #endregion
        
        /// <summary>
        /// Writes out a comment <code>/*...*/</code> containing the specified text. 
        /// </summary>
        /// <param name="text">Text to place inside the comment.</param>
        public virtual void WriteComment(string text)
        {
            this.AutoComplete(JsonToken.Comment);
        }
        
        /// <summary>
        /// Writes out the given white space.
        /// </summary>
        /// <param name="ws">The string of white space characters.</param>
        public virtual void WriteWhitespace(string ws)
        {
            if (ws != null)
            {
                if (!StringUtils.IsWhiteSpace(ws))
                {
                    throw new JsonWriterException("Only white space characters should be used.");
                }
            }
        }
        
        void IDisposable.Dispose()
        {
            this.Dispose(true);
        }
        
        private void Dispose(bool disposing)
        {
            if (this._currentState != State.Closed)
            {
                this.Close();
            }
        }
    }
}