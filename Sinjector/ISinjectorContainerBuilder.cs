using System;

namespace Sinjector;

public interface ISinjectorContainerBuilder
{
    void RegisterTestDoubleType(Type type, Type[] asTypes);
    void RegisterTestDoubleInstance(object instance, Type[] asTypes);
    void Add(object actionOnBuilder);
    ISinjectorContainer Build();
}