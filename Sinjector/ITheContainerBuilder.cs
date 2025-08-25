using System;
using Sinjector.Internals;

namespace Sinjector;

public interface ITheContainerBuilder
{
    void RegisterTestDoubleType(Type type, Type[] asTypes, ITestDoubles testDoubles);
    void RegisterTestDoubleInstance(object instance, Type[] asTypes);
    object AddService(Type type, bool instancePerLifeTimeScope);
    void AddService<TService>(TService instance) where TService : class;
    void Add(object thing);
    ITheContainer Build();
}