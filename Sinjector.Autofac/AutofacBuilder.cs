using System;
using Autofac;

namespace Sinjector;

public class AutofacBuilder(ContainerBuilder builder) : ISinjectorContainerBuilder
{
    public void RegisterTestDoubleType(Type type, Type[] asTypes) =>
        builder
            .RegisterType(type)
            .AsSelf()
            .As(asTypes)
            .SingleInstance();

    public void RegisterTestDoubleInstance(object instance, Type[] asTypes) =>
        builder
            .RegisterInstance(instance)
            .AsSelf()
            .As(asTypes);

    public void Add(object actionOnBuilder)
    {
        var action = (Action<ContainerBuilder>)actionOnBuilder;
        action(builder); 
    }

    public ISinjectorContainer Build() => 
        new AutofacContainer(builder.Build());
}