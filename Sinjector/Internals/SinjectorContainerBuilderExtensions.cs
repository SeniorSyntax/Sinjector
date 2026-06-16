using System;

namespace Sinjector.Internals;

internal static class SinjectorContainerBuilderExtensions
{
    internal static void AddService(this ISinjectorContainerBuilder sinjectorContainerBuilder, Type type) => 
        sinjectorContainerBuilder.RegisterTestDoubleType(type, type.GetInterfaces());

    internal static void AddService<TService>(this ISinjectorContainerBuilder sinjectorContainerBuilder, TService instance) where TService : class => 
        sinjectorContainerBuilder.RegisterTestDoubleInstance(instance, instance.GetType().GetInterfaces());
}