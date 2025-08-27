using System;

namespace Sinjector.Internals;

internal class IgnoreTestDoubles : ITestDoubles
{
    public void Register(ISinjectorContainerBuilder builder, object instance, Type type, Type[] asTypes)
    {
    }

    public void RegisterFromPreviousContainer(ISinjectorContainerBuilder builder)
    {
    }

    public void SetInstances(ISinjectorContainer sinjectorContainer)
    {
        
    }
}