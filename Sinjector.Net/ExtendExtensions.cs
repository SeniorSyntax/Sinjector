using System;
using Microsoft.Extensions.DependencyInjection;

namespace Sinjector;

public static class ExtendExtensions
{
    public static void AddSpecific(this IExtend extend, Action<IServiceCollection> actionOnBuilder) => 
        extend.Add(actionOnBuilder);
}