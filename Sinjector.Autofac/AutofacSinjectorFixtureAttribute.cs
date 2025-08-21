using Autofac;

namespace Sinjector;

public class AutofacSinjectorFixtureAttribute : SinjectorFixtureAttribute
{
    protected override ITheContainerBuilder CreateBuilder() => 
        new AutofacBuilder(new ContainerBuilder());
}