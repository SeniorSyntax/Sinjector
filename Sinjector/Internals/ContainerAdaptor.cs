using System;
using Autofac;

namespace Sinjector.Internals
{
	internal class ContainerAdaptor(ITestDoubles testDoubles, ContainerBuilder builder, ExtensionQuerier extensions)
	{
		public ITestDoubleFor UseTestDouble(object instance) =>
			new testDoubleFor(testDoubles, builder, null, instance);

		public ITestDoubleFor UseTestDoubleForType(Type type) =>
			new testDoubleFor(testDoubles, builder, type, null);

		public void AddService<TService>(bool instancePerLifeTimeScope)
		{
			if (instancePerLifeTimeScope)
			{
				var registration = builder
					.RegisterType<TService>()
					.AsSelf()
					.AsImplementedInterfaces()
					.InstancePerLifetimeScope();
				extensions.InvokeExtensions<IContainerRegistrationSetup>(x => x.ContainerRegistrationSetup(registration));
			}
			else
			{
				var registration = builder
					.RegisterType<TService>()
					.AsSelf()
					.AsImplementedInterfaces()
					.SingleInstance();
				extensions.InvokeExtensions<IContainerRegistrationSetup>(x => x.ContainerRegistrationSetup(registration));
			}
		}

		public void AddService<TService>(TService instance) where TService : class
		{
			builder
				.RegisterInstance(instance)
				.AsSelf()
				.AsImplementedInterfaces()
				.SingleInstance();
		}
		
		public void AddService(Type type, bool instancePerLifeTimeScope)
		{
			if (instancePerLifeTimeScope)
			{
				var registration = builder
					.RegisterType(type)
					.AsSelf()
					.AsImplementedInterfaces()
					.InstancePerLifetimeScope();
				extensions.InvokeExtensions<IContainerRegistrationSetup>(x => x.ContainerRegistrationSetup(registration));
			}
			else
			{
				var registration = builder
					.RegisterType(type)
					.AsSelf()
					.AsImplementedInterfaces()
					.SingleInstance();
				extensions.InvokeExtensions<IContainerRegistrationSetup>(x => x.ContainerRegistrationSetup(registration));
			}
		}

		public void AddModule(Module module) =>
			builder.RegisterModule(module);

		private class testDoubleFor(ITestDoubles testDoubles, ContainerBuilder builder, Type type, object instance)
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
}
