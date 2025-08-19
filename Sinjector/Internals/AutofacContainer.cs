using System;
using Autofac;

namespace Sinjector.Internals;

//should be public I think
public class AutofacBuilder(ContainerBuilder builder) : ITheContainerBuilder
{
    public ContainerBuilder ContainerBuilder { get; } = builder;

    public void RegisterTestDoubleType(Type type, Type[] asTypes)
    {
        builder
            .RegisterType(type)
            .SingleInstance()
            .AsSelf()
            .As(asTypes)
            .ExternallyOwned()
            .PropertiesAutowired()
            .OnActivated(c =>
            {
                c.Context.Resolve<ITestDoubles>()
                    .KeepInstance(c.Instance, type);
            });
    }

    public void RegisterTestDoubleInstance(object instance, Type[] asTypes)
    {
        builder
            .RegisterInstance(instance)
            .AsSelf()
            .As(asTypes)
            .ExternallyOwned()
            .PropertiesAutowired();
    }
    
    public object AddService<TService>(bool instancePerLifeTimeScope = false)
    {
        var registration = builder
            .RegisterType<TService>()
            .AsSelf()
            .AsImplementedInterfaces();
			
        if (instancePerLifeTimeScope)
            registration.InstancePerLifetimeScope();
        else
            registration.SingleInstance();

        return registration;
    }

    public object AddService(Type type, bool instancePerLifeTimeScope = false)
    {
        var registration = builder
            .RegisterType(type)
            .AsSelf()
            .AsImplementedInterfaces();

        if (instancePerLifeTimeScope)
            registration.InstancePerLifetimeScope();
        else
            registration.SingleInstance();

        return registration;
    }
    
    public void AddService<TService>(TService instance) where TService : class
    {
        builder
            .RegisterInstance(instance)
            .AsSelf()
            .AsImplementedInterfaces()
            .SingleInstance();
    }
    
    public ITheContainer Build() => 
        new AutofacContainer(ContainerBuilder.Build());
}

//should be internal
public class AutofacContainer(IContainer container) : ITheContainer
{
    public object Resolve(Type type) => 
        container.Resolve(type);

    public T Resolve<T>() => 
        container.Resolve<T>();

    public void Dispose() => 
        container.Dispose();
}