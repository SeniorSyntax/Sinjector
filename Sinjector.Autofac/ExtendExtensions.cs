using System;
using Autofac;
using Autofac.Core;

namespace Sinjector;

public static class ExtendExtensions
{
    public static void AddModule(this IExtend extend, IModule module) => 
        extend.AddSpecific(x => x.RegisterModule(module));
    
    public static void AddSpecific(this IExtend extend, Action<ContainerBuilder> actionOnBuilder) => 
        extend.Add(actionOnBuilder);
}