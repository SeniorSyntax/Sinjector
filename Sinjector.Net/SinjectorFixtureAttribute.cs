using System;

namespace Sinjector;

public class SinjectorFixtureAttribute : SinjectorFixtureBaseAttribute
{
    protected override ITheContainerBuilder CreateBuilder() => 
        throw new Exception();
}