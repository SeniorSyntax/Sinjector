using System;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SharpTestsEx;

namespace Sinjector.Test;

[TestSystem]
public class InstancePerScopeTest
{
    public IServiceProvider Container;
    public ThingWithUniqueScope_Instance InstanceThing;
    public ThingWithUniqueScope_Type TypeThing;
    
    [Test]
    public void ShouldCareAboutInstancePerScopeFlag_Instance()
    {
        using var scope = Container.CreateScope();
        var newThing = scope.ServiceProvider.GetRequiredService<ThingWithUniqueScope_Instance>();
        InstanceThing.Should().Not.Be.SameInstanceAs(newThing);
    }
    
    [Test]
    public void ShouldCareAboutInstancePerScopeFlag_Type()
    {
        using var scope = Container.CreateScope();
        var newThing = scope.ServiceProvider.GetRequiredService<ThingWithUniqueScope_Type>();
        TypeThing.Should().Not.Be.SameInstanceAs(newThing);
    }
    
    public class TestSystemAttribute : SinjectorFixtureAttribute, IExtendSystem
    {
        public void Extend(IExtend extend)
        {
            extend.AddService<ThingWithUniqueScope_Instance>(true);
            extend.AddService(typeof(ThingWithUniqueScope_Type), true);
        }
    }
    
    public class ThingWithUniqueScope_Instance;
    public class ThingWithUniqueScope_Type;
}