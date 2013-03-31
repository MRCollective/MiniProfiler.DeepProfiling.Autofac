using System.Collections;
using System.Linq;
using Autofac;
using Autofac.Core;
using Castle.DynamicProxy;

namespace StackExchange.Profiling.DeepProfiling.Autofac
{
    public class MiniProfilerInterceptionModule : Module
    {
        private static readonly ProxyGenerator ProxyGenerator = new ProxyGenerator();

        protected override void AttachToComponentRegistration(IComponentRegistry componentRegistry, IComponentRegistration registration)
        {
            registration.Activating += (sender, e) =>
            {
                //Todo: Unit tests
                //Todo: Ensure this works for special cases: IEnumerable, Func etc
                var serviceType = ((TypedService)registration.Services.First()).ServiceType;
                if (!serviceType.IsInterface || !serviceType.IsVisible || serviceType.IsAssignableTo<IEnumerable>() || e.Instance is IProxyTargetAccessor)
                {
                    return;
                }

                e.Instance = ProxyGenerator.CreateInterfaceProxyWithTarget(serviceType, e.Instance, new ProfilingInterceptor());
            };

            base.AttachToComponentRegistration(componentRegistry, registration);
        }
    }
}
