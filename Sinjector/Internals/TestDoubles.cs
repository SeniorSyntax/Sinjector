using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;

namespace Sinjector.Internals
{
	internal class TestDoubles : IDisposable
	{
		private readonly List<testDouble> _items = new List<testDouble>();

		private class testDouble
		{
			public object instance;
			public Type type;
			public Type[] asTypes;
		}

		public void Register<TTestDouble>(ContainerBuilder builder, object instance, Type type, Type[] asTypes) where TTestDouble : class
		{
			var testDouble = new testDouble
			{
				instance = instance,
				type = type ?? typeof(TTestDouble),
				asTypes = asTypes
			};
			_items.Add(testDouble);
			register<TTestDouble>(builder, testDouble);
		}

		private static void register<TTestDouble>(ContainerBuilder builder, testDouble testDouble)
		{
			var instance = testDouble.instance;
			var type = testDouble.type;
			var asTypes = testDouble.asTypes;

			if (instance != null)
				registerInstance(builder, instance, asTypes);
			else
				registerType(builder, type ?? typeof(TTestDouble), asTypes);
		}

		private void keepInstance(object instance, Type type)
		{
			_items
				.Where(x => x.type == type)
				.ForEach(x => { x.instance = instance; });
		}

		public void RegisterFromPreviousContainer(ContainerBuilder builder)
		{
			_items.ForEach(x =>
			{
				if (x.instance == null)
				{
					registerType(builder, x.type, x.asTypes);
				}
				else
				{
					registerInstance(builder, x.instance, x.asTypes);
				}
			});
		}

		private static void registerType(ContainerBuilder builder, Type type, Type[] asTypes)
		{
			builder
				.RegisterType(type)
				.SingleInstance()
				.AsSelf()
				.As(asTypes)
				.ExternallyOwned()
				.PropertiesAutowired()
				.OnActivated(c =>
				{
					c.Context.Resolve<TestDoubles>()
						.keepInstance(c.Instance, type);
				});
		}

		private static void registerInstance(ContainerBuilder builder, object instance, Type[] asTypes)
		{
			builder
				.RegisterInstance(instance)
				.AsSelf()
				.As(asTypes)
				.ExternallyOwned()
				.PropertiesAutowired();
		}

		public void Dispose()
		{
			_items.ForEach(x =>
			{
				var disposable = x.instance as IDisposable;
				disposable?.Dispose();
			});
		}
	}
}
