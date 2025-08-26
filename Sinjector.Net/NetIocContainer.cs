using System;
using Microsoft.Extensions.DependencyInjection;

namespace Sinjector;

public class NetIocContainer(ServiceProvider serviceProvider) : ITheContainer
{
    public object Resolve(Type type) => 
        serviceProvider.GetService(type);

    public T Resolve<T>() => 
        serviceProvider.GetService<T>();

    public void Dispose() => 
        serviceProvider.Dispose();
}