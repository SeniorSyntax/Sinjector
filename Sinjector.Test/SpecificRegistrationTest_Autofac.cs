using System;
using Autofac;
using NUnit.Framework;
using SharpTestsEx;

namespace Sinjector.Test;

[SinjectorFixture]
[Specific]
public class SpecificRegistrationTest
{
    public SpecificService Service;

    [Test]
    public void ShouldBeAbleToRegisterSpecifically()
    {
        Service.Should().Not.Be.Null();
    }
}

public class SpecificAttribute : Attribute, IContainerSetup
{
    public void ContainerSetup(IExtend context)
    {
        context.AddSpecific(x => x.RegisterInstance(new SpecificService()));
    }
}

public class SpecificService;