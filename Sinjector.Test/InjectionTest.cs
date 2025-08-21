using System;
using System.Collections.Generic;
using Autofac;
using NUnit.Framework;
using SharpTestsEx;

namespace Sinjector.Test;

[TestSystem]
public class InjectionTest
{
	public IEnumerable<TestService> Injected;
	public IEnumerable<TestExtendedService> Extended;
	public IEnumerable<TestExtendedLevel2Service> ExtendedLevel2;

	[Test]
	public void ShouldInject()
	{
		Injected.Should().Have.Count.EqualTo(1);
	}

	[Test]
	public void ShouldExtendFromOtherAttribute()
	{
		Extended.Should().Have.Count.EqualTo(1);
	}

	[Test]
	public void ShouldExtendFromOthersAttributeAttribute()
	{
		ExtendedLevel2.Should().Have.Count.EqualTo(1);
	}

	[TestSystemExtended]
	public class TestSystemAttribute : AutofacSinjectorFixtureAttribute, IContainerSetup
	{
		public void ContainerSetup(IContainerSetupContext context)
		{
			context.AddModule(new TestSystemModule());
		}

		public class TestSystemModule : Module
		{
			protected override void Load(ContainerBuilder builder)
			{
				builder.RegisterType<TestService>().SingleInstance();
			}
		}
	}

	public class TestService
	{
	}

	[TestSystemExtendedLevel2]
	public class TestSystemExtendedAttribute : Attribute, IExtendSystem
	{
		public void Extend(IExtend extend)
		{
			extend.AddService<TestExtendedService>();
		}
	}

	public class TestExtendedService
	{
	}

	public class TestSystemExtendedLevel2Attribute : Attribute, IExtendSystem
	{
		public void Extend(IExtend extend)
		{
			extend.AddService<TestExtendedLevel2Service>();
		}
	}

	public class TestExtendedLevel2Service
	{
	}
}
