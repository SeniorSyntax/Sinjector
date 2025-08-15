using System;
using Autofac;

namespace Sinjector.Internals;

public class AutofacThingy(IComponentContext container) : ITheContainerThingy
{
    public object Resolve(Type type) => 
        container.Resolve(type);

    public T Resolve<T>() => 
        container.Resolve<T>();
}