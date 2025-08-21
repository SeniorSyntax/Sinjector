using System;
using Autofac;
using NUnit.Framework;
using SharpTestsEx;

namespace Sinjector.Test;

[TestSystem]
public class RestartSimulationTest
{
	public TestService Service;
	public FakeService FakedService;
	public IFakeDisposeService FakedDisposeService;
	public ISinjectorTestContext Context;

	[Test]
	public void ShouldGetNewServiceInstanceAfterRestart()
	{
		var first = Service;
		Context.SimulateRestart();
		var second = Service;
		first.Should().Not.Be.SameInstanceAs(second);
	}

	[Test]
	public void ShouldGetSameFakeInstanceAfterRestart()
	{
		var first = FakedService;
		Context.SimulateRestart();
		var second = FakedService;
		first.Should().Be.SameInstanceAs(second);
	}
	
	[Test]
	public void ShouldNotCallDisposeOnFakeAtRestart()
	{
		FakedDisposeService.Value = 37;
		Context.SimulateRestart();
		FakedDisposeService.Value.Should().Be(37);
	}

	public class TestSystemAttribute : AutofacSinjectorFixtureAttribute, IContainerSetup, IIsolateSystem
	{
		public void ContainerSetup(IContainerSetupContext context)
		{
			context.AddModule(new TestSystemModule());
		}

		public void Isolate(IIsolate isolate)
		{
			isolate.UseTestDouble<FakeService>().For<IFakedService>();
			isolate.UseTestDouble<FakeDisposeService>().For<IFakeDisposeService>();
		}

		public class TestSystemModule : Module
		{
			protected override void Load(ContainerBuilder builder)
			{
				builder.RegisterType<TestService>().SingleInstance();
			}
		}
	}

	public class TestService;
	
	public interface IFakedService;

	public class FakeService : IFakedService;

	public interface IFakeDisposeService
	{
		int Value { get; set; }
	}
	public class FakeDisposeService : IFakeDisposeService, IDisposable
	{
		public int Value { get; set; }
		public void Dispose() => Value = 0;
	}
	
}
