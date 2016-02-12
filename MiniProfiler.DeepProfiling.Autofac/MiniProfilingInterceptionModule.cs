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
                if (e.Instance is IProxyTargetAccessor)
                    return;

                var services = registration.Services;

                //Todo: Unit tests
                //Todo: Ensure this works for special cases: IEnumerable, Func etc

                var proxyServices = services.Select(s => ((IServiceWithType)s).ServiceType)
                    .Where(s => s.IsInterface && s.IsVisible && !s.IsAssignableTo<IEnumerable>())
                    .ToList();
                if (!proxyServices.Any())
                    return;

                e.Instance = ProxyGenerator.CreateInterfaceProxyWithTarget(proxyServices.First(), proxyServices.Skip(1).ToArray(), e.Instance, new ProfilingInterceptor());
            };

            base.AttachToComponentRegistration(componentRegistry, registration);
        }
    }
}
