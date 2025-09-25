using System;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using Sinjector.Internals;

namespace Sinjector;

[AttributeUsage(AttributeTargets.Class)]
public abstract class SinjectorFixtureBaseAttribute : Attribute, ITestAction, ISinjectorTestContext
{
	public ActionTargets Targets => ActionTargets.Test;

	private Injector _injector;
	private ExtensionQuerier _extensions;
	private ISinjectorContainer _container;
	private TestDoubles _testDoubles;

	private readonly IList<ISinjectorContainer> _containersToDisposeAfterTestRun = new List<ISinjectorContainer>();

	public void BeforeTest(ITest testDetails)
	{
		_testDoubles = new TestDoubles();
		_extensions = new ExtensionQuerier()
			.Fixture(testDetails.Fixture)
			.Attribute(this)
			.TestMethod(testDetails.Method.MethodInfo);
		buildContainer(null);

		_injector = new Injector()
			.Target(testDetails.Fixture)
			.Target(this);
		_injector.InjectFrom(_container);

		InvokeExtensions<ITestSetup>(x => x.TestSetup());
	}

	private void buildContainer(ISinjectorContainer previousContainer)
	{
		InvokeExtensions<IContainerBuild>(a =>
		{
			_container = a.ContainerBuild(builder =>
			{
				register(builder, previousContainer);
			});
		});

		if (_container == null)
		{
			var builder = CreateBuilder();
			register(builder, previousContainer);
			_container = builder.Build();
		}
	}

	private void register(ISinjectorContainerBuilder builder, ISinjectorContainer previousContainer)
	{
		var context = new ContainerSetupContext(_testDoubles, builder, _extensions);
		context.AddService(this);

		InvokeExtensions<IContainerSetup>(x => x.ContainerSetup(context));
		InvokeExtensions<IExtendSystem>(x => x.Extend(context));
		InvokeExtensions<IIsolateSystem>(x => x.Isolate(context));

		_testDoubles.OverwriteRegistrationsFromPreviousContainer(builder, previousContainer);
	}

	public void AfterTest(ITest testDetails)
	{
		InvokeExtensions<ITestTeardown>(x => x.TestTeardown());
		disposeContainer();
		_containersToDisposeAfterTestRun.ForEach(x => x.Dispose());
		_container = null;
	}

	private void disposeContainer() => 
		_container?.Dispose();

	protected void InvokeExtensions<T>(Action<T> action) where T : class =>
		_extensions.Query<T>().ForEach(action);
		
	protected IEnumerable<T> QueryAllAttributes<T>() => _extensions.Query<T>();
	protected T Resolve<T>() => _container.Resolve<T>();

	protected abstract ISinjectorContainerBuilder CreateBuilder(); 

	public void SimulateShutdown() => 
		disposeContainer();
	
	public void SimulateRestart()
	{
		var previousContainer = _container;
		_container = null;

		_containersToDisposeAfterTestRun.Add(previousContainer);
		buildContainer(previousContainer);
		
		_injector?.InjectFrom(_container);
	}
}