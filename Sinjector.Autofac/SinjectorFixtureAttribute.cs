using Autofac;

namespace Sinjector;

public class SinjectorFixtureAttribute : SinjectorFixtureBaseAttribute
{
    protected override ISinjectorContainerBuilder CreateBuilder() => 
        new AutofacBuilder(new ContainerBuilder());
}