using System;

namespace Sinjector.Internals;

internal class IgnoreTestDoubles : ITestDoubles
{
    public void Register(ISinjectorContainerBuilder builder, object instance, Type type, Type[] asTypes)
    {
    }

    public void RegisterFromPreviousContainer(ISinjectorContainer sinjectorContainer, ISinjectorContainerBuilder builder)
    {
    }
}