using System;
using System.Linq;
using Rpc.Internals;

namespace Rpc
{
    public class RpcClient
    {
        public RpcClient(string url)
        {
            this.Url = url;
        }

        public string Url { get; private set; }

        public dynamic CreateProxy<T>()
            where T : IRpcProxy
        {
            return new Proxy(typeof(T), this, typeof(T).Name);
        }

        public object Call(string methodName, params object[] parameters)
        {
            XmlRpcRequest client = new XmlRpcRequest(methodName, parameters);

            XmlRpcResponse response = client.Send(this.Url);

            return response.Value;
        }
    }
}