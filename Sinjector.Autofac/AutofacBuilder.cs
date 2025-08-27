using System;
using Autofac;

namespace Sinjector;

public class AutofacBuilder(ContainerBuilder builder) : ISinjectorContainerBuilder
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

    public object AddService(Type type) =>
        builder
            .RegisterType(type)
            .AsSelf()
            .AsImplementedInterfaces()
            .SingleInstance();

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

    public ISinjectorContainer Build() => 
        new AutofacContainer(builder.Build());
}