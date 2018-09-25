using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Practices.Unity.InterceptionExtension;
using ZJB.Core.Logging;

namespace ZJB.Core.Injection
{
    public class PerformanceInterceptor : IInterceptionBehavior
    {
        readonly Logger logger;

        public PerformanceInterceptor()
        {
            logger = new Logger(GetType());
        }

        public IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
        {
            if(!input.MethodBase.ReflectedType.FullName.StartsWith("ZJB",StringComparison.OrdinalIgnoreCase))
                return getNext().Invoke(input, getNext);

            logger.Debug(string.Format("{0}.{1} Begin.", input.MethodBase.ReflectedType.FullName, input.MethodBase.Name));
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            IMethodReturn methodReturn = getNext().Invoke(input, getNext);
            stopwatch.Stop();
            logger.Debug(string.Format("{0}.{1} End. Cost {2} ms", input.MethodBase.ReflectedType.FullName, input.MethodBase.Name, stopwatch.ElapsedMilliseconds));
            return methodReturn;
        }

        public IEnumerable<Type> GetRequiredInterfaces()
        {
            return Type.EmptyTypes;
        }

        public bool WillExecute
        {
            get { return true; }
        }
    }
}
