using NUnit.Framework;
using SharpTestsEx;

namespace Sinjector.Test;

[TestSystem]
public class TestDoublesTest
{
	public FakeGenericTypedService FakeService;
	public IGenericTypedService Service;
	public FakeInstancedService FakeInstanced;
	public IInstancedService Instanced;
	public FakeTypedService FakeTyped;
	public ITypedService Typed;
	public FakeDoubleFakedService FakeDoubleFaked;
	public TestSystemAttribute Context;

	[Test]
	public void ShouldInjectFake()
	{
		Service.Should().Not.Be.Null();
		Service.Should().Be.OfType<FakeGenericTypedService>();
		FakeService.Should().Be.OfType<FakeGenericTypedService>();
		Service.Should().Be.SameInstanceAs(FakeService);
	}

	[Test]
	public void ShouldInjectInstancedFake()
	{
		Instanced.Should().Not.Be.Null();
		Instanced.Should().Be.SameInstanceAs(FakeInstanced);
	}

	[Test]
	public void ShouldInjectTypedFake()
	{
		Typed.Should().Not.Be.Null();
		Typed.Should().Be.SameInstanceAs(FakeTyped);
	}

	[Test]
	public void ShouldGetSameFakeInstanceAfterRestart()
	{
		var first = FakeService;
		Context.SimulateRestart();
		var second = FakeService;
		first.Should().Be.SameInstanceAs(second);
	}

	[Test]
	public void ShouldGetSameFakeInstanceAfter2Restarts()
	{
		var first = FakeService;
		Context.SimulateRestart();
		Context.SimulateRestart();
		var second = FakeService;
		first.Should().Be.SameInstanceAs(second);
	}

	[Test]
	public void ShouldResolveNonActivatedAfterRestart()
	{
		Context.SimulateRestart();
		var first = Context.Resolve<FakeNonActivatedService>();
		Context.SimulateRestart();
		var second = Context.Resolve<FakeNonActivatedService>();
		first.Should().Be.SameInstanceAs(second);
	}

	[Test]
	public void ShouldInjectDuplicateFaked()
	{
		FakeDoubleFaked.Should().Not.Be.Null();
	}

	public class TestSystemAttribute : SinjectorFixtureAttribute, IContainerSetup, IIsolateSystem
	{
		public void ContainerSetup(IContainerSetupContext context)
		{
			context.AddService<RealGenericTypedService>();
			context.AddService<RealInstancedService>();
			context.AddService<RealTypedService>();
			context.AddService<RealNonActivatedService>();
			context.AddService<RealDoubleFakedService>();
		}

		public void Isolate(IIsolate isolate)
		{
			isolate.UseTestDouble<FakeGenericTypedService>().For<IGenericTypedService>();
			isolate.UseTestDouble(new FakeInstancedService()).For<IInstancedService>();
			isolate.UseTestDoubleForType(typeof(FakeTypedService)).For<ITypedService>();
			isolate.UseTestDouble<FakeNonActivatedService>().For<INonActivatedService>();
			isolate.UseTestDouble<FakeDoubleFakedService>().For<IDoubleFakedService>();
			isolate.UseTestDouble<FakeDoubleFakedService>().For<IDoubleFakedService>();
		}

		public new T Resolve<T>() => base.Resolve<T>();
	}

	public interface IGenericTypedService;

	private class RealGenericTypedService : IGenericTypedService;

	public class FakeGenericTypedService : IGenericTypedService;

	public interface IInstancedService;

	public class RealInstancedService : IInstancedService;

	public class FakeInstancedService : IInstancedService;

	public interface ITypedService;

	public class RealTypedService : ITypedService;

	public class FakeTypedService : ITypedService;

	public interface INonActivatedService;

	public class RealNonActivatedService : INonActivatedService;

	public class FakeNonActivatedService : INonActivatedService;

	public interface IDoubleFakedService;

	public class RealDoubleFakedService : IDoubleFakedService;

	public class FakeDoubleFakedService : IDoubleFakedService;
}
