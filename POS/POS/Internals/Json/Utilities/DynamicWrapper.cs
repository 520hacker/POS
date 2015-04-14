#if !SILVERLIGHT && !PocketPC
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Resources;
using Lib.JSON.Utilities;

namespace Creek.Data.JSON.Net.Utilities
{
    internal static class DynamicWrapper
    {
        private static readonly object _lock = new object();
        private static readonly WrapperDictionary _wrapperDictionary = new WrapperDictionary();

        private static ModuleBuilder _moduleBuilder;

        private static ModuleBuilder ModuleBuilder
        {
            get
            {
                Init();
                return _moduleBuilder;
            }
        }

        private static void Init()
        {
            if (_moduleBuilder == null)
            {
                lock (_lock)
                {
                    if (_moduleBuilder == null)
                    {
                        AssemblyName assemblyName = new AssemblyName("Lib.JSON.Dynamic");
                        assemblyName.KeyPair = new StrongNameKeyPair(GetStrongKey());

                        AssemblyBuilder assembly = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
                        _moduleBuilder = assembly.DefineDynamicModule("Lib.JSON.DynamicModule", false);
                    }
                }
            }
        }

        private static byte[] GetStrongKey()
        {
            const string name = "Lib.JSON.Dynamic.snk";

            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(name))
            {
                if (stream == null)
                {
                    throw new MissingManifestResourceException(string.Format("Should have {0} as an embedded resource.", name));
                }

                int length = (int)stream.Length;
                byte[] buffer = new byte[length];
                stream.Read(buffer, 0, length);

                return buffer;
            }
        }

        public static Type GetWrapper(Type interfaceType, Type realObjectType)
        {
            Type wrapperType = _wrapperDictionary.GetType(interfaceType, realObjectType);

            if (wrapperType == null)
            {
                lock (_lock)
                {
                    wrapperType = _wrapperDictionary.GetType(interfaceType, realObjectType);

                    if (wrapperType == null)
                    {
                        wrapperType = GenerateWrapperType(interfaceType, realObjectType);
                        _wrapperDictionary.SetType(interfaceType, realObjectType, wrapperType);
                    }
                }
            }

            return wrapperType;
        }

        public static object GetUnderlyingObject(object wrapper)
        {
            DynamicWrapperBase wrapperBase = wrapper as DynamicWrapperBase;
            if (wrapperBase == null)
            {
                throw new ArgumentException("Object is not a wrapper.", "wrapper");
            }

            return wrapperBase.UnderlyingObject;
        }

        private static Type GenerateWrapperType(Type interfaceType, Type underlyingType)
        {
            TypeBuilder wrapperBuilder = ModuleBuilder.DefineType(
                "{0}_{1}_Wrapper".FormatWith(CultureInfo.InvariantCulture, interfaceType.Name, underlyingType.Name),
                TypeAttributes.NotPublic | TypeAttributes.Sealed,
                typeof(DynamicWrapperBase),
                new[] { interfaceType });

            WrapperMethodBuilder wrapperMethod = new WrapperMethodBuilder(underlyingType, wrapperBuilder);

            foreach (MethodInfo method in interfaceType.AllMethods())
            {
                wrapperMethod.Generate(method);
            }

            return wrapperBuilder.CreateType();
        }

        public static T CreateWrapper<T>(object realObject) where T : class
        {
            var dynamicType = GetWrapper(typeof(T), realObject.GetType());
            var dynamicWrapper = (DynamicWrapperBase)Activator.CreateInstance(dynamicType);

            dynamicWrapper.UnderlyingObject = realObject;

            return dynamicWrapper as T;
        }
    }
}
#endif