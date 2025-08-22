using Autofac.Core;

namespace Sinjector;

public static class ExtendExtensions
{
    public static void AddModule(this IExtend extend, IModule module) => 
        extend.Add(module);
}