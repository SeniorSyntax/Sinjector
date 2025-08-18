using System;
using Autofac;

namespace Sinjector.Internals;

internal class IgnoreTestDoubles : ITestDoubles
{
    public void Register(ContainerBuilder builder, object instance, Type type, Type[] asTypes)
    {
    }

    public void RegisterFromPreviousContainer(ContainerBuilder builder)
    {
    }

    public void KeepInstance(object instance, Type type)
    {
    }
}