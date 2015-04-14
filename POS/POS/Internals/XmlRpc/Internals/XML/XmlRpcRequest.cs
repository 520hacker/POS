namespace Rpc.Internals
{
    using System;
    using System.Collections;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Xml;

    /// <summary>Class supporting the request side of an XML-RPC transaction.</summary>
    internal class XmlRpcRequest
    {
        /// <summary><c>ArrayList</c> containing the parameters.</summary>
        protected IList _params = null;

        private readonly Encoding _encoding = new ASCIIEncoding();
        private readonly XmlRpcRequestSerializer _serializer = new XmlRpcRequestSerializer();
        private readonly XmlRpcResponseDeserializer _deserializer = new XmlRpcResponseDeserializer();

        private String _methodName = null;

        /// <summary>Instantiate an <c>XmlRpcRequest</c></summary>
        public XmlRpcRequest()
        {
            this._params = new ArrayList();
        }

        /// <summary>Instantiate an <c>XmlRpcRequest</c> for a specified method and parameters.</summary>
        /// <param name="methodName"><c>String</c> designating the <i>object.method</i> on the server the request
        /// should be directed to.</param>
        /// <param name="parameters"><c>ArrayList</c> of XML-RPC type parameters to invoke the request with.</param>
        public XmlRpcRequest(String methodName, IList parameters)
        {
            this.MethodName = methodName;
            this._params = parameters;
        }

        /// <summary><c>ArrayList</c> conntaining the parameters for the request.</summary>
        public virtual IList Params
        {
            get
            {
                return this._params;
            }
        }

        /// <summary><c>String</c> conntaining the method name, both object and method, that the request will be sent to.</summary>
        public virtual String MethodName
        {
            get
            {
                return this._methodName;
            }
            set
            {
                this._methodName = value;
            }
        }

        /// <summary><c>String</c> object name portion of the method name.</summary>
        public String MethodNameObject
        {
            get
            {
                int index = this.MethodName.IndexOf(".");

                if (index == -1)
                {
                    return this.MethodName;
                }

                return this.MethodName.Substring(0, index);
            }
        }

        /// <summary><c>String</c> method name portion of the object.method name.</summary>
        public String MethodNameMethod
        {
            get
            {
                int index = this.MethodName.IndexOf(".");

                if (index == -1)
                {
                    return this.MethodName;
                }

                return this.MethodName.Substring(index + 1, this.MethodName.Length - index - 1);
            }
        }

        /// <summary>Invoke this request on the server.</summary>
        /// <param name="url"><c>String</c> The url of the XML-RPC server.</param>
        /// <returns><c>Object</c> The value returned from the method invocation on the server.</returns>
        /// <exception cref="XmlRpcException">If an exception generated on the server side.</exception>
        public Object Invoke(String url)
        {
            XmlRpcResponse res = this.Send(url);

            if (res.IsFault)
            {
                throw new XmlRpcException(res.FaultCode, res.FaultString);
            }
	
            return res.Value;
        }

        /// <summary>Send the request to the server.</summary>
        /// <param name="url"><c>String</c> The url of the XML-RPC server.</param>
        /// <returns><c>XmlRpcResponse</c> The response generated.</returns>
        public XmlRpcResponse Send(String url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            if (request == null)
            {
                throw new XmlRpcException(XmlRpcErrorCodes.TRANSPORT_ERROR,
                    string.Format("{0}: Could not create request with {1}", XmlRpcErrorCodes.TRANSPORT_ERROR_MSG, url));
            }
            request.Method = "POST";
            request.ContentType = "text/xml";
            request.AllowWriteStreamBuffering = true;

            Stream stream = request.GetRequestStream();
            XmlTextWriter xml = new XmlTextWriter(stream, this._encoding);
            this._serializer.Serialize(xml, this);
            xml.Flush();
            xml.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader input = new StreamReader(response.GetResponseStream());

            XmlRpcResponse resp = (XmlRpcResponse)this._deserializer.Deserialize(input);
            input.Close();
            response.Close();
            return resp;
        }

        /// <summary>Produce <c>String</c> representation of the object.</summary>
        /// <returns><c>String</c> representation of the object.</returns>
        override public String ToString()
        {
            return this._serializer.Serialize(this);
        }
    }
}