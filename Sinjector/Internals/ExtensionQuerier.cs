using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Sinjector.Internals
{
	internal class ExtensionQuerier
	{
		private object _fixture;
		private object _attribute;
		private ICustomAttributeProvider _method;
		private readonly Lazy<IEnumerable<object>> _allExtensions;

		public ExtensionQuerier()
		{
			_allExtensions = new Lazy<IEnumerable<object>>(GetAllExtensions);
		}

		public ExtensionQuerier TestMethod(MethodInfo method)
		{
			_method = method;
			return this;
		}

		public ExtensionQuerier Fixture(object fixture)
		{
			_fixture = fixture;
			return this;
		}

		public ExtensionQuerier Attribute(object attribute)
		{
			_attribute = attribute;
			return this;
		}

		public IEnumerable<T> Query<T>() =>
			_allExtensions.Value.OfType<T>().ToArray();

		public void InvokeExtensions<T>(Action<T> action) where T : class =>
			Query<T>().ForEach(action.Invoke);
		
		private IEnumerable<object> GetAllExtensions()
		{
			var attributeType = _attribute.GetType();
			var fixtureExtensions = _fixture.GetType().GetCustomAttributes(true)
				.Where(x => x.GetType() != attributeType)
				.ToArray();
			var fixtureExtensionsLevel2 = fixtureExtensions.SelectMany(x => x.GetType().GetCustomAttributes(true)).ToArray();
			var fixtureExtensionsLevel3 = fixtureExtensionsLevel2.SelectMany(x => x.GetType().GetCustomAttributes(true)).ToArray();

			var attributeExtensions = attributeType.GetCustomAttributes(true);
			var attributeExtensionsLevel2 = attributeExtensions.SelectMany(x => x.GetType().GetCustomAttributes(true)).ToArray();
			var attributeExtensionsLevel3 = attributeExtensionsLevel2.SelectMany(x => x.GetType().GetCustomAttributes(true)).ToArray();

			var methodExtensions = _method.GetCustomAttributes(true);
			var methodExtensionsLevel2 = methodExtensions.SelectMany(x => x.GetType().GetCustomAttributes(true)).ToArray();
			var methodExtensionsLevel3 = methodExtensionsLevel2.SelectMany(x => x.GetType().GetCustomAttributes(true)).ToArray();

			var extensions = new[] {_attribute}
					.Concat(attributeExtensions)
					.Concat(attributeExtensionsLevel2)
					.Concat(attributeExtensionsLevel3)
					.Concat(fixtureExtensions)
					.Concat(fixtureExtensionsLevel2)
					.Concat(fixtureExtensionsLevel3)
					.Append(_fixture)
					.Concat(methodExtensions)
					.Concat(methodExtensionsLevel2)
					.Concat(methodExtensionsLevel3)
				;

			return extensions.ToArray();
		}
	}
}
