using System;
using Autofac;

namespace Sinjector.Internals
{
    internal class IgnoreTestDoubles : ITestDoubles
    {
        public void Register<TTestDouble>(ContainerBuilder builder, object instance, Type type, Type[] asTypes) 
            where TTestDouble : class
        {
        }

        public void RegisterFromPreviousContainer(ContainerBuilder builder)
        {
        }
    }
}