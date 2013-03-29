using System.Linq;
using Castle.DynamicProxy;

namespace StackExchange.Profiling.DeepProfiling.Autofac
{
    internal class ProfilingInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            var parameters = string.Join(", ", invocation.Arguments.Select(a => (a ?? "").ToString()).ToArray());

            using (MiniProfiler.Current.Step(string.Format("Method {0} ({1})\nParams {2}", invocation.Method.Name, invocation.InvocationTarget.GetType().Name, parameters)))
            {
                invocation.Proceed();
            }
        }
    }
}