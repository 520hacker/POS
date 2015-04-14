namespace Rpc.Internals
{
    using System;
    using System.Runtime.Remoting.Messaging;
    using System.Runtime.Remoting.Proxies;

    /// <summary>This class provides support for creating local proxies of XML-RPC remote objects</summary>
    /// <remarks>
    /// To create a local proxy you need to create a local C# interface and then, via <i>createProxy</i>
    /// associate that interface with a remote object at a given URL.
    /// </remarks>
    internal class XmlRpcClientProxy : RealProxy
    {
        private readonly String _remoteObjectName;
        private readonly String _url;
        private readonly XmlRpcRequest _client = new XmlRpcRequest();

        private XmlRpcClientProxy(String remoteObjectName, String url, Type t) : base(t)
        {
            this._remoteObjectName = remoteObjectName;
            this._url = url;
        }

        /// <summary>Factory method to create proxies.</summary>
        /// <remarks>
        /// To create a local proxy you need to create a local C# interface with methods that mirror those of the server object.
        /// Next, pass that interface into <c>createProxy</c> along with the object name and URL of the remote object and 
        /// cast the resulting object to the specifice interface.
        /// </remarks>
        /// <param name="remoteObjectName"><c>String</c> The name of the remote object.</param>
        /// <param name="url"><c>String</c> The URL of the remote object.</param>
        /// <param name="anInterface"><c>Type</c> The typeof() of a C# interface.</param>
        /// <returns><c>Object</c> A proxy for your specified interface. Cast to appropriate type.</returns>
        public static Object createProxy(String remoteObjectName, String url, Type anInterface)
        {
            return new XmlRpcClientProxy(remoteObjectName, url, anInterface).GetTransparentProxy();
        }

        /// <summary>The local method dispatcher - do not invoke.</summary>
        override public IMessage Invoke(IMessage msg)
        {
            IMethodCallMessage methodMessage = (IMethodCallMessage)msg;

            this._client.MethodName = string.Format("{0}.{1}", this._remoteObjectName, methodMessage.MethodName);
            this._client.Params.Clear();
            foreach (Object o in methodMessage.Args)
            {
                this._client.Params.Add(o);
            }

            try
            {
                Object ret = this._client.Invoke(this._url);
                return new ReturnMessage(ret,null,0,
                    methodMessage.LogicalCallContext, methodMessage);
            }
            catch (Exception e)
            {
                return new ReturnMessage(e, methodMessage);
            }
        }
    }
}