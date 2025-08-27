using System;

namespace Sinjector.Internals;

internal interface ITestDoubles
{
    void Register(ISinjectorContainerBuilder builder, object instance, Type type, Type[] asTypes);
    void RegisterFromPreviousContainer(ISinjectorContainer previousContainer, ISinjectorContainerBuilder builder);
}