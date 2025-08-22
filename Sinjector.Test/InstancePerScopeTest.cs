using Autofac;
using NUnit.Framework;
using SharpTestsEx;

namespace Sinjector.Test;

[TestSystem]
public class InstancePerScopeTest
{
    public ILifetimeScope Container;
    public ThingWithUniqueScope Thing;
    
    [Test]
    public void ShouldCareAboutInstancePerScopeFlag()
    {
        using var scope = Container.BeginLifetimeScope();
        var newThing = scope.Resolve<ThingWithUniqueScope>();
        Thing.Should().Not.Be.SameInstanceAs(newThing);
    }
    
    
    public class TestSystemAttribute : SinjectorFixtureAttribute, IExtendSystem
    {
        public void Extend(IExtend extend)
        {
            extend.AddService<ThingWithUniqueScope>(true);
        }
    }
    
    public class ThingWithUniqueScope{}
}