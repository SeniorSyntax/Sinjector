using System;
using Autofac;

namespace Sinjector.Internals;

internal interface ITestDoubles
{
    void Register(ITheContainerBuilder builder, object instance, Type type, Type[] asTypes);
    void RegisterFromPreviousContainer(ITheContainerBuilder builder);
    void KeepInstance(object instance, Type type);
}