using System;

namespace Sinjector;

public interface ITheContainerBuilder
{
    void RegisterTestDoubleType(Type type, Type[] asTypes);
    void RegisterTestDoubleInstance(object instance, Type[] asTypes);
    object AddService(Type type, bool instancePerLifeTimeScope);
    void AddService<TService>(TService instance) where TService : class;
    void Add(object actionOnBuilder);
    ITheContainer Build();
}