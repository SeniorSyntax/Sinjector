using Microsoft.Extensions.DependencyInjection;

namespace Sinjector;

public class SinjectorFixtureAttribute : SinjectorFixtureBaseAttribute
{
    protected override ITheContainerBuilder CreateBuilder()
    {
        return new NetIocBuilder(new ServiceCollection());
    }
}