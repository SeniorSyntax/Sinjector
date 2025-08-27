using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Sinjector.Internals;

internal class Injector
{
	private ISinjectorContainer _container;
	private readonly List<object> _targets = new();

	public Injector Source(ISinjectorContainer container)
	{
		_container = container;
		return this;
	}

	public Injector Target(object target)
	{
		_targets.Add(target);
		return this;
	}

	public void Inject() => 			
		_targets.ForEach(x => injectTo(_container, x));
		
	private static void injectTo(ISinjectorContainer container, object target)
	{
		var type = target.GetType();
		var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
			.Where(x => x.CanWrite);
		properties.ForEach(x => x.SetValue(target, container.Resolve(x.PropertyType), null));
		var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
		fields.ForEach(x => x.SetValue(target, container.Resolve(x.FieldType)));
	}

	internal static void Inject(object target, object instance)
	{
		var type = target.GetType();
		var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
			.Where(x => x.CanWrite)
			.Where(x => x.PropertyType.IsInstanceOfType(instance));
		properties.ForEach(x => x.SetValue(target, instance, null));
		var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance)
			.Where(x => x.FieldType.IsInstanceOfType(instance));
		fields.ForEach(x => x.SetValue(target, instance));
	}
}