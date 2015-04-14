using System;
using System.Globalization;
using System.Linq;
using Lib.JSON.Utilities;

namespace Lib.JSON.Linq
{
    /// <summary>
    /// Represents a reader that provides fast, non-cached, forward-only access to serialized Json data.
    /// </summary>
    public class JTokenReader : JsonReader, IJsonLineInfo
    {
        private readonly JToken _root;
        private JToken _parent;
        private JToken _current;

        /// <summary>
        /// Initializes a new instance of the <see cref="JTokenReader"/> class.
        /// </summary>
        /// <param name="token">The token to read from.</param>
        public JTokenReader(JToken token)
        {
            ValidationUtils.ArgumentNotNull(token, "token");

            this._root = token;
            this._current = token;
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

            // attempt to convert possible base 64 string to bytes
            if (this.TokenType == JsonToken.String)
            {
                string s = (string)this.Value;
                byte[] data = (s.Length == 0) ? new byte[0] : Convert.FromBase64String(s);
                this.SetToken(JsonToken.Bytes, data);
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
                this.SetToken(JsonToken.Integer, Convert.ToInt32(Value, CultureInfo.InvariantCulture));
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
        /// <returns>A <see cref="Nullable{DateTimeOffset}"/>. This method will return <c>null</c> at the end of an array.</returns>
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
            if (this.CurrentState != State.Start)
            {
                JContainer container = this._current as JContainer;
                if (container != null && this._parent != container)
                {
                    return this.ReadInto(container);
                }
                else
                {
                    return this.ReadOver(_current);
                }
            }

            this.SetToken(_current);
            return true;
        }

        private bool ReadOver(JToken t)
        {
            if (t == this._root)
            {
                return this.ReadToEnd();
            }

            JToken next = t.Next;
            if ((next == null || next == t) || t == t.Parent.Last)
            {
                if (t.Parent == null)
                {
                    return this.ReadToEnd();
                }

                return this.SetEnd(t.Parent);
            }
            else
            {
                this._current = next;
                this.SetToken(_current);
                return true;
            }
        }

        private bool ReadToEnd()
        {
            //CurrentState = State.Finished;
            return false;
        }

        private bool IsEndElement
        {
            get
            {
                return (this._current == this._parent);
            }
        }

        private JsonToken? GetEndToken(JContainer c)
        {
            switch (c.Type)
            {
                case JTokenType.Object:
                    return JsonToken.EndObject;
                case JTokenType.Array:
                    return JsonToken.EndArray;
                case JTokenType.Constructor:
                    return JsonToken.EndConstructor;
                case JTokenType.Property:
                    return null;
                default:
                    throw MiscellaneousUtils.CreateArgumentOutOfRangeException("Type", c.Type, "Unexpected JContainer type.");
            }
        }

        private bool ReadInto(JContainer c)
        {
            JToken firstChild = c.First;
            if (firstChild == null)
            {
                return this.SetEnd(c);
            }
            else
            {
                this.SetToken(firstChild);
                this._current = firstChild;
                this._parent = c;
                return true;
            }
        }

        private bool SetEnd(JContainer c)
        {
            JsonToken? endToken = this.GetEndToken(c);
            if (endToken != null)
            {
                this.SetToken(endToken.Value);
                this._current = c;
                this._parent = c;
                return true;
            }
            else
            {
                return this.ReadOver(c);
            }
        }

        private void SetToken(JToken token)
        {
            switch (token.Type)
            {
                case JTokenType.Object:
                    this.SetToken(JsonToken.StartObject);
                    break;
                case JTokenType.Array:
                    this.SetToken(JsonToken.StartArray);
                    break;
                case JTokenType.Constructor:
                    this.SetToken(JsonToken.StartConstructor);
                    break;
                case JTokenType.Property:
                    this.SetToken(JsonToken.PropertyName, ((JProperty)token).Name);
                    break;
                case JTokenType.Comment:
                    this.SetToken(JsonToken.Comment, ((JValue)token).Value);
                    break;
                case JTokenType.Integer:
                    this.SetToken(JsonToken.Integer, ((JValue)token).Value);
                    break;
                case JTokenType.Float:
                    this.SetToken(JsonToken.Float, ((JValue)token).Value);
                    break;
                case JTokenType.String:
                    this.SetToken(JsonToken.String, ((JValue)token).Value);
                    break;
                case JTokenType.Boolean:
                    this.SetToken(JsonToken.Boolean, ((JValue)token).Value);
                    break;
                case JTokenType.Null:
                    this.SetToken(JsonToken.Null, ((JValue)token).Value);
                    break;
                case JTokenType.Undefined:
                    this.SetToken(JsonToken.Undefined, ((JValue)token).Value);
                    break;
                case JTokenType.Date:
                    this.SetToken(JsonToken.Date, ((JValue)token).Value);
                    break;
                case JTokenType.Raw:
                    this.SetToken(JsonToken.Raw, ((JValue)token).Value);
                    break;
                case JTokenType.Bytes:
                    this.SetToken(JsonToken.Bytes, ((JValue)token).Value);
                    break;
                case JTokenType.Guid:
                    this.SetToken(JsonToken.String, this.SafeToString(((JValue)token).Value));
                    break;
                case JTokenType.Uri:
                    this.SetToken(JsonToken.String, this.SafeToString(((JValue)token).Value));
                    break;
                case JTokenType.TimeSpan:
                    this.SetToken(JsonToken.String, this.SafeToString(((JValue)token).Value));
                    break;
                default:
                    throw MiscellaneousUtils.CreateArgumentOutOfRangeException("Type", token.Type, "Unexpected JTokenType.");
            }
        }

        private string SafeToString(object value)
        {
            return (value != null) ? value.ToString() : null;
        }

        bool IJsonLineInfo.HasLineInfo()
        {
            if (this.CurrentState == State.Start)
            {
                return false;
            }

            IJsonLineInfo info = this.IsEndElement ? null : this._current;
            return (info != null && info.HasLineInfo());
        }

        int IJsonLineInfo.LineNumber
        {
            get
            {
                if (this.CurrentState == State.Start)
                {
                    return 0;
                }

                IJsonLineInfo info = this.IsEndElement ? null : this._current;
                if (info != null)
                {
                    return info.LineNumber;
                }
        
                return 0;
            }
        }

        int IJsonLineInfo.LinePosition
        {
            get
            {
                if (this.CurrentState == State.Start)
                {
                    return 0;
                }

                IJsonLineInfo info = this.IsEndElement ? null : this._current;
                if (info != null)
                {
                    return info.LinePosition;
                }

                return 0;
            }
        }
    }
}