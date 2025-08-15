using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Autofac;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using Sinjector.Internals;

namespace Sinjector;

[AttributeUsage(AttributeTargets.Class)]
public class SinjectorFixtureAttribute : Attribute, ITestAction, ISinjectorTestContext
{
	public ActionTargets Targets => ActionTargets.Test;

	private Injector _injector;
	private ExtensionQuerier _extensions;

	private class TestState
	{
		public object Fixture;
		public ITheContainerThingy Container;
		public ITestDoubles TestDoubles;
	}

	private static string WorkerId => TestContext.CurrentContext.WorkerId ?? "single";
	private readonly ConcurrentDictionary<string, TestState> _state = new();
	private TestState State => _state[WorkerId];

	public void BeforeTest(ITest testDetails)
	{
		_state[WorkerId] = new TestState();
		State.Fixture = testDetails.Fixture;
		State.TestDoubles = new TestDoubles();
		_extensions = new ExtensionQuerier()
			.Fixture(testDetails.Fixture)
			.Attribute(this)
			.TestMethod(testDetails.Method.MethodInfo);
		buildContainer();

		if (TestExecutionContext.CurrentContext.ParallelScope.HasFlag(ParallelScope.Children))
			Injector.Inject(State.Fixture, this);
		else
		{
			_injector = new Injector()
				.Source(State.Container)
				.Target(testDetails.Fixture)
				.Target(this);
			_injector.Inject();
		}

		InvokeExtensions<ITestSetup>(x => x.TestSetup());
	}

	private void buildContainer()
	{
		InvokeExtensions<IContainerBuild<ContainerBuilder>>(a =>
		{
			State.Container = a.ContainerBuild(builder =>
			{
				register(builder, () => new ContainerAdaptor(State.TestDoubles, builder, _extensions));
			});
		});

		if (State.Container == null)
		{
			var builder = new ContainerBuilder();
			register(builder, () => new ContainerAdaptor(State.TestDoubles, builder, _extensions));
			State.Container = new AutofacThingy(builder.Build());
		}
	}

	private void rebuildContainer()
	{
		State.Container = null;

		InvokeExtensions<IContainerBuild<ContainerBuilder>>(a =>
		{
			State.Container = a.ContainerBuild(builder =>
			{
				register(builder, () => new ContainerAdaptor(new IgnoreTestDoubles(), builder, _extensions));
				State.TestDoubles.RegisterFromPreviousContainer(builder);
			});
		});

		if (State.Container == null)
		{
			var builder = new ContainerBuilder();
			register(builder, () => new ContainerAdaptor(new IgnoreTestDoubles(), builder, _extensions));
			State.TestDoubles.RegisterFromPreviousContainer(builder);
			State.Container = new AutofacThingy(builder.Build());
		}

		_injector?.Source(State.Container);
	}

	private void register(ContainerBuilder builder, Func<ContainerAdaptor> adaptor)
	{
		builder.RegisterInstance(State.TestDoubles).ExternallyOwned();

		var containerAdaptor = adaptor.Invoke();

		var context = new ContainerSetupContext(containerAdaptor, _extensions);
		context.AddService(this);

		InvokeExtensions<IContainerSetup>(x => x.ContainerSetup(context));
		InvokeExtensions<IExtendSystem>(x => x.Extend(context));
		InvokeExtensions<IIsolateSystem>(x => x.Isolate(context));
	}

	public void AfterTest(ITest testDetails)
	{
		InvokeExtensions<ITestTeardown>(x => x.TestTeardown());
		disposeContainer();
		(State.TestDoubles as IDisposable)?.Dispose();
		_injector = null;
		_state.TryRemove(WorkerId, out _);
	}

	private void disposeContainer() => 
		State.Container?.Dispose();

	protected void InvokeExtensions<T>(Action<T> action) where T : class =>
		_extensions.InvokeExtensions(action);
		
	protected IEnumerable<T> QueryAllAttributes<T>() => _extensions.Query<T>();
	protected T Resolve<T>() => State.Container.Resolve<T>();

	public void SimulateShutdown() => 
		disposeContainer();

	public void SimulateRestart()
	{
		disposeContainer();
		rebuildContainer();
		_injector?.Inject();
	}
}