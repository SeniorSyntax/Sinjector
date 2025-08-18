using System;
using Autofac;

namespace Sinjector.Internals;

//should be public I think
public class AutofacBuilder(ContainerBuilder builder) : ITheContainerBuilder
{
    public ContainerBuilder ContainerBuilder { get; } = builder;

    public ITheContainer Build()
    {
        return new AutofacContainer(ContainerBuilder.Build());
    }
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