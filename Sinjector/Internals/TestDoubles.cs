using System;
using System.Collections.Generic;
using System.Linq;

namespace Sinjector.Internals;

internal class TestDoubles : ITestDoubles, IDisposable
{
	private readonly List<testDouble> _items = [];

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

	private void register(ITheContainerBuilder builder, testDouble testDouble)
	{
		var instance = testDouble.instance;
		var type = testDouble.type;
		var asTypes = testDouble.asTypes;

		if (instance != null)
			builder.RegisterTestDoubleInstance(instance, asTypes);
		else
			builder.RegisterTestDoubleType(type, asTypes, this);
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
				builder.RegisterTestDoubleType(x.type, x.asTypes, this);
			}
			else
			{
				builder.RegisterTestDoubleInstance(x.instance, x.asTypes);
			}
		});
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