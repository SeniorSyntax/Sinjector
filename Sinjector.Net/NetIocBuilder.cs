using System;
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

    public void RegisterTestDoubleInstance(object instance, Type[] asTypes)
    {
        serviceCollection.AddSingleton(instance.GetType(), instance);
        foreach (var asType in asTypes)
        {
            serviceCollection.AddSingleton(asType, instance); 
        }
    }

    public object AddService(Type type, bool instancePerLifeTimeScope)
    {
        return instancePerLifeTimeScope ?
            serviceCollection.AddScoped(type) :
            serviceCollection.AddSingleton(type);
    }

    public void AddService<TService>(TService instance) where TService : class
    {
        serviceCollection.AddSingleton(instance.GetType(), instance);
    }

    public void Add(object actionOnBuilder)
    {
        var action = (Action<IServiceCollection>)actionOnBuilder;
        action(serviceCollection);
    }

    public ITheContainer Build()
    {
        return new NetIocContainer(serviceCollection.BuildServiceProvider());
    }
}