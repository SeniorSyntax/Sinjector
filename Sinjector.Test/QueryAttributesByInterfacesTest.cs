using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SharpTestsEx;

namespace Sinjector.Test;

[TestFixture]
[TestSystem]
public class QueryAttributesByInterfacesTest
{
	public TestSystemAttribute Context;

	[Test]
	[TestQueriable("ShouldFindAttributeFromIsolateExtension")]
	public void ShouldFindAttributeFromIsolateExtension()
	{
		Context.QueryAllAttributes<TestSystemExtensionAttribute>()
			.Single()
			.IsolateFoundAttributes.Single()
			.Message.Should().Be("ShouldFindAttributeFromIsolateExtension");
	}

	[Test]
	[TestQueriable("ShouldFindAttributeFromExtendExtension")]
	public void ShouldFindAttributeFromExtendExtension()
	{
		Context.QueryAllAttributes<TestSystemExtensionAttribute>()
			.Single()
			.ExtendFoundAttributes.Single()
			.Message.Should().Be("ShouldFindAttributeFromExtendExtension");
	}

	[Test]
	[TestQueriable("ShouldFindAttributeFromContainerSetupExtension")]
	public void ShouldFindAttributeFromContainerSetupExtension()
	{
		Context.QueryAllAttributes<TestSystemExtensionAttribute>()
			.Single()
			.ContainerSetupFoundAttributes.Single()
			.Message.Should().Be("ShouldFindAttributeFromContainerSetupExtension");
	}

	[TestSystemExtension]
	public class TestSystemAttribute : IoCTestAttribute
	{
		public new IEnumerable<T> QueryAllAttributes<T>() =>
			base.QueryAllAttributes<T>();
	}

	public class TestSystemExtensionAttribute : Attribute, IContainerSetup, IExtendSystem, IIsolateSystem
	{
		public IEnumerable<TestQueriableAttribute> ContainerSetupFoundAttributes;
		public IEnumerable<TestQueriableAttribute> IsolateFoundAttributes;
		public IEnumerable<TestQueriableAttribute> ExtendFoundAttributes;

		public void ContainerSetup(IContainerSetupContext context)
		{
			ContainerSetupFoundAttributes = context.QueryAllAttributes<TestQueriableAttribute>();
		}

		public void Extend(IExtend extend)
		{
			ExtendFoundAttributes = extend.QueryAllAttributes<TestQueriableAttribute>();
		}

		public void Isolate(IIsolate isolate)
		{
			IsolateFoundAttributes = isolate.QueryAllAttributes<TestQueriableAttribute>();
		}
	}

	public class TestQueriableAttribute : Attribute
	{
		public string Message { get; }

		public TestQueriableAttribute(string message)
		{
			Message = message;
		}
	}
}
