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
using System.Text;
using Lib.JSON.Utilities;

namespace Lib.JSON.Bson
{
    /// <summary>
    /// Represents a reader that provides fast, non-cached, forward-only access to serialized Json data.
    /// </summary>
    public class BsonReader : JsonReader
    {
        private const int MaxCharBytesSize = 128;
        private static readonly byte[] SeqRange1 = new byte[] { 0, 127 }; // range of 1-byte sequence
        private static readonly byte[] SeqRange2 = new byte[] { 194, 223 }; // range of 2-byte sequence
        private static readonly byte[] SeqRange3 = new byte[] { 224, 239 }; // range of 3-byte sequence
        private static readonly byte[] SeqRange4 = new byte[] { 240, 244 }; // range of 4-byte sequence

        private readonly BinaryReader _reader;
        private readonly List<ContainerContext> _stack;

        private byte[] _byteBuffer;
        private char[] _charBuffer;
        
        private BsonType _currentElementType;
        private BsonReaderState _bsonReaderState;
        private ContainerContext _currentContext;
        
        private enum BsonReaderState
        {
            Normal,
            ReferenceStart,
            ReferenceRef,
            ReferenceId,
            CodeWScopeStart,
            CodeWScopeCode,
            CodeWScopeScope,
            CodeWScopeScopeObject,
            CodeWScopeScopeEnd
        }
        
        private class ContainerContext
        {
            public readonly BsonType Type;
            public int Length;
            public int Position;
            
            public ContainerContext(BsonType type)
            {
                this.Type = type;
            }
        }
        
        /// <summary>
        /// Gets or sets a value indicating whether binary data reading should compatible with incorrect Json.NET 3.5 written binary.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if binary data reading will be compatible with incorrect Json.NET 3.5 written binary; otherwise, <c>false</c>.
        /// </value>
        public bool JsonNet35BinaryCompatibility { get; set; }
        
        /// <summary>
        /// Gets or sets a value indicating whether the root object will be read as a JSON array.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if the root object will be read as a JSON array; otherwise, <c>false</c>.
        /// </value>
        public bool ReadRootValueAsArray { get; set; }
        
        /// <summary>
        /// Gets or sets the <see cref="DateTimeKind" /> used when reading <see cref="DateTime"/> values from BSON.
        /// </summary>
        /// <value>The <see cref="DateTimeKind" /> used when reading <see cref="DateTime"/> values from BSON.</value>
        public DateTimeKind DateTimeKindHandling { get; set; }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="BsonReader"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public BsonReader(Stream stream) : this(stream, false, DateTimeKind.Local)
        {
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="BsonReader"/> class.
        /// </summary>
        /// <param name="reader">The reader.</param>
        public BsonReader(BinaryReader reader) : this(reader, false, DateTimeKind.Local)
        {
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="BsonReader"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="readRootValueAsArray">if set to <c>true</c> the root object will be read as a JSON array.</param>
        /// <param name="dateTimeKindHandling">The <see cref="DateTimeKind" /> used when reading <see cref="DateTime"/> values from BSON.</param>
        public BsonReader(Stream stream, bool readRootValueAsArray, DateTimeKind dateTimeKindHandling)
        {
            ValidationUtils.ArgumentNotNull(stream, "stream");
            this._reader = new BinaryReader(stream);
            this._stack = new List<ContainerContext>();
            this.ReadRootValueAsArray = readRootValueAsArray;
            this.DateTimeKindHandling = dateTimeKindHandling;
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="BsonReader"/> class.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="readRootValueAsArray">if set to <c>true</c> the root object will be read as a JSON array.</param>
        /// <param name="dateTimeKindHandling">The <see cref="DateTimeKind" /> used when reading <see cref="DateTime"/> values from BSON.</param>
        public BsonReader(BinaryReader reader, bool readRootValueAsArray, DateTimeKind dateTimeKindHandling)
        {
            ValidationUtils.ArgumentNotNull(reader, "reader");
            this._reader = reader;
            this._stack = new List<ContainerContext>();
            this.ReadRootValueAsArray = readRootValueAsArray;
            this.DateTimeKindHandling = dateTimeKindHandling;
        }
        
        private string ReadElement()
        {
            this._currentElementType = this.ReadType();
            string elementName = this.ReadString();
            return elementName;
        }
        
        /// <summary>
        /// Reads the next JSON token from the stream as a <see cref="T:Byte[]"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:Byte[]"/> or a null reference if the next JSON token is null. This method will return <c>null</c> at the end of an array.
        /// </returns>
        public override byte[] ReadAsBytes()
        {
            this.Read();
            
            if (this.IsWrappedInTypeObject())
            {
                byte[] data = this.ReadAsBytes();
                this.Read();
                this.SetToken(JsonToken.Bytes, data);
                return data;
            }
            
            if (this.TokenType == JsonToken.Null)
            {
                return null;
            }
            if (this.TokenType == JsonToken.Bytes)
            {
                return (byte[])this.Value;
            }
            
            if (this.TokenType == JsonToken.EndArray)
            {
                return null;
            }

            throw this.CreateReaderException(this, "Error reading bytes. Expected bytes but got {0}.".FormatWith(CultureInfo.InvariantCulture, TokenType));
        }
        
        private bool IsWrappedInTypeObject()
        {
            if (this.TokenType == JsonToken.StartObject)
            {
                this.Read();
                if (this.Value.ToString() == "$type")
                {
                    this.Read();
                    if (this.Value != null && this.Value.ToString().StartsWith("System.Byte[]"))
                    {
                        this.Read();
                        if (this.Value.ToString() == "$value")
                        {
                            return true;
                        }
                    }
                }

                throw this.CreateReaderException(this, "Unexpected token when reading bytes: {0}.".FormatWith(CultureInfo.InvariantCulture, JsonToken.StartObject));
            }

            return false;
        }
        
        /// <summary>
        /// Reads the next JSON token from the stream as a <see cref="Nullable{Decimal}"/>.
        /// </summary>
        /// <returns>A <see cref="Nullable{Decimal}"/>. This method will return <c>null</c> at the end of an array.</returns>
        public override decimal? ReadAsDecimal()
        {
            this.Read();
            
            if (this.TokenType == JsonToken.Integer || this.TokenType == JsonToken.Float)
            {
                this.SetToken(JsonToken.Float, Convert.ToDecimal(Value, CultureInfo.InvariantCulture));
                return (decimal)this.Value;
            }
            
            if (this.TokenType == JsonToken.Null)
            {
                return null;
            }
            
            decimal d;
            if (this.TokenType == JsonToken.String)
            {
                if (decimal.TryParse((string)this.Value, NumberStyles.Number, this.Culture, out d))
                {
                    this.SetToken(JsonToken.Float, d);
                    return d;
                }
                else
                {
                    throw this.CreateReaderException(this, "Could not convert string to decimal: {0}.".FormatWith(CultureInfo.InvariantCulture, Value));
                }
            }
            
            if (this.TokenType == JsonToken.EndArray)
            {
                return null;
            }

            throw this.CreateReaderException(this, "Error reading decimal. Expected a number but got {0}.".FormatWith(CultureInfo.InvariantCulture, TokenType));
        }
        
        /// <summary>
        /// Reads the next JSON token from the stream as a <see cref="Nullable{Int32}"/>.
        /// </summary>
        /// <returns>A <see cref="Nullable{Int32}"/>. This method will return <c>null</c> at the end of an array.</returns>
        public override int? ReadAsInt32()
        {
            this.Read();
            
            if (this.TokenType == JsonToken.Integer || this.TokenType == JsonToken.Float)
            {
                this.SetToken(JsonToken.Float, Convert.ToInt32(Value, CultureInfo.InvariantCulture));
                return (int)this.Value;
            }
            
            if (this.TokenType == JsonToken.Null)
            {
                return null;
            }
            
            int i;
            if (this.TokenType == JsonToken.String)
            {
                if (int.TryParse((string)this.Value, NumberStyles.Integer, this.Culture, out i))
                {
                    this.SetToken(JsonToken.Integer, i);
                    return i;
                }
                else
                {
                    throw this.CreateReaderException(this, "Could not convert string to integer: {0}.".FormatWith(CultureInfo.InvariantCulture, Value));
                }
            }
            
            if (this.TokenType == JsonToken.EndArray)
            {
                return null;
            }

            throw this.CreateReaderException(this, "Error reading integer. Expected a number but got {0}.".FormatWith(CultureInfo.InvariantCulture, TokenType));
        }
        
        #if !NET20
        /// <summary>
        /// Reads the next JSON token from the stream as a <see cref="Nullable{DateTimeOffset}"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="Nullable{DateTimeOffset}"/>. This method will return <c>null</c> at the end of an array.
        /// </returns>
        public override DateTimeOffset? ReadAsDateTimeOffset()
        {
            this.Read();
            
            if (this.TokenType == JsonToken.Date)
            {
                this.SetToken(JsonToken.Date, new DateTimeOffset((DateTime)Value));
                return (DateTimeOffset)this.Value;
            }
            
            if (this.TokenType == JsonToken.Null)
            {
                return null;
            }
            
            DateTimeOffset dt;
            if (this.TokenType == JsonToken.String)
            {
                if (DateTimeOffset.TryParse((string)this.Value, this.Culture, DateTimeStyles.None, out dt))
                {
                    this.SetToken(JsonToken.Date, dt);
                    return dt;
                }
                else
                {
                    throw this.CreateReaderException(this, "Could not convert string to DateTimeOffset: {0}.".FormatWith(CultureInfo.InvariantCulture, Value));
                }
            }
            
            if (this.TokenType == JsonToken.EndArray)
            {
                return null;
            }

            throw this.CreateReaderException(this, "Error reading date. Expected date but got {0}.".FormatWith(CultureInfo.InvariantCulture, TokenType));
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
            try
            {
                switch (this._bsonReaderState)
                {
                    case BsonReaderState.Normal:
                        return this.ReadNormal();
                    case BsonReaderState.ReferenceStart:
                    case BsonReaderState.ReferenceRef:
                    case BsonReaderState.ReferenceId:
                        return this.ReadReference();
                    case BsonReaderState.CodeWScopeStart:
                    case BsonReaderState.CodeWScopeCode:
                    case BsonReaderState.CodeWScopeScope:
                    case BsonReaderState.CodeWScopeScopeObject:
                    case BsonReaderState.CodeWScopeScopeEnd:
                        return this.ReadCodeWScope();
                    default:
                        throw this.CreateReaderException(this, "Unexpected state: {0}".FormatWith(CultureInfo.InvariantCulture, _bsonReaderState));
                }
            }
            catch (EndOfStreamException)
            {
                return false;
            }
        }
        
        /// <summary>
        /// Changes the <see cref="JsonReader.State"/> to Closed.
        /// </summary>
        public override void Close()
        {
            base.Close();
            
            if (this.CloseInput && this._reader != null)
            {
                this._reader.Close();
            }
        }
        
        private bool ReadCodeWScope()
        {
            switch (this._bsonReaderState)
            {
                case BsonReaderState.CodeWScopeStart:
                    this.SetToken(JsonToken.PropertyName, "$code");
                    this._bsonReaderState = BsonReaderState.CodeWScopeCode;
                    return true;
                case BsonReaderState.CodeWScopeCode:
                    // total CodeWScope size - not used
                    this.ReadInt32();
                    
                    this.SetToken(JsonToken.String, this.ReadLengthString());
                    this._bsonReaderState = BsonReaderState.CodeWScopeScope;
                    return true;
                case BsonReaderState.CodeWScopeScope:
                    if (this.CurrentState == State.PostValue)
                    {
                        this.SetToken(JsonToken.PropertyName, "$scope");
                        return true;
                    }
                    else
                    {
                        this.SetToken(JsonToken.StartObject);
                        this._bsonReaderState = BsonReaderState.CodeWScopeScopeObject;
                        
                        ContainerContext newContext = new ContainerContext(BsonType.Object);
                        this.PushContext(newContext);
                        newContext.Length = this.ReadInt32();
                        
                        return true;
                    }
                case BsonReaderState.CodeWScopeScopeObject:
                    bool result = this.ReadNormal();
                    if (result && this.TokenType == JsonToken.EndObject)
                    {
                        this._bsonReaderState = BsonReaderState.CodeWScopeScopeEnd;
                    }
                    
                    return result;
                case BsonReaderState.CodeWScopeScopeEnd:
                    this.SetToken(JsonToken.EndObject);
                    this._bsonReaderState = BsonReaderState.Normal;
                    return true;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private bool ReadReference()
        {
            switch (this.CurrentState)
            {
                case State.ObjectStart:
                    {
                        this.SetToken(JsonToken.PropertyName, "$ref");
                        this._bsonReaderState = BsonReaderState.ReferenceRef;
                        return true;
                    }
                case State.Property:
                    {
                        if (this._bsonReaderState == BsonReaderState.ReferenceRef)
                        {
                            this.SetToken(JsonToken.String, this.ReadLengthString());
                            return true;
                        }
                        else if (this._bsonReaderState == BsonReaderState.ReferenceId)
                        {
                            this.SetToken(JsonToken.Bytes, this.ReadBytes(12));
                            return true;
                        }
                        else
                        {
                            throw this.CreateReaderException(this, string.Format("Unexpected state when reading BSON reference: {0}", _bsonReaderState));
                        }
                    }
                case State.PostValue:
                    {
                        if (this._bsonReaderState == BsonReaderState.ReferenceRef)
                        {
                            this.SetToken(JsonToken.PropertyName, "$id");
                            this._bsonReaderState = BsonReaderState.ReferenceId;
                            return true;
                        }
                        else if (this._bsonReaderState == BsonReaderState.ReferenceId)
                        {
                            this.SetToken(JsonToken.EndObject);
                            this._bsonReaderState = BsonReaderState.Normal;
                            return true;
                        }
                        else
                        {
                            throw this.CreateReaderException(this, string.Format("Unexpected state when reading BSON reference: {0}", _bsonReaderState));
                        }
                    }
                default:
                    throw this.CreateReaderException(this, string.Format("Unexpected state when reading BSON reference: {0}", CurrentState));
            }
        }
        
        private bool ReadNormal()
        {
            switch (this.CurrentState)
            {
                case State.Start:
                    {
                        JsonToken token = (!this.ReadRootValueAsArray) ? JsonToken.StartObject : JsonToken.StartArray;
                        BsonType type = (!this.ReadRootValueAsArray) ? BsonType.Object : BsonType.Array;
                        
                        this.SetToken(token);
                        ContainerContext newContext = new ContainerContext(type);
                        this.PushContext(newContext);
                        newContext.Length = this.ReadInt32();
                        return true;
                    }
                case State.Complete:
                case State.Closed:
                    return false;
                case State.Property:
                    {
                        this.ReadType(_currentElementType);
                        return true;
                    }
                case State.ObjectStart:
                case State.ArrayStart:
                case State.PostValue:
                    ContainerContext context = this._currentContext;
                    if (context == null)
                    {
                        return false;
                    }
                    
                    int lengthMinusEnd = context.Length - 1;
                    
                    if (context.Position < lengthMinusEnd)
                    {
                        if (context.Type == BsonType.Array)
                        {
                            this.ReadElement();
                            this.ReadType(_currentElementType);
                            return true;
                        }
                        else
                        {
                            this.SetToken(JsonToken.PropertyName, this.ReadElement());
                            return true;
                        }
                    }
                    else if (context.Position == lengthMinusEnd)
                    {
                        if (this.ReadByte() != 0)
                        {
                            throw this.CreateReaderException(this, "Unexpected end of object byte value.");
                        }
                        
                        this.PopContext();
                        if (this._currentContext != null)
                        {
                            this.MovePosition(context.Length);
                        }
                        
                        JsonToken endToken = (context.Type == BsonType.Object) ? JsonToken.EndObject : JsonToken.EndArray;
                        this.SetToken(endToken);
                        return true;
                    }
                    else
                    {
                        throw this.CreateReaderException(this, "Read past end of current container context.");
                    }
                case State.ConstructorStart:
                    break;
                case State.Constructor:
                    break;
                case State.Error:
                    break;
                case State.Finished:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return false;
        }
        
        private void PopContext()
        {
            this._stack.RemoveAt(this._stack.Count - 1);
            if (this._stack.Count == 0)
            {
                this._currentContext = null;
            }
            else
            {
                this._currentContext = this._stack[this._stack.Count - 1];
            }
        }
        
        private void PushContext(ContainerContext newContext)
        {
            this._stack.Add(newContext);
            this._currentContext = newContext;
        }
        
        private byte ReadByte()
        {
            this.MovePosition(1);
            return this._reader.ReadByte();
        }
        
        private void ReadType(BsonType type)
        {
            switch (type)
            {
                case BsonType.Number:
                    this.SetToken(JsonToken.Float, this.ReadDouble());
                    break;
                case BsonType.String:
                case BsonType.Symbol:
                    this.SetToken(JsonToken.String, this.ReadLengthString());
                    break;
                case BsonType.Object:
                    {
                        this.SetToken(JsonToken.StartObject);
                        
                        ContainerContext newContext = new ContainerContext(BsonType.Object);
                        this.PushContext(newContext);
                        newContext.Length = this.ReadInt32();
                        break;
                    }
                case BsonType.Array:
                    {
                        this.SetToken(JsonToken.StartArray);
                        
                        ContainerContext newContext = new ContainerContext(BsonType.Array);
                        this.PushContext(newContext);
                        newContext.Length = this.ReadInt32();
                        break;
                    }
                case BsonType.Binary:
                    this.SetToken(JsonToken.Bytes, this.ReadBinary());
                    break;
                case BsonType.Undefined:
                    this.SetToken(JsonToken.Undefined);
                    break;
                case BsonType.Oid:
                    byte[] oid = this.ReadBytes(12);
                    this.SetToken(JsonToken.Bytes, oid);
                    break;
                case BsonType.Boolean:
                    bool b = Convert.ToBoolean(this.ReadByte());
                    this.SetToken(JsonToken.Boolean, b);
                    break;
                case BsonType.Date:
                    long ticks = this.ReadInt64();
                    DateTime utcDateTime = JsonConvert.ConvertJavaScriptTicksToDateTime(ticks);
                    
                    DateTime dateTime;
                    switch (this.DateTimeKindHandling)
                    {
                        case DateTimeKind.Unspecified:
                            dateTime = DateTime.SpecifyKind(utcDateTime, DateTimeKind.Unspecified);
                            break;
                        case DateTimeKind.Local:
                            dateTime = utcDateTime.ToLocalTime();
                            break;
                        default:
                            dateTime = utcDateTime;
                            break;
                    }
                    
                    this.SetToken(JsonToken.Date, dateTime);
                    break;
                case BsonType.Null:
                    this.SetToken(JsonToken.Null);
                    break;
                case BsonType.Regex:
                    string expression = this.ReadString();
                    string modifiers = this.ReadString();
                    
                    string regex = string.Format("{0}{1}{2}{3}", @"/", expression, @"/", modifiers);
                    this.SetToken(JsonToken.String, regex);
                    break;
                case BsonType.Reference:
                    this.SetToken(JsonToken.StartObject);
                    this._bsonReaderState = BsonReaderState.ReferenceStart;
                    break;
                case BsonType.Code:
                    this.SetToken(JsonToken.String, this.ReadLengthString());
                    break;
                case BsonType.CodeWScope:
                    this.SetToken(JsonToken.StartObject);
                    this._bsonReaderState = BsonReaderState.CodeWScopeStart;
                    break;
                case BsonType.Integer:
                    this.SetToken(JsonToken.Integer, (long)this.ReadInt32());
                    break;
                case BsonType.TimeStamp:
                case BsonType.Long:
                    this.SetToken(JsonToken.Integer, this.ReadInt64());
                    break;
                default:
                    throw new ArgumentOutOfRangeException("type", string.Format("Unexpected BsonType value: {0}", type));
            }
        }
        
        private byte[] ReadBinary()
        {
            int dataLength = this.ReadInt32();
            
            BsonBinaryType binaryType = (BsonBinaryType)this.ReadByte();
            
            #pragma warning disable 612,618
            // the old binary type has the data length repeated in the data for some reason
            if (binaryType == BsonBinaryType.Data && !this.JsonNet35BinaryCompatibility)
            {
                dataLength = this.ReadInt32();
            }
            #pragma warning restore 612,618

            return this.ReadBytes(dataLength);
        }
        
        private string ReadString()
        {
            this.EnsureBuffers();
            
            StringBuilder builder = null;
            
            int totalBytesRead = 0;
            // used in case of left over multibyte characters in the buffer
            int offset = 0;
            do
            {
                int count = offset;
                byte b;
                while (count < MaxCharBytesSize && (b = this._reader.ReadByte()) > 0)
                {
                    this._byteBuffer[count++] = b;
                }
                int byteCount = count - offset;
                totalBytesRead += byteCount;
                
                if (count < MaxCharBytesSize && builder == null)
                {
                    // pref optimization to avoid reading into a string builder
                    // if string is smaller than the buffer then return it directly
                    int length = Encoding.UTF8.GetChars(this._byteBuffer, 0, byteCount, this._charBuffer, 0);
                    
                    this.MovePosition(totalBytesRead + 1);
                    return new string(this._charBuffer, 0, length);
                }
                else
                {
                    // calculate the index of the end of the last full character in the buffer
                    int lastFullCharStop = this.GetLastFullCharStop(count - 1);
                    
                    int charCount = Encoding.UTF8.GetChars(this._byteBuffer, 0, lastFullCharStop + 1, this._charBuffer, 0);
                    
                    if (builder == null)
                    {
                        builder = new StringBuilder(MaxCharBytesSize * 2);
                    }
                    
                    builder.Append(this._charBuffer, 0, charCount);
                    
                    if (lastFullCharStop < byteCount - 1)
                    {
                        offset = byteCount - lastFullCharStop - 1;
                        // copy left over multi byte characters to beginning of buffer for next iteration
                        Array.Copy(this._byteBuffer, lastFullCharStop + 1, this._byteBuffer, 0, offset);
                    }
                    else
                    {
                        // reached end of string
                        if (count < MaxCharBytesSize)
                        {
                            this.MovePosition(totalBytesRead + 1);
                            return builder.ToString();
                        }
                        
                        offset = 0;
                    }
                }
            }
            while (true);
        }
        
        private string ReadLengthString()
        {
            int length = this.ReadInt32();
            
            this.MovePosition(length);

            string s = this.GetString(length - 1);
            this._reader.ReadByte();

            return s;
        }
        
        private string GetString(int length)
        {
            if (length == 0)
            {
                return string.Empty;
            }
            
            this.EnsureBuffers();
            
            StringBuilder builder = null;
            
            int totalBytesRead = 0;
            
            // used in case of left over multibyte characters in the buffer
            int offset = 0;
            do
            {
                int count = ((length - totalBytesRead) > MaxCharBytesSize - offset)
                            ? MaxCharBytesSize - offset
                            : length - totalBytesRead;
                
                int byteCount = this._reader.Read(this._byteBuffer, offset, count);
                
                if (byteCount == 0)
                {
                    throw new EndOfStreamException("Unable to read beyond the end of the stream.");
                }
                
                totalBytesRead += byteCount;
                
                // Above, byteCount is how many bytes we read this time.
                // Below, byteCount is how many bytes are in the _byteBuffer.
                byteCount += offset;
                
                if (byteCount == length)
                {
                    // pref optimization to avoid reading into a string builder
                    // first iteration and all bytes read then return string directly
                    int charCount = Encoding.UTF8.GetChars(this._byteBuffer, 0, byteCount, this._charBuffer, 0);
                    return new string(this._charBuffer, 0, charCount);
                }
                else
                {
                    int lastFullCharStop = this.GetLastFullCharStop(byteCount - 1);
                    
                    if (builder == null)
                    {
                        builder = new StringBuilder(length);
                    }

                    int charCount = Encoding.UTF8.GetChars(this._byteBuffer, 0, lastFullCharStop + 1, this._charBuffer, 0);
                    builder.Append(this._charBuffer, 0, charCount);
                    
                    if (lastFullCharStop < byteCount - 1)
                    {
                        offset = byteCount - lastFullCharStop - 1;
                        // copy left over multi byte characters to beginning of buffer for next iteration
                        Array.Copy(this._byteBuffer, lastFullCharStop + 1, this._byteBuffer, 0, offset);
                    }
                    else
                    {
                        offset = 0;
                    }
                }
            }
            while (totalBytesRead < length);

            return builder.ToString();
        }
        
        private int GetLastFullCharStop(int start)
        {
            int lookbackPos = start;
            int bis = 0;
            while (lookbackPos >= 0)
            {
                bis = this.BytesInSequence(_byteBuffer[lookbackPos]);
                if (bis == 0)
                {
                    lookbackPos--;
                    continue;
                }
                else if (bis == 1)
                {
                    break;
                }
                else
                {
                    lookbackPos--;
                    break;
                }
            }
            if (bis == start - lookbackPos)
            {
                //Full character.
                return start;
            }
            else
            {
                return lookbackPos;
            }
        }
        
        private int BytesInSequence(byte b)
        {
            if (b <= SeqRange1[1])
            {
                return 1;
            }
            if (b >= SeqRange2[0] && b <= SeqRange2[1])
            {
                return 2;
            }
            if (b >= SeqRange3[0] && b <= SeqRange3[1])
            {
                return 3;
            }
            if (b >= SeqRange4[0] && b <= SeqRange4[1])
            {
                return 4;
            }
            return 0;
        }
        
        private void EnsureBuffers()
        {
            if (this._byteBuffer == null)
            {
                this._byteBuffer = new byte[MaxCharBytesSize];
            }
            if (this._charBuffer == null)
            {
                int charBufferSize = Encoding.UTF8.GetMaxCharCount(MaxCharBytesSize);
                this._charBuffer = new char[charBufferSize];
            }
        }
        
        private double ReadDouble()
        {
            this.MovePosition(8);
            return this._reader.ReadDouble();
        }
        
        private int ReadInt32()
        {
            this.MovePosition(4);
            return this._reader.ReadInt32();
        }
        
        private long ReadInt64()
        {
            this.MovePosition(8);
            return this._reader.ReadInt64();
        }
        
        private BsonType ReadType()
        {
            this.MovePosition(1);
            return (BsonType)this._reader.ReadSByte();
        }
        
        private void MovePosition(int count)
        {
            this._currentContext.Position += count;
        }
        
        private byte[] ReadBytes(int count)
        {
            this.MovePosition(count);
            return this._reader.ReadBytes(count);
        }
    }
}