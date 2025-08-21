using System.Linq;
using NUnit.Framework;
using Sinjector.Autofac;

namespace Sinjector.Test;

[TestSystem]
[Parallelizable(ParallelScope.Children)]
public class ParallelizableChildrenTest
{
	public TestSystemAttribute Attribute;
	private TestSystemAttribute.TestSystemContext ctx => Attribute.SystemContext;

	private static int[] Ones = Enumerable.Repeat(1, 50).ToArray();

	[Test, TestCaseSource("Ones")]
	public void ShouldSupportParallelizable(int one)
	{
		ctx.State += one;
		Assert.AreEqual(one, ctx.State);
	}

	public class TestSystemAttribute : AutofacSinjectorFixtureAttribute, IExtendSystem
	{
		public TestSystemContext SystemContext => Resolve<TestSystemContext>();

		public void Extend(IExtend extend)
		{
			extend.AddService<TestSystemContext>();
			extend.AddService(this);
		}

		public class TestSystemContext
		{
			public int State = 0;
		}
	}
}
