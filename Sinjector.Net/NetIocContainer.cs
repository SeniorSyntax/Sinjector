using System;
using Microsoft.Extensions.DependencyInjection;

namespace Sinjector;

public class NetIocContainer(ServiceProvider serviceProvider) : ISinjectorContainer
{
    public object Resolve(Type type) => 
        serviceProvider.GetService(type);

    public void Dispose() => 
        serviceProvider.Dispose();
}