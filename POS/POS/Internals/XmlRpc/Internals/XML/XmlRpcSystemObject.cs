namespace Rpc.Internals
{
    using System;
    using System.Collections;
    using System.Reflection;

    /// <summary> XML-RPC System object implementation of extended specifications.</summary>
    [XmlRpcExposed]
    internal class XmlRpcSystemObject
    {
        private static readonly IDictionary _methodHelp = new Hashtable();

        private readonly XmlRpcServer _server;

        /// <summary>Constructor.</summary>
        /// <param name="server"><c>XmlRpcServer</c> server to be the system object for.</param>
        public XmlRpcSystemObject(XmlRpcServer server)
        {
            this._server = server;
            server.Add("system", this);
            _methodHelp.Add(string.Format("{0}.methodHelp", this.GetType().FullName), "Return a string description.");
        }

        /// <summary>Static <c>IDictionary</c> to hold mappings of method name to associated documentation String</summary>
        static public IDictionary MethodHelp
        {
            get
            {
                return _methodHelp;
            }
        }

        /// <summary>Invoke a method on a given object.</summary>
        /// <remarks>Using reflection, and respecting the <c>XmlRpcExposed</c> attribute,
        /// invoke the <paramref>methodName</paramref> method on the <paramref>target</paramref>
        /// instance with the <paramref>parameters</paramref> provided. All this packages other <c>Invoke</c> methods 
        /// end up calling this.</remarks>
        /// <returns><c>Object</c> the value the invoked method returns.</returns>
        /// <exception cref="XmlRpcException">If method does not exist, is not exposed, parameters invalid, or invocation
        /// results in an exception. Note, the <c>XmlRpcException.Code</c> will indicate cause.</exception>
        static public Object Invoke(Object target, String methodName, IList parameters)
        {
            if (target == null)
            {
                throw new XmlRpcException(XmlRpcErrorCodes.SERVER_ERROR_METHOD,
                    string.Format("{0}: Invalid target object.", XmlRpcErrorCodes.SERVER_ERROR_METHOD_MSG));
            }
	  
            Type type = target.GetType();
            MethodInfo method = type.GetMethod(methodName);

            try
            {
                if (!XmlRpcExposedAttribute.ExposedMethod(target, methodName))
                {
                    throw new XmlRpcException(XmlRpcErrorCodes.SERVER_ERROR_METHOD,
                        string.Format("{0}: Method {1} is not exposed.", XmlRpcErrorCodes.SERVER_ERROR_METHOD_MSG, methodName));
                }
            }
            catch (MissingMethodException me)
            {
                throw new XmlRpcException(XmlRpcErrorCodes.SERVER_ERROR_METHOD,
                    string.Format("{0}: {1}", XmlRpcErrorCodes.SERVER_ERROR_METHOD_MSG, me.Message));
            }

            Object[] args = new Object[parameters.Count];

            int index = 0;
            foreach (Object arg in parameters)
            {
                args[index] = arg;
                index++;
            }

            try
            {
                Object retValue = method.Invoke(target, args);
                if (retValue == null)
                {
                    throw new XmlRpcException(XmlRpcErrorCodes.APPLICATION_ERROR,
                        string.Format("{0}: Method returned NULL.", XmlRpcErrorCodes.APPLICATION_ERROR_MSG));
                }
                return retValue;
            }
            catch (XmlRpcException e)
            {
                throw e;
            }
            catch (ArgumentException ae)
            {
                Logger.WriteEntry(string.Format("{0}: {1}", XmlRpcErrorCodes.SERVER_ERROR_PARAMS_MSG, ae.Message),
                    LogLevel.Information);
                String call = string.Format("{0}( ", methodName);
                foreach (Object o in args)
                {
                    call += o.GetType().Name;
                    call += " ";
                }
                call += ")";
                throw new XmlRpcException(XmlRpcErrorCodes.SERVER_ERROR_PARAMS,
                    string.Format("{0}: Arguement type mismatch invoking {1}", XmlRpcErrorCodes.SERVER_ERROR_PARAMS_MSG, call));
            }
            catch (TargetParameterCountException tpce)
            {
                Logger.WriteEntry(string.Format("{0}: {1}", XmlRpcErrorCodes.SERVER_ERROR_PARAMS_MSG, tpce.Message),
                    LogLevel.Information);
                throw new XmlRpcException(XmlRpcErrorCodes.SERVER_ERROR_PARAMS,
                    string.Format("{0}: Arguement count mismatch invoking {1}", XmlRpcErrorCodes.SERVER_ERROR_PARAMS_MSG, methodName));
            }
            catch (TargetInvocationException tie)
            {
                throw new XmlRpcException(XmlRpcErrorCodes.APPLICATION_ERROR,
                    string.Format("{0} Invoked method {1}: {2}", XmlRpcErrorCodes.APPLICATION_ERROR_MSG, methodName, tie.Message));
            }
        }

        /// <summary>List methods available on all handlers of this server.</summary>
        /// <returns><c>IList</c> An array of <c>Strings</c>, each <c>String</c> will have form "object.method".</returns>
        [XmlRpcExposed]
        public IList listMethods()
        {
            IList methods = new ArrayList();
            Boolean considerExposure;

            foreach (DictionaryEntry handlerEntry in this._server)
            {
                considerExposure = XmlRpcExposedAttribute.IsExposed(handlerEntry.Value.GetType());

                foreach (MemberInfo mi in handlerEntry.Value.GetType().GetMembers())
                {
                    if (mi.MemberType != MemberTypes.Method)
                    {
                        continue;
                    }

                    if (!((MethodInfo)mi).IsPublic)
                    {
                        continue;
                    }

                    if (considerExposure && !XmlRpcExposedAttribute.IsExposed(mi))
                    {
                        continue;
                    }

                    methods.Add(string.Format("{0}.{1}", handlerEntry.Key, mi.Name));
                }
            }

            return methods;
        }

        /// <summary>Given a method name return the possible signatures for it.</summary>
        /// <param name="name"><c>String</c> The object.method name to look up.</param>
        /// <returns><c>IList</c> Of arrays of signatures.</returns>
        [XmlRpcExposed]
        public IList methodSignature(String name)
        {
            IList signatures = new ArrayList();
            int index = name.IndexOf('.');

            if (index < 0)
            {
                return signatures;
            }

            String oName = name.Substring(0, index);
            Object obj = this._server[oName];

            if (obj == null)
            {
                return signatures;
            }

            MemberInfo[] mi = obj.GetType().GetMember(name.Substring(index + 1));
	
            if (mi == null || mi.Length != 1) // for now we want a single signature
            {
                return signatures;
            }

            MethodInfo method;

            try
            {
                method = (MethodInfo)mi[0];
            }
            catch (Exception e)
            {
                Logger.WriteEntry(string.Format("Attempted methodSignature call on {0} caused: {1}", mi[0], e),
                    LogLevel.Information);
                return signatures;
            }

            if (!method.IsPublic)
            {
                return signatures;
            }

            IList signature = new ArrayList();
            signature.Add(method.ReturnType.Name);

            foreach (ParameterInfo param in method.GetParameters())
            {
                signature.Add(param.ParameterType.Name);
            }

            signatures.Add(signature);

            return signatures;
        }

        /// <summary>Help for given method signature. Not implemented yet.</summary>
        /// <param name="name"><c>String</c> The object.method name to look up.</param>
        /// <returns><c>String</c> help text. Rich HTML text.</returns>
        [XmlRpcExposed]
        public String methodHelp(String name)
        {
            String help = null;

            try 
            {
                help = (String)_methodHelp[this._server.MethodName(name)];
            }
            catch (XmlRpcException e)
            {
                throw e;
            }
            catch (Exception)
            {
            /* ignored */ }

            if (help == null)
            {
                help = string.Format("No help available for: {0}", name);
            }

            return help;
        }

        /// <summary>Boxcarring support method.</summary>
        /// <param name="calls"><c>IList</c> of calls</param>
        /// <returns><c>ArrayList</c> of results/faults.</returns>
        [XmlRpcExposed]
        public IList multiCall(IList calls)
        {
            IList responses = new ArrayList();
            XmlRpcResponse fault = new XmlRpcResponse();

            foreach (IDictionary call in calls)
            {
                try
                {
                    XmlRpcRequest req = new XmlRpcRequest((String)call[XmlRpcXmlTokens.METHOD_NAME],
                        (ArrayList)call[XmlRpcXmlTokens.PARAMS]);
                    Object results = this._server.Invoke(req);
                    IList response = new ArrayList();
                    response.Add(results);
                    responses.Add(response);
                }
                catch (XmlRpcException e)
                {
                    fault.SetFault(e.FaultCode, e.FaultString);
                    responses.Add(fault.Value);
                }
                catch (Exception e2)
                {
                    fault.SetFault(XmlRpcErrorCodes.APPLICATION_ERROR,
                        string.Format("{0}: {1}", XmlRpcErrorCodes.APPLICATION_ERROR_MSG, e2.Message));
                    responses.Add(fault.Value);
                }
            }

            return responses;
        }
    }
}