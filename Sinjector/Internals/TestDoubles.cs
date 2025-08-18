using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;

namespace Sinjector.Internals;

internal class TestDoubles : ITestDoubles, IDisposable
{
	private readonly List<testDouble> _items = new List<testDouble>();

	private class testDouble
	{
		public object instance;
		public Type type;
		public Type[] asTypes;
	}

	public void Register(ITheContainerBuilder builder, object instance, Type type, Type[] asTypes)
	{
		var testDouble = new testDouble
		{
			instance = instance,
			type = type,
			asTypes = asTypes
		};
		_items.Add(testDouble);
		register(builder, testDouble);
	}

	private static void register(ITheContainerBuilder builder, testDouble testDouble)
	{
		var instance = testDouble.instance;
		var type = testDouble.type;
		var asTypes = testDouble.asTypes;

		if (instance != null)
			registerInstance(builder, instance, asTypes);
		else
			registerType(builder, type, asTypes);
	}

	public void KeepInstance(object instance, Type type)
	{
		_items
			.Where(x => x.type == type)
			.ForEach(x => { x.instance = instance; });
	}

	public void RegisterFromPreviousContainer(ITheContainerBuilder builder)
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

	private static void registerType(ITheContainerBuilder builder, Type type, Type[] asTypes)
	{
		builder.ContainerBuilder
			.RegisterType(type)
			.SingleInstance()
			.AsSelf()
			.As(asTypes)
			.ExternallyOwned()
			.PropertiesAutowired()
			.OnActivated(c =>
			{
				c.Context.Resolve<ITestDoubles>()
					.KeepInstance(c.Instance, type);
			});
	}

	private static void registerInstance(ITheContainerBuilder builder, object instance, Type[] asTypes)
	{
		builder.ContainerBuilder
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