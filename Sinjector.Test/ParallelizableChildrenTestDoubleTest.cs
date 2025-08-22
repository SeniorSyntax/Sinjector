using NUnit.Framework;

namespace Sinjector.Test;

[TestSystem]
[Parallelizable(ParallelScope.Children)]
public class ParallelizableChildrenTestDoubleTest
{
	[Test]
	public void ShouldSupportParallelizable([Range(1, 50)] int i)
	{
	}

	public class TestSystemAttribute : SinjectorFixtureAttribute, IIsolateSystem
	{
		public void Isolate(IIsolate isolate)
		{
			isolate.UseTestDouble<FakeService>().For<IServiceInterface>();
		}

		public class FakeService : IServiceInterface
		{
		}

		public interface IServiceInterface
		{
		}
	}
}
