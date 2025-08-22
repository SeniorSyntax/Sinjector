using System;
using Microsoft.Extensions.DependencyInjection;

namespace Sinjector;

public class NetIocContainer(ServiceProvider serviceProvider) : ITheContainer
{
    public object Resolve(Type type)
    {
        return serviceProvider.GetService(type);
    }

    public T Resolve<T>()
    {
        return serviceProvider.GetService<T>();
    }
    
    public void Dispose()
    {
        serviceProvider.Dispose();
    }
}