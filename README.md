MiniProfiler.DeepProfiling.Autofac
==================================

Automatically supports profiling all methods called on interfaces resolved from an Autofac IoC container.

You can register the included module in your Autofac container builder:

builder.RegisterModule(new MiniProfilerInterceptionModule());

We suggest registering this module only in a development/testing environment to prevent any additional load being placed on your application in production.

Once the module is registered, MiniProfiler will show you the time spent in each method of your application (where those methods are called on an interface resolved via Autofac). This allows detailed CPU/time profiling into individual methods within your application.
