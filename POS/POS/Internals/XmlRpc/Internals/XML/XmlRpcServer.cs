namespace Rpc.Internals
{
    using System;
    using System.Collections;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;

    /// <summary>A restricted HTTP server for use with XML-RPC.</summary>
    /// <remarks>It only handles POST requests, and only POSTs representing XML-RPC calls.
    /// In addition to dispatching requests it also provides a registry for request handlers. 
    /// </remarks>
    internal class XmlRpcServer : IEnumerable
    {
        private const int RESPONDER_COUNT = 10;
        private readonly int _port;
        private readonly IPAddress _address;
        private readonly IDictionary _handlers;
        private readonly XmlRpcSystemObject _system;
        private readonly WaitCallback _wc;

        private TcpListener _myListener;

        ///<summary>Constructor with port and address.</summary>
        ///<remarks>This constructor sets up a TcpListener listening on the
        ///given port and address. It also calls a Thread on the method StartListen().</remarks>
        ///<param name="address"><c>IPAddress</c> value of the address to listen on.</param>
        ///<param name="port"><c>Int</c> value of the port to listen on.</param>
        public XmlRpcServer(IPAddress address, int port)
        {
            this._port = port;
            this._address = address;
            this._handlers = new Hashtable();
            this._system = new XmlRpcSystemObject(this);
            this._wc = new WaitCallback(this.WaitCallback);
        }

        ///<summary>Basic constructor.</summary>
        ///<remarks>This constructor sets up a TcpListener listening on the
        ///given port. It also calls a Thread on the method StartListen(). IPAddress.Any
        ///is assumed as the address here.</remarks>
        ///<param name="port"><c>Int</c> value of the port to listen on.</param>
        public XmlRpcServer(int port) : this(IPAddress.Any, port)
        {
        }

        /// <summary>Retrieve a handler by name.</summary>
        /// <param name="name"><c>String</c> naming a handler</param>
        /// <returns><c>Object</c> that is the handler.</returns>
        public Object this [String name]
        {
            get
            {
                return this._handlers[name];
            }
        }

        /// <summary>
        /// This function send the Header Information to the client (Browser)
        /// </summary>
        /// <param name="sHttpVersion">HTTP Version</param>
        /// <param name="sMIMEHeader">Mime Type</param>
        /// <param name="iTotBytes">Total Bytes to be sent in the body</param>
        /// <param name="sStatusCode"></param>
        /// <param name="output">Socket reference</param>
        static public void HttpHeader(string sHttpVersion, string sMIMEHeader, long iTotBytes, string sStatusCode, TextWriter output)
        {
            String sBuffer = "";
			
            // if Mime type is not provided set default to text/html
            if (sMIMEHeader.Length == 0)
            {
                sMIMEHeader = "text/html";  // Default Mime Type is text/html
            }

            sBuffer += string.Format("{0}{1}\r\n", sHttpVersion, sStatusCode);
            sBuffer += "Connection: close\r\n";
            if (iTotBytes > 0)
            {
                sBuffer += string.Format("Content-Length: {0}\r\n", iTotBytes);
            }
            sBuffer += "Server: XmlRpcServer \r\n";
            sBuffer += string.Format("Content-Type: {0}\r\n", sMIMEHeader);
            sBuffer += "\r\n";

            output.Write(sBuffer);
        }

        /// <summary>Start the server.</summary>
        public void Start()
        {
            try
            {
                this.Stop();
                //start listing on the given port
                //	    IPAddress addr = IPAddress.Parse("127.0.0.1");
                lock (this)
                {
                    this._myListener = new TcpListener(this._port);
                    this._myListener.Start();
                    //start the thread which calls the method 'StartListen'
                    Thread th = new Thread(new ThreadStart(this.StartListen));
                    th.Start();
                }
            }
            catch (Exception e)
            {
                Logger.WriteEntry(string.Format("An Exception Occurred while Listening :{0}", e.ToString()), LogLevel.Error);
            }
        }

        /// <summary>Stop the server.</summary>
        public void Stop() 
        {
            try
            {
                if (this._myListener != null) 
                {
                    lock (this)
                    {
                        this._myListener.Stop();
                        this._myListener = null;
                    }
                }
            }
            catch (Exception e)
            {
                Logger.WriteEntry(string.Format("An Exception Occurred while stopping :{0}", e.ToString()), LogLevel.Error);
            }
        }

        /// <summary>Get an enumeration of my XML-RPC handlers.</summary>
        /// <returns><c>IEnumerable</c> the handler enumeration.</returns>
        public IEnumerator GetEnumerator()
        {
            return this._handlers.GetEnumerator();
        }

        ///<summary>
        ///This method Accepts new connections and dispatches them when appropriate.
        ///</summary>
        public void StartListen()
        {
            while (true && this._myListener != null)
            {
                //Accept a new connection
                XmlRpcResponder responder = new XmlRpcResponder(this, this._myListener.AcceptTcpClient());
                ThreadPool.QueueUserWorkItem(this._wc, responder);
            }
        }

        ///<summary>
        ///Add an XML-RPC handler object by name.
        ///</summary>
        ///<param name="name"><c>String</c> XML-RPC dispatch name of this object.</param>
        ///<param name="obj"><c>Object</c> The object that is the XML-RPC handler.</param>
        public void Add(String name, Object obj)
        {
            this._handlers.Add(name, obj);
        }

        ///<summary>Return a C# object.method name for and XML-RPC object.method name pair.</summary>
        ///<param name="methodName">The XML-RPC object.method.</param>
        ///<returns><c>String</c> of form object.method for the underlying C# method.</returns>
        public String MethodName(String methodName)
        {
            int dotAt = methodName.LastIndexOf('.');

            if (dotAt == -1)
            {
                throw new XmlRpcException(XmlRpcErrorCodes.SERVER_ERROR_METHOD,
                    string.Format("{0}: Bad method name {1}", XmlRpcErrorCodes.SERVER_ERROR_METHOD_MSG, methodName));
            }
	  
            String objectName = methodName.Substring(0, dotAt);
            Object target = this._handlers[objectName];

            if (target == null)
            {
                throw new XmlRpcException(XmlRpcErrorCodes.SERVER_ERROR_METHOD,
                    string.Format("{0}: Object {1} not found", XmlRpcErrorCodes.SERVER_ERROR_METHOD_MSG, objectName));
            }

            return string.Format("{0}.{1}", target.GetType().FullName, methodName.Substring(dotAt + 1));
        }

        ///<summary>Invoke a method described in a request.</summary>
        ///<param name="req"><c>XmlRpcRequest</c> containing a method descriptions.</param>
        /// <seealso cref="XmlRpcSystemObject.Invoke"/>
        /// <seealso cref="XmlRpcServer.Invoke(String,String,IList)"/>
        public Object Invoke(XmlRpcRequest req)
        {
            return this.Invoke(req.MethodNameObject, req.MethodNameMethod, req.Params);
        }

        ///<summary>Invoke a method on a named handler.</summary>
        ///<param name="objectName"><c>String</c> The name of the handler.</param>
        ///<param name="methodName"><c>String</c> The name of the method to invoke on the handler.</param>
        ///<param name="parameters"><c>IList</c> The parameters to invoke the method with.</param>
        /// <seealso cref="XmlRpcSystemObject.Invoke"/>
        public Object Invoke(String objectName, String methodName, IList parameters)
        {
            Object target = this._handlers[objectName];

            if (target == null)
            {
                throw new XmlRpcException(XmlRpcErrorCodes.SERVER_ERROR_METHOD,
                    string.Format("{0}: Object {1} not found", XmlRpcErrorCodes.SERVER_ERROR_METHOD_MSG, objectName));
            }

            return XmlRpcSystemObject.Invoke(target, methodName, parameters);
        }

        /// <summary>The method the thread pool invokes when a thread is available to handle an HTTP request.</summary>
        /// <param name="responder">TcpClient from the socket accept.</param>
        public void WaitCallback(object responder)
        {
            XmlRpcResponder resp = (XmlRpcResponder)responder;

            if (resp.HttpReq.HttpMethod == "POST")
            {
                try
                {
                    resp.Respond();
                }
                catch (Exception e)
                {
                    Logger.WriteEntry(string.Format("Failed on post: {0}", e), LogLevel.Error);
                }
            }
            else
            {
                Logger.WriteEntry(string.Format("Only POST methods are supported: {0} ignored", resp.HttpReq.HttpMethod), LogLevel.Error);
            }

            resp.Close();
        }
    }
}