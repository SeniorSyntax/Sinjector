using Autofac;
using NUnit.Framework;
using SharpTestsEx;

namespace Sinjector.Test;

[TestFixture]
[TestSystem]
public class RestartSimulationTest
{
	public TestService Service;
	public FakeService FakedService;
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

	public class TestSystemAttribute : SinjectorFixtureAttribute, IContainerSetup, IIsolateSystem, IExtendSystem
	{
		public void ContainerSetup(IContainerSetupContext context)
		{
			context.AddModule(new TestSystemModule());
		}

		public void Isolate(IIsolate isolate)
		{
			isolate.UseTestDouble<FakeService>().For<IFakedService>();
		}

		public void Extend(IExtend extend)
		{
			extend.AddService<TestExtensionService>();
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

	public class TestExtensionService
	{
	}
	
	public interface IFakedService
	{
	}

	public class FakeService : IFakedService
	{
	}

}
