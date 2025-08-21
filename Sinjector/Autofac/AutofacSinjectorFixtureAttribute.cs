using Autofac;

namespace Sinjector.Autofac;

public class AutofacSinjectorFixtureAttribute : SinjectorFixtureAttribute
{
    protected override ITheContainerBuilder CreateBuilder() => 
        new AutofacBuilder(new ContainerBuilder());
}