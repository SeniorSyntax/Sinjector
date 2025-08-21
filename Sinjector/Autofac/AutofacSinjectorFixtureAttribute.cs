using Autofac;
using Sinjector.Autofac;

namespace Sinjector;

public class SinjectorFixtureAttribute : SinjectorFixtureAttributeOLD
{
    protected override ITheContainerBuilder CreateBuilder() => 
        new AutofacBuilder(new ContainerBuilder());
}