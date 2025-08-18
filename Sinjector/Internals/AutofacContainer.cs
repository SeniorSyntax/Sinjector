using System;
using Autofac;

namespace Sinjector.Internals;

public class AutofacBuilder(ContainerBuilder builder) : ITheContainerBuilder
{
    public ContainerBuilder ContainerBuilder { get; } = builder;

    public ITheContainer Build()
    {
        return new AutofacContainer(ContainerBuilder.Build());
    }
}

public class AutofacContainer(IContainer container) : ITheContainer
{
    public object Resolve(Type type) => 
        container.Resolve(type);

    public T Resolve<T>() => 
        container.Resolve<T>();

    public void Dispose() => 
        container.Dispose();
}