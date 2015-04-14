namespace Rpc.Internals
{
    using System;
    using System.Collections;
    using System.Globalization;
    using System.IO;
    using System.Xml;

    /// <summary>Basic XML-RPC data deserializer.</summary>
    /// <remarks>Uses <c>XmlTextReader</c> to parse the XML data. This level of the class 
    /// only handles the tokens common to both Requests and Responses. This class is not useful in and of itself
    /// but is designed to be subclassed.</remarks>
    internal class XmlRpcDeserializer : XmlRpcXmlTokens
    {
        private static readonly DateTimeFormatInfo _dateFormat = new DateTimeFormatInfo();

        private Object _container;
        private Stack _containerStack;

        /// <summary>Protected reference to last text.</summary>
        protected String _text;
        /// <summary>Protected reference to last deserialized value.</summary>
        protected Object _value;
        /// <summary>Protected reference to last name field.</summary>
        protected String _name;

        /// <summary>Basic constructor.</summary>
        public XmlRpcDeserializer()
        {
            this.Reset();
            _dateFormat.FullDateTimePattern = ISO_DATETIME;
        }

        /// <summary>Static method that parses XML data into a response using the Singleton.</summary>
        /// <param name="xmlData"><c>StreamReader</c> containing an XML-RPC response.</param>
        /// <returns><c>Object</c> object resulting from the deserialization.</returns>
        virtual public Object Deserialize(TextReader xmlData)
        {
            return null;
        }

        /// <summary>Protected method to parse a node in an XML-RPC XML stream.</summary>
        /// <remarks>Method deals with elements common to all XML-RPC data, subclasses of 
        /// this object deal with request/response spefic elements.</remarks>
        /// <param name="reader"><c>XmlTextReader</c> of the in progress parsing data stream.</param>
        protected void DeserializeNode(XmlTextReader reader)
        {
            switch (reader.NodeType)
            {
                case XmlNodeType.Element:
                    if (Logger.Delegate != null)
                    {
                        Logger.WriteEntry(string.Format("START {0}", reader.Name), LogLevel.Information);
                    }
                    switch (reader.Name)
                    {
                        case VALUE:
                            this._value = null;
                            this._text = null;
                            break;
                        case STRUCT:
                            this.PushContext();
                            this._container = new Hashtable();
                            break;
                        case ARRAY:
                            this.PushContext();
                            this._container = new ArrayList();
                            break;
                    }
                    break;
                case XmlNodeType.EndElement:
                    if (Logger.Delegate != null)
                    {
                        Logger.WriteEntry(string.Format("END {0}", reader.Name), LogLevel.Information);
                    }
                    switch (reader.Name)
                    {
                        case BASE64:
                            this._value = Convert.FromBase64String(this._text);
                            break;
                        case BOOLEAN:
                            int val = Int16.Parse(this._text);
                            if (val == 0)
                            {
                                this._value = false;
                            }
                            else if (val == 1)
                            {
                                this._value = true;
                            }
                            break;
                        case STRING:
                            this._value = this._text;
                            break;
                        case DOUBLE:
                            this._value = Double.Parse(this._text);
                            break;
                        case INT:
                        case ALT_INT:
                            this._value = Int32.Parse(this._text);
                            break;
                        case DATETIME:
                            #if __MONO__
		    _value = DateParse(_text);
                            #else
                            this._value = DateTime.ParseExact(this._text, "F", _dateFormat);
                            #endif
                            break;
                        case NAME:
                            this._name = this._text;
                            break;
                        case VALUE:
                            if (this._value == null)
                            {
                                this._value = this._text; // some kits don't use <string> tag, they just do <value>
                            }

                            if ((this._container != null) && (this._container is IList)) // in an array?  If so add value to it.
                            {
                                ((IList)this._container).Add(this._value);
                            }
                            break;
                        case MEMBER:
                            if ((this._container != null) && (this._container is IDictionary)) // in an struct?  If so add value to it.
                            {
                                ((IDictionary)this._container).Add(this._name, this._value);
                            }
                            break;
                        case ARRAY:
                        case STRUCT:
                            this._value = this._container;
                            this.PopContext();
                            break;
                    }
                    break;
                case XmlNodeType.Text:
                    if (Logger.Delegate != null)
                    {
                        Logger.WriteEntry(string.Format("Text {0}", reader.Value), LogLevel.Information);
                    }
                    this._text = reader.Value;
                    break;
                default:
                    break;
            }
        }

        /// <summary>Static method that parses XML in a <c>String</c> into a 
        /// request using the Singleton.</summary>
        /// <param name="xmlData"><c>String</c> containing an XML-RPC request.</param>
        /// <returns><c>XmlRpcRequest</c> object resulting from the parse.</returns>
        public Object Deserialize(String xmlData)
        {
            StringReader sr = new StringReader(xmlData);
            return this.Deserialize(sr);
        }

        /// <summary>Pop a Context of the stack, an Array or Struct has closed.</summary>
        private void PopContext()
        {
            Context c = (Context)this._containerStack.Pop();
            this._container = c.Container;
            this._name = c.Name;
        }

        /// <summary>Push a Context on the stack, an Array or Struct has opened.</summary>
        private void PushContext()
        {
            Context context;

            context.Container = this._container;
            context.Name = this._name;

            this._containerStack.Push(context);
        }

        /// <summary>Reset the private state of the deserializer.</summary>
        protected void Reset()
        {
            this._text = null;
            this._value = null;
            this._name = null;
            this._container = null;
            this._containerStack = new Stack();
        }
        #if __MONO__
    private DateTime DateParse(String str)
      {
	int year = Int32.Parse(str.Substring(0,4));
	int month = Int32.Parse(str.Substring(4,2));
	int day = Int32.Parse(str.Substring(6,2));
	int hour = Int32.Parse(str.Substring(9,2));
	int min = Int32.Parse(str.Substring(12,2));
	int sec = Int32.Parse(str.Substring(15,2));
	return new DateTime(year,month,day,hour,min,sec);
      }
        #endif
    }
}