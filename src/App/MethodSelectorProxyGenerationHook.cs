using System;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;

namespace App
{
    public class MethodSelectorProxyGenerationHook :IProxyGenerationHook
    {
        private readonly string[] methodsToIntercept;

        public MethodSelectorProxyGenerationHook(params string[] methodsToIntercept)
        {
            this.methodsToIntercept = methodsToIntercept;
        }

        public void MethodsInspected()
        {
        }

        public void NonProxyableMemberNotification(Type type, MemberInfo memberInfo)
        {
        }

        public bool ShouldInterceptMethod(Type type, MethodInfo methodInfo)
        {
            return methodsToIntercept.Contains(methodInfo.Name);
        }
    }
}