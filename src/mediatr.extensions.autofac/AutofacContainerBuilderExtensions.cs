namespace MediatR
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Autofac;
    using Autofac.Features.Variance;

    public static class AutofacContainerBuilderExtensions
    {
        public static void AddMediatR(this ContainerBuilder builder, Assembly assembly)
        {
            Decorate(builder, assembly);
        }

        private static void RegisterRequestHandlersFromAssembly(ContainerBuilder builder, Assembly assembly)
        {
            builder.RegisterAssemblyTypes(assembly).As(t => t.GetTypeInfo().GetInterfaces()
                .Where(i => i.IsClosedTypeOf(typeof(IRequestHandler<,>))));
        }

        private static void RegisterAsyncRequestHandlersFromAssembly(ContainerBuilder builder, Assembly assembly)
        {
            builder.RegisterAssemblyTypes(assembly).As(t => t.GetTypeInfo().GetInterfaces()
                .Where(i => i.IsClosedTypeOf(typeof(IAsyncRequestHandler<,>))));
        }

        private static void RegisterNotificationHandlersFromAssembly(ContainerBuilder builder, Assembly assembly)
        {
            builder.RegisterAssemblyTypes(assembly)
                .As(t => t.GetTypeInfo().GetInterfaces().Where(i => i.IsClosedTypeOf(typeof(INotificationHandler<>))).ToArray());
        }

        private static void RegisterAsyncNotificationHandlersFromAssembly(ContainerBuilder builder, Assembly assembly)
        {
            builder.RegisterAssemblyTypes(assembly)
                .As(t => t.GetTypeInfo().GetInterfaces().Where(i => i.IsClosedTypeOf(typeof(IAsyncNotificationHandler<>))).ToArray());
        }

        public static void Decorate(ContainerBuilder builder, Assembly handlersAssembly)
        {
            builder.RegisterSource(new ContravariantRegistrationSource());
            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly).AsImplementedInterfaces();
            builder.Register<SingleInstanceFactory>(ctx =>
            {
                var c = ctx.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });
            builder.Register<MultiInstanceFactory>(ctx =>
            {
                var c = ctx.Resolve<IComponentContext>();
                return t => (IEnumerable<object>)c.Resolve(typeof(IEnumerable<>).MakeGenericType(t));
            });

            RegisterRequestHandlersFromAssembly(builder, handlersAssembly);
            RegisterAsyncRequestHandlersFromAssembly(builder, handlersAssembly);
            RegisterNotificationHandlersFromAssembly(builder, handlersAssembly);
            RegisterAsyncNotificationHandlersFromAssembly(builder, handlersAssembly);
        }
    }
}
