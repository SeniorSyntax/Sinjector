using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Sinjector;

public class NetIocBuilder(IServiceCollection serviceCollection) : ITheContainerBuilder
{
    public void RegisterTestDoubleType(Type type, Type[] asTypes)
    {
        serviceCollection.AddSingleton(type);
        foreach (var asType in asTypes)
        {
            serviceCollection.AddSingleton(asType, sp => sp.GetRequiredService(type));   
        }
    }

    public void RegisterTestDoubleInstance(object instance, Type[] asTypes, bool propHack)
    {
        serviceCollection.AddSingleton(instance);
        foreach (var asType in asTypes)
        {
            serviceCollection.AddSingleton(asType, instance); 
        }
    }

    public object AddService<TService>(bool instancePerLifeTimeScope) where TService : class
    {
        return serviceCollection.AddSingleton<TService>();
    }

    public object AddService(Type type, bool instancePerLifeTimeScope)
    {
        return serviceCollection.AddSingleton(type);
    }

    public void AddService<TService>(TService instance) where TService : class
    {
        serviceCollection.AddSingleton(instance);
    }

    public void Add(object thing)
    {
        throw new NotImplementedException();
    }

    public ITheContainer Build()
    {
        return new NetIocContainer(serviceCollection.BuildServiceProvider());
    }
}