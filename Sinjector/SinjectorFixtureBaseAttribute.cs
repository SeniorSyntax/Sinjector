using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using Sinjector.Internals;

namespace Sinjector;

[AttributeUsage(AttributeTargets.Class)]
public abstract class SinjectorFixtureBaseAttribute : Attribute, ITestAction, ISinjectorTestContext
{
	public ActionTargets Targets => ActionTargets.Test;

	private Injector _injector;
	private ExtensionQuerier _extensions;

	private class TestState
	{
		public ISinjectorContainer Container;
		public TestDoubles TestDoubles;
	}

	private readonly IList<ISinjectorContainer> _containersToDisposeAfterTestRun = new List<ISinjectorContainer>();
	private static string WorkerId => TestContext.CurrentContext.WorkerId ?? "single";
	private readonly ConcurrentDictionary<string, TestState> _state = new();
	private TestState State => _state[WorkerId];

	public void BeforeTest(ITest testDetails)
	{
		_state[WorkerId] = new TestState();
		State.TestDoubles = new TestDoubles();
		_extensions = new ExtensionQuerier()
			.Fixture(testDetails.Fixture)
			.Attribute(this)
			.TestMethod(testDetails.Method.MethodInfo);
		buildContainer(null);

		if (TestExecutionContext.CurrentContext.ParallelScope.HasFlag(ParallelScope.Children))
			Injector.Inject(testDetails.Fixture, this);
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

	private void buildContainer(ISinjectorContainer previousContainer)
	{
		InvokeExtensions<IContainerBuild>(a =>
		{
			State.Container = a.ContainerBuild(builder =>
			{
				register(builder, previousContainer);
			});
		});

		if (State.Container == null)
		{
			var builder = CreateBuilder();
			register(builder, previousContainer);
			State.Container = builder.Build();
		}
	}
	

	private void register(ISinjectorContainerBuilder builder, ISinjectorContainer previousContainer)
	{
		var context = new ContainerSetupContext(State.TestDoubles, builder, _extensions);
		context.AddService(this);

		InvokeExtensions<IContainerSetup>(x => x.ContainerSetup(context));
		InvokeExtensions<IExtendSystem>(x => x.Extend(context));
		InvokeExtensions<IIsolateSystem>(x => x.Isolate(context));

		State.TestDoubles.OverwriteRegistrationsFromPreviousContainer(builder, previousContainer);
	}

	public void AfterTest(ITest testDetails)
	{
		InvokeExtensions<ITestTeardown>(x => x.TestTeardown());
		disposeContainer();
		_containersToDisposeAfterTestRun.ForEach(x => x.Dispose());
		_injector = null;
		_state.TryRemove(WorkerId, out _);
	}

	private void disposeContainer() => 
		State.Container?.Dispose();

	protected void InvokeExtensions<T>(Action<T> action) where T : class =>
		_extensions.InvokeExtensions(action);
		
	protected IEnumerable<T> QueryAllAttributes<T>() => _extensions.Query<T>();
	protected T Resolve<T>() => State.Container.Resolve<T>();

	protected abstract ISinjectorContainerBuilder CreateBuilder(); 

	public void SimulateShutdown() => 
		disposeContainer();
	
	public void SimulateRestart()
	{
		var previousContainer = State.Container;
		State.Container = null;

		_containersToDisposeAfterTestRun.Add(previousContainer);
		buildContainer(previousContainer);

		_injector?.Source(State.Container);
		_injector?.Inject();
	}
}