using System;

namespace Sinjector;

public class NetIocContainer(IServiceProvider serviceProvider) : ISinjectorContainer
{
    public object Resolve(Type type) => 
        serviceProvider.GetService(type);

    public void Dispose() => 
        (serviceProvider as IDisposable)?.Dispose();
}