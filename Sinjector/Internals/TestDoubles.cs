using System;
using System.Collections.Generic;

namespace Sinjector.Internals;

internal class TestDoubles
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
		if (instance != null)
			builder.RegisterTestDoubleInstance(instance, asTypes);
		else
			builder.RegisterTestDoubleType(type, asTypes);
		
		_items.Add(new testDouble
		{
			instance = instance,
			type = type,
			asTypes = asTypes
		});
	}
	
	public void OverwriteRegistrationsFromPreviousContainer(ISinjectorContainerBuilder builder, ISinjectorContainer previousContainer)
	{
		if (previousContainer == null)
			return;
		_items.ForEach(x =>
		{
			if (x.instance == null) 
				x.instance = previousContainer.Resolve(x.type);
			
			builder.RegisterTestDoubleInstance(x.instance, x.asTypes);
		});
	}
}