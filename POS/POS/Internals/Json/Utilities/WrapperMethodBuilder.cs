using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Creek.Data.JSON.Net.Utilities
{
    internal class WrapperMethodBuilder
    {
        private readonly Type _realObjectType;
        private readonly TypeBuilder _wrapperBuilder;

        public WrapperMethodBuilder(Type realObjectType, TypeBuilder proxyBuilder)
        {
            this._realObjectType = realObjectType;
            this._wrapperBuilder = proxyBuilder;
        }

        public void Generate(MethodInfo newMethod)
        {
            if (newMethod.IsGenericMethod)
            {
                newMethod = newMethod.GetGenericMethodDefinition();
            }

            FieldInfo srcField = typeof(DynamicWrapperBase).GetField("UnderlyingObject", BindingFlags.Instance | BindingFlags.NonPublic);

            var parameters = newMethod.GetParameters();
            var parameterTypes = parameters.Select(parameter => parameter.ParameterType).ToArray();

            MethodBuilder methodBuilder = this._wrapperBuilder.DefineMethod(
                newMethod.Name,
                MethodAttributes.Public | MethodAttributes.Virtual,
                newMethod.ReturnType,
                parameterTypes);

            if (newMethod.IsGenericMethod)
            {
                methodBuilder.DefineGenericParameters(
                    newMethod.GetGenericArguments().Select(arg => arg.Name).ToArray());
            }

            ILGenerator ilGenerator = methodBuilder.GetILGenerator();

            LoadUnderlyingObject(ilGenerator, srcField);
            PushParameters(parameters, ilGenerator);
            this.ExecuteMethod(newMethod, parameterTypes, ilGenerator);
            Return(ilGenerator);
        }

        private static void Return(ILGenerator ilGenerator)
        {
            ilGenerator.Emit(OpCodes.Ret);
        }

        private void ExecuteMethod(MethodBase newMethod, Type[] parameterTypes, ILGenerator ilGenerator)
        {
            MethodInfo srcMethod = this.GetMethod(newMethod, parameterTypes);

            if (srcMethod == null)
            {
                throw new MissingMethodException(string.Format("Unable to find method {0} on {1}", newMethod.Name, this._realObjectType.FullName));
            }

            ilGenerator.Emit(OpCodes.Call, srcMethod);
        }

        private MethodInfo GetMethod(MethodBase realMethod, Type[] parameterTypes)
        {
            if (realMethod.IsGenericMethod)
            {
                return this._realObjectType.GetGenericMethod(realMethod.Name, parameterTypes);
            }

            return this._realObjectType.GetMethod(realMethod.Name, parameterTypes);
        }

        private static void PushParameters(ICollection<ParameterInfo> parameters, ILGenerator ilGenerator)
        {
            for (int i = 1; i < parameters.Count + 1; i++)
            {
                ilGenerator.Emit(OpCodes.Ldarg, i);
            }
        }

        private static void LoadUnderlyingObject(ILGenerator ilGenerator, FieldInfo srcField)
        {
            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Ldfld, srcField);
        }
    }
}