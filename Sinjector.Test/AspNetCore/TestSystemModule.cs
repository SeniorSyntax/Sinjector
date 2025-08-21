using Autofac;

namespace Sinjector.Test.AspNetCore;

public class TestSystemModule : Module
{
	protected override void Load(ContainerBuilder builder)
	{
		builder.RegisterType<RealService>().As<ITestService>();
	}

	public class RealService : ITestService
	{
		public string Value() => "Real service";
	}
}
