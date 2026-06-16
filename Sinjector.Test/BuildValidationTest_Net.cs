using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SharpTestsEx;

namespace Sinjector.Test;

[SinjectorFixtureWithBuildOverride]
public class BuildValidationTest
{
    public SinjectorFixtureWithBuildOverrideAttribute Context;
    
    [Test]
    public void CanOverrideBuildOfContainer()
    {
        Context.Builder().ICanOverrideBuild
            .Should().Be.True();   
    }
}

public class SinjectorFixtureWithBuildOverrideAttribute : SinjectorFixtureBaseAttribute
{
    private readonly NetIocBuilderWithBuildOverride _builder = new(new ServiceCollection());
    public NetIocBuilderWithBuildOverride Builder() => _builder;
    
    
    protected override ISinjectorContainerBuilder CreateBuilder() => _builder;
}

public class NetIocBuilderWithBuildOverride(IServiceCollection serviceCollection) : 
    NetIocBuilder(serviceCollection)
{
    public bool ICanOverrideBuild;
    public override ISinjectorContainer Build()
    {
        ICanOverrideBuild = true;
        return base.Build();
    }
}