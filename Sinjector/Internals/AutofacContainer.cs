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