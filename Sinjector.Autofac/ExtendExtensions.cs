using System;
using Autofac;
using Autofac.Core;

namespace Sinjector;

public static class ExtendExtensions
{
    extension(IExtend extend)
    {
        public void AddModule(IModule module) => 
            extend.AddSpecific(x => x.RegisterModule(module));

        public void AddSpecific(Action<ContainerBuilder> actionOnBuilder) => 
            extend.Add(actionOnBuilder);
    }
}