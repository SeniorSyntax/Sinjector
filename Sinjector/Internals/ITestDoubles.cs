using System;

namespace Sinjector.Internals;

internal interface ITestDoubles
{
    void Register(ITheContainerBuilder builder, object instance, Type type, Type[] asTypes);
    void RegisterFromPreviousContainer(ITheContainerBuilder builder);
    void KeepInstances(ITheContainer theContainer);
}