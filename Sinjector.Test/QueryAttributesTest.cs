using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SharpTestsEx;

namespace Sinjector.Test;

[TestSystem]
[TestQueriable("QueryAttributesTest")]
public class QueryAttributesTest
{
	public TestSystemAttribute Context;

	[Test]
	[TestQueriable("ShouldFindAttributeFromTest")]
	public void ShouldFindAttributeFromTest()
	{
		Context.QueryAllAttributes<TestQueriableAttribute>()
			.Select(x => x.Message)
			.Should().Contain("ShouldFindAttributeFromTest");
	}

	[Test]
	public void ShouldFindAttributeFromFixture()
	{
		Context.QueryAllAttributes<TestQueriableAttribute>()
			.Select(x => x.Message)
			.Should().Contain("QueryAttributesTest");
	}

	[Test]
	public void ShouldFindAttributeFromSystemAttribute()
	{
		Context.QueryAllAttributes<TestQueriableAttribute>()
			.Select(x => x.Message)
			.Should().Contain("TestSystemAttribute");
	}

	[Test]
	public void ShouldFindAttributeFromExtensionAttribute()
	{
		Context.QueryAllAttributes<TestQueriableAttribute>()
			.Select(x => x.Message)
			.Should().Contain("TestSystemExtendedAttribute");
	}

	[Test]
	public void ShouldFindAttributeFromExtensionLevel2Attribute()
	{
		Context.QueryAllAttributes<TestQueriableAttribute>()
			.Select(x => x.Message)
			.Should().Contain("TestSystemExtendedLevel2Attribute");
	}

	[TestSystemExtension]
	[TestQueriable("TestSystemAttribute")]
	public class TestSystemAttribute : SinjectorFixtureAttribute
	{
		public new IEnumerable<T> QueryAllAttributes<T>() =>
			base.QueryAllAttributes<T>();
	}

	[TestQueriable("TestSystemExtendedAttribute")]
	[TestSystemExtensionLevel2]
	public class TestSystemExtensionAttribute : Attribute;

	[TestQueriable("TestSystemExtendedLevel2Attribute")]
	public class TestSystemExtensionLevel2Attribute : Attribute;

	public class TestQueriableAttribute : Attribute
	{
		public string Message { get; }

		public TestQueriableAttribute(string message)
		{
			Message = message;
		}
	}
}
