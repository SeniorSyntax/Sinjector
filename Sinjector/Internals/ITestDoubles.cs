using System;
using Autofac;

namespace Sinjector.Internals
{
    internal interface ITestDoubles
    {
        void Register<TTestDouble>(ContainerBuilder builder, object instance, Type type, Type[] asTypes)
            where TTestDouble : class;

        void RegisterFromPreviousContainer(ContainerBuilder builder);
    }
}