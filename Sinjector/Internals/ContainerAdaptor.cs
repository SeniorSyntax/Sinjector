using System;
using Autofac;

namespace Sinjector.Internals
{
	internal class ContainerAdaptor
	{
		private readonly ITestDoubles _testDoubles;
		private readonly ContainerBuilder _builder;
		private readonly ExtensionQuerier _extensions;

		public ContainerAdaptor(ITestDoubles testDoubles, ContainerBuilder builder, ExtensionQuerier extensions)
		{
			_testDoubles = testDoubles;
			_builder = builder;
			_extensions = extensions;
		}

		public ITestDoubleFor UseTestDouble<TTestDouble>() where TTestDouble : class =>
			new testDoubleFor<TTestDouble>(_testDoubles, _builder, null);

		public ITestDoubleFor UseTestDouble<TTestDouble>(TTestDouble instance) where TTestDouble : class =>
			new testDoubleFor<TTestDouble>(_testDoubles, _builder, instance);

		public ITestDoubleFor UseTestDoubleForType(Type type) =>
			new testDoubleFor<object>(_testDoubles, _builder, type);

		public void AddService<TService>(bool instancePerLifeTimeScope)
		{
			if (instancePerLifeTimeScope)
			{
				var registration = _builder
					.RegisterType<TService>()
					.AsSelf()
					.AsImplementedInterfaces()
					.InstancePerLifetimeScope();
				_extensions.InvokeExtensions<IContainerRegistrationSetup>(x => x.ContainerRegistrationSetup(registration));
			}
			else
			{
				var registration = _builder
					.RegisterType<TService>()
					.AsSelf()
					.AsImplementedInterfaces()
					.SingleInstance();
				_extensions.InvokeExtensions<IContainerRegistrationSetup>(x => x.ContainerRegistrationSetup(registration));
			}
		}

		public void AddService<TService>(TService instance) where TService : class
		{
			_builder
				.RegisterInstance(instance)
				.AsSelf()
				.AsImplementedInterfaces()
				.SingleInstance();
		}
		
		public void AddService(Type type, bool instancePerLifeTimeScope)
		{
			if (instancePerLifeTimeScope)
			{
				var registration = _builder
					.RegisterType(type)
					.AsSelf()
					.AsImplementedInterfaces()
					.InstancePerLifetimeScope();
				_extensions.InvokeExtensions<IContainerRegistrationSetup>(x => x.ContainerRegistrationSetup(registration));
			}
			else
			{
				var registration = _builder
					.RegisterType(type)
					.AsSelf()
					.AsImplementedInterfaces()
					.SingleInstance();
				_extensions.InvokeExtensions<IContainerRegistrationSetup>(x => x.ContainerRegistrationSetup(registration));
			}
		}

		public void AddModule(Module module) =>
			_builder.RegisterModule(module);

		private class testDoubleFor<TTestDouble> : ITestDoubleFor where TTestDouble : class
		{
			private readonly ITestDoubles _testDoubles;
			private readonly ContainerBuilder _builder;
			private readonly Type _type;
			private readonly object _instance;

			public testDoubleFor(ITestDoubles testDoubles, ContainerBuilder builder, object instance)
			{
				_testDoubles = testDoubles;
				_builder = builder;
				_instance = instance;
			}

			public testDoubleFor(ITestDoubles testDoubles, ContainerBuilder builder, Type type)
			{
				_testDoubles = testDoubles;
				_builder = builder;
				_type = type;
			}

			public void For<T>() => register(typeof(T));
			public void For<T1, T2>() => register(typeof(T1), typeof(T2));
			public void For<T1, T2, T3>() => register(typeof(T1), typeof(T2), typeof(T3));
			public void For<T1, T2, T3, T4>() => register(typeof(T1), typeof(T2), typeof(T3), typeof(T4));
			public void For<T1, T2, T3, T4, T5>() => register(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5));
			public void For<T1, T2, T3, T4, T5, T6>() => register(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6));
			public void For<T1, T2, T3, T4, T5, T6, T7>() => register(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7));
			public void For(Type type) => register(type);

			private void register(params Type[] asTypes)
			{
				_testDoubles.Register<TTestDouble>(_builder, _instance, _type, asTypes);
			}
		}
	}
}
