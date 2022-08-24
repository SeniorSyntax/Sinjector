using System;
using Autofac;
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
        context.AddModule(new MySystemModule());
    }
}


public class MySystemModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<MyService>();
    }
}

public class MyService
{
    public void DoStuff()
    {
    }
}
