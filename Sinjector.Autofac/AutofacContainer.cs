using System;
using Autofac;

namespace Sinjector;

public class AutofacContainer(IContainer container) : ISinjectorContainer
{
    public object Resolve(Type type) => 
        container.Resolve(type);

    public void Dispose() => 
        container.Dispose();
}