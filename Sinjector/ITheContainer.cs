using System;
using Autofac;

namespace Sinjector;

public interface ITheContainerBuilder
{
    //should be removed
    ContainerBuilder ContainerBuilder { get; }

    void RegisterTestDoubleType(Type type, Type[] asTypes);
    void RegisterTestDoubleInstance(object instance, Type[] asTypes);
}

public interface ITheContainer : IDisposable
{
    object Resolve(Type type);
    T Resolve<T>();
}