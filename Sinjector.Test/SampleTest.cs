using System;
using NUnit.Framework;

namespace Sinjector.Test;

[SinjectorFixture]
[MySystem]
public class SampleTest
{
    public MyService Target;

    [Test]
    public void ShouldDoStuff()
    {
        Target.DoStuff();
    }
}

public class MySystemAttribute : Attribute, IContainerSetup
{
    public void ContainerSetup(IContainerSetupContext context)
    {
        context.AddService<MyService>();
    }
}

public class MyService
{
    public void DoStuff()
    {
    }
}
