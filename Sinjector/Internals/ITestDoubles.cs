using System;

namespace Sinjector.Internals;

internal interface ITestDoubles
{
    void Register(ISinjectorContainerBuilder builder, object instance, Type type, Type[] asTypes);
    void RegisterFromPreviousContainer(ISinjectorContainerBuilder builder);
    void SetInstances(ISinjectorContainer sinjectorContainer);
}