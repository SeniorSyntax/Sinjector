using System;

namespace Sinjector;

public interface ITheContainerBuilder
{
    void RegisterTestDoubleType(Type type, Type[] asTypes);
    void RegisterTestDoubleInstance(object instance, Type[] asTypes);
    object AddService<TService>(bool instancePerLifeTimeScope);
    object AddService(Type type, bool instancePerLifeTimeScope);
    void AddService<TService>(TService instance) where TService : class;
    //how to handle this?
    void AddModule(object module);
}

public interface ITheContainer : IDisposable
{
    object Resolve(Type type);
    T Resolve<T>();
}