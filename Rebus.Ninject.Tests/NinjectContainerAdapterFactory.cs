using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ninject;
using Rebus.Activation;
using Rebus.Bus;
using Rebus.Config;
using Rebus.Handlers;
using Rebus.Tests.Contracts.Activation;

namespace Rebus.Ninject.Tests
{
    public class NinjectContainerAdapterFactory : IActivationContext
    {
        public IHandlerActivator CreateActivator(Action<IHandlerRegistry> configureHandlers, out IActivatedContainer container)
        {
            var kernel = new StandardKernel();
            configureHandlers(new HandlerRegistry(kernel));

            container = new ActivatedContainer(kernel);

            return new NinjectContainerAdapter(kernel);
        }

        public IBus CreateBus(Action<IHandlerRegistry> configureHandlers, Func<RebusConfigurer, RebusConfigurer> configureBus, out IActivatedContainer container)
        {
            var kernel = new StandardKernel();
            configureHandlers(new HandlerRegistry(kernel));

            container = new ActivatedContainer(kernel);

            return configureBus(Configure.With(new NinjectContainerAdapter(kernel))).Start();
        }

        private class HandlerRegistry : IHandlerRegistry
        {
            private readonly StandardKernel _kernel;

            public HandlerRegistry(StandardKernel kernel)
            {
                _kernel = kernel;
            }

            public IHandlerRegistry Register<THandler>() where THandler : class, IHandleMessages
            {
                _kernel.Bind(GetHandlerInterfaces<THandler>().ToArray())
                    .To<THandler>()
                    .InTransientScope();

                return this;
            }

            static IEnumerable<Type> GetHandlerInterfaces<THandler>() where THandler : class, IHandleMessages
            {
#if NETSTANDARD1_6
                return typeof(THandler).GetTypeInfo().GetInterfaces().Where(i => i.GetTypeInfo().IsGenericType && i.GetGenericTypeDefinition() == typeof(IHandleMessages<>));
#else
            return typeof(THandler).GetInterfaces().Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IHandleMessages<>));
#endif
            }
        }

        private class ActivatedContainer : IActivatedContainer
        {
            private readonly StandardKernel _kernel;

            public ActivatedContainer(StandardKernel kernel)
            {
                _kernel = kernel;
            }

            public void Dispose()
            {
                _kernel.Dispose();
            }

            public IBus ResolveBus()
            {
                return ResolutionExtensions.Get<IBus>(_kernel);
            }
        }
    }
}