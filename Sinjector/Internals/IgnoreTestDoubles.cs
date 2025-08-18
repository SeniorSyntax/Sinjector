using System;
using Autofac;

namespace Sinjector.Internals;

internal class IgnoreTestDoubles : ITestDoubles
{
    public void Register(ITheContainerBuilder builder, object instance, Type type, Type[] asTypes)
    {
    }

    public void RegisterFromPreviousContainer(ITheContainerBuilder builder)
    {
    }

    public void KeepInstance(object instance, Type type)
    {
    }
}