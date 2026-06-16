using System;

namespace Sinjector.Internals;

internal static class SinjectorContainerBuilderExtensions
{
    extension(ISinjectorContainerBuilder sinjectorContainerBuilder)
    {
        internal void AddService(Type type) => 
            sinjectorContainerBuilder.RegisterTestDoubleType(type, type.GetInterfaces());

        internal void AddService<TService>(TService instance) where TService : class => 
            sinjectorContainerBuilder.RegisterTestDoubleInstance(instance, instance.GetType().GetInterfaces());
    }
}