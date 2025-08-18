using System;
using Autofac;

namespace Sinjector.Internals;

public class AutofacBuilder : ITheContainerBuilder
{
    public AutofacBuilder(ContainerBuilder builder)
    {
        ContainerBuilder = builder;
    }
    
    public ContainerBuilder ContainerBuilder { get; } = new ();
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