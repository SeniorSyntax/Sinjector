using System;
using Autofac;
using Autofac.Core;
using Sinjector.Internals;

namespace Sinjector;

public class AutofacBuilder(ContainerBuilder builder) : ITheContainerBuilder
{
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

    //fix me somehow
    public void RegisterTestDoubleInstance(object instance, Type[] asTypes, bool propHack)
    {
        var reg = builder
            .RegisterInstance(instance)
            .AsSelf()
            .As(asTypes)
            .ExternallyOwned();
        
        //remove me? //////////
        if (propHack)
            reg.PropertiesAutowired();
        //////////////////////
    }

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
    
    public void Add(object thing) => 
        builder.RegisterModule((IModule)thing);

    public ITheContainer Build() => 
        new AutofacContainer(builder.Build());
}