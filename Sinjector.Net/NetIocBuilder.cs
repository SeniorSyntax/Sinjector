using System;
using Microsoft.Extensions.DependencyInjection;

namespace Sinjector;

public class NetIocBuilder(IServiceCollection serviceCollection) : ISinjectorContainerBuilder
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
    
    public void Add(object actionOnBuilder)
    {
        var action = (Action<IServiceCollection>)actionOnBuilder;
        action(serviceCollection);
    }

    public virtual ISinjectorContainer Build() => 
        new NetIocContainer(serviceCollection.BuildServiceProvider());
}