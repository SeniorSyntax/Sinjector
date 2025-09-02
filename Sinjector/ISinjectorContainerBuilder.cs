using System;

namespace Sinjector;

public interface ISinjectorContainerBuilder
{
    void RegisterTestDoubleType(Type type, Type[] asTypes);
    void RegisterTestDoubleInstance(object instance, Type[] asTypes);
    void AddService(Type type);
    void AddService<TService>(TService instance) where TService : class;
    void Add(object actionOnBuilder);
    ISinjectorContainer Build();
}