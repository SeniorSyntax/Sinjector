using System;
using Autofac;
using Autofac.Core;

namespace Sinjector;

public class AutofacBuilder(ContainerBuilder builder) : ITheContainerBuilder
{
    public void RegisterTestDoubleType(Type type, Type[] asTypes) =>
        builder
            .RegisterType(type)
            .SingleInstance()
            .AsSelf()
            .As(asTypes)
            .ExternallyOwned()
            .PropertiesAutowired();

    public void RegisterTestDoubleInstance(object instance, Type[] asTypes) =>
        builder
            .RegisterInstance(instance)
            .AsSelf()
            .As(asTypes)
            .ExternallyOwned()
            .PropertiesAutowired();

    public object AddService(Type type, bool instancePerLifeTimeScope)
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
    
    public void AddService<TService>(TService instance) where TService : class =>
        builder
            .RegisterInstance(instance)
            .AsSelf()
            .AsImplementedInterfaces()
            .SingleInstance();

    public void Add(object actionOnBuilder)
    {
        var action = (Action<ContainerBuilder>)actionOnBuilder;
        action(builder); 
    }


    public ITheContainer Build() => 
        new AutofacContainer(builder.Build());
}