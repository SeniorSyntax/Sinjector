using System;
using Autofac;

namespace Sinjector;

public interface ITheContainerBuilder
{
    //should be removed
    ContainerBuilder ContainerBuilder { get; }
    
    ITheContainer Build();
}

public interface ITheContainer : IDisposable
{
    object Resolve(Type type);
    T Resolve<T>();
}