using System;
using Autofac;

namespace Sinjector.Internals
{
    internal interface ITestDoubles
    {
        void Register(ContainerBuilder builder, object instance, Type type, Type[] asTypes);
        void RegisterFromPreviousContainer(ContainerBuilder builder);
        void KeepInstance(object instance, Type type);
    }
}