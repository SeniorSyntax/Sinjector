using System;
using System.Collections.Generic;

namespace Sinjector.Internals;

internal class ContainerSetupContext : IContainerSetupContext, IIsolate
{
	private readonly ITestDoubles _testDoubles;
	private readonly ISinjectorContainerBuilder _builder;
	private readonly ExtensionQuerier _extensionQuerier;

	internal ContainerSetupContext(ITestDoubles testDoubles, ISinjectorContainerBuilder builder, ExtensionQuerier extensionQuerier)
	{
		_testDoubles = testDoubles;
		_builder = builder;
		_extensionQuerier = extensionQuerier;
	}

	public object State { get; set; }

	public void AddService<TService>(TService instance) where TService : class => 
		_builder.AddService(instance);
	
	public void AddService<TService>() where TService : class => 
		AddService(typeof(TService));

	public void AddService(Type type)
	{
		var registration = _builder.AddService(type);
		_extensionQuerier.InvokeExtensions<IContainerRegistrationSetup>(x => x.RegistrationCallback(registration));
	}

	public void Add(object module) =>
		_builder.Add(module);

	public ITestDoubleFor UseTestDouble<TTestDouble>() where TTestDouble : class =>
		UseTestDoubleForType(typeof(TTestDouble));

	public ITestDoubleFor UseTestDouble<TTestDouble>(TTestDouble instance) where TTestDouble : class =>
		new testDoubleFor(_testDoubles, _builder, null, instance);

	public ITestDoubleFor UseTestDoubleForType(Type type) =>
		new testDoubleFor(_testDoubles, _builder, type, null);

	public IEnumerable<T> QueryAllAttributes<T>() =>
		_extensionQuerier.Query<T>();
		
	private class testDoubleFor(ITestDoubles testDoubles, ISinjectorContainerBuilder builder, Type type, object instance)
		: ITestDoubleFor
	{
		public void For<T>() => register(typeof(T));
		public void For<T1, T2>() => register(typeof(T1), typeof(T2));
		public void For<T1, T2, T3>() => register(typeof(T1), typeof(T2), typeof(T3));
		public void For<T1, T2, T3, T4>() => register(typeof(T1), typeof(T2), typeof(T3), typeof(T4));
		public void For<T1, T2, T3, T4, T5>() => register(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5));
		public void For<T1, T2, T3, T4, T5, T6>() => register(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6));
		public void For<T1, T2, T3, T4, T5, T6, T7>() => register(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7));
		public void For(Type type) => register(type);

		private void register(params Type[] asTypes) => 
			testDoubles.Register(builder, instance, type, asTypes);
	}
}