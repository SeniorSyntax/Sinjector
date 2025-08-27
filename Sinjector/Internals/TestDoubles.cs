using System;
using System.Collections.Generic;

namespace Sinjector.Internals;

internal class TestDoubles : ITestDoubles
{
	private readonly List<testDouble> _items = [];

	private class testDouble
	{
		public object instance;
		public Type type;
		public Type[] asTypes;
	}

	public void Register(ISinjectorContainerBuilder builder, object instance, Type type, Type[] asTypes)
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

	private void register(ISinjectorContainerBuilder builder, testDouble testDouble)
	{
		var instance = testDouble.instance;
		var type = testDouble.type;
		var asTypes = testDouble.asTypes;

		if (instance != null)
			builder.RegisterTestDoubleInstance(instance, asTypes);
		else
			builder.RegisterTestDoubleType(type, asTypes);
	}
	
	public void RegisterFromPreviousContainer(ISinjectorContainer sinjectorContainer, ISinjectorContainerBuilder builder)
	{
		_items.ForEach(x =>
		{
			if (x.instance == null) 
				x.instance = sinjectorContainer.Resolve(x.type);
			
			builder.RegisterTestDoubleInstance(x.instance, x.asTypes);
		});
	}
}