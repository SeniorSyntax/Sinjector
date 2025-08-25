using System;

namespace Sinjector.Internals;


//todo: back to internal?
public interface ITestDoubles
{
    void Register(ITheContainerBuilder builder, object instance, Type type, Type[] asTypes);
    void RegisterFromPreviousContainer(ITheContainerBuilder builder);
    void KeepInstances(ITheContainer theContainer);
}