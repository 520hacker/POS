namespace Rpc.Internals
{
    using System;
    using System.Collections;

    /// <summary>Class designed to represent an XML-RPC response.</summary>
    internal class XmlRpcResponse
    {
        /// <summary><c>bool</c> indicating if this response represents a fault.</summary>
        public bool IsFault;

        private Object _value;

        /// <summary>Basic constructor</summary>
        public XmlRpcResponse()
        {
            this.Value = null;
            this.IsFault = false;
        }

        /// <summary>Constructor for a fault.</summary>
        /// <param name="code"><c>int</c> the numeric faultCode value.</param>
        /// <param name="message"><c>String</c> the faultString value.</param>
        public XmlRpcResponse(int code, String message) : this()
        {
            this.SetFault(code, message);
        }

        /// <summary>The data value of the response, may be fault data.</summary>
        public Object Value
        {
            get
            {
                return this._value;
            }
            set
            {
                this.IsFault = false;
                this._value = value;
            }
        }

        /// <summary>The faultCode if this is a fault.</summary>
        public int FaultCode
        {
            get
            {
                if (!this.IsFault)
                {
                    return 0;
                }
                else
                {
                    return (int)((Hashtable)this._value)[XmlRpcXmlTokens.FAULT_CODE];
                }
            }
        }

        /// <summary>The faultString if this is a fault.</summary>
        public String FaultString
        {
            get
            {
                if (!this.IsFault)
                {
                    return "";
                }
                else
                {
                    return (String)((Hashtable)this._value)[XmlRpcXmlTokens.FAULT_STRING];
                }
            }
        }

        /// <summary>Set this response to be a fault.</summary>
        /// <param name="code"><c>int</c> the numeric faultCode value.</param>
        /// <param name="message"><c>String</c> the faultString value.</param>
        public void SetFault(int code, String message)
        {
            Hashtable fault = new Hashtable();
            fault.Add("faultCode", code);
            fault.Add("faultString", message);
            this.Value = fault;
            this.IsFault = true;
        }

        /// <summary>Form a useful string representation of the object, in this case the XML response.</summary>
        /// <returns><c>String</c> The XML serialized XML-RPC response.</returns>
        override public String ToString()
        {
            return XmlRpcResponseSerializer.Singleton.Serialize(this);
        }
    }
}