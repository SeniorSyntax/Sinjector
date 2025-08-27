using Microsoft.Extensions.DependencyInjection;

namespace Sinjector;

public class SinjectorFixtureAttribute : SinjectorFixtureBaseAttribute
{
    protected override ISinjectorContainerBuilder CreateBuilder()
    {
        return new NetIocBuilder(new ServiceCollection());
    }
}