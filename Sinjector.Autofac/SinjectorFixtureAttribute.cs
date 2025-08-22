using Autofac;

namespace Sinjector;

public class SinjectorFixtureAttribute : SinjectorFixtureBaseAttribute
{
    protected override ITheContainerBuilder CreateBuilder() => 
        new AutofacBuilder(new ContainerBuilder());
}