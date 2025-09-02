using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Sinjector.Internals;

internal class Injector
{
	private readonly List<object> _targets = new();
	
	public Injector Target(object target)
	{
		_targets.Add(target);
		return this;
	}

	public void InjectFrom(ISinjectorContainer container) => 			
		_targets.ForEach(x => injectTo(container, x));
		
	private static void injectTo(ISinjectorContainer container, object target)
	{
		var type = target.GetType();
		var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
			.Where(x => x.CanWrite);
		properties.ForEach(x => x.SetValue(target, container.Resolve(x.PropertyType), null));
		var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
		fields.ForEach(x => x.SetValue(target, container.Resolve(x.FieldType)));
	}
}