using Autofac;
using NUnit.Framework;
using SharpTestsEx;

namespace Sinjector.Test;

[SinjectorFixture]
public class AutofacModuleTest : IExtendSystem
{
    public TheService Target;

    [Test]
    public void ShouldCallService()
    {
        Target.Return33()
            .Should().Be.EqualTo(33);
    }
    
    public void Extend(IExtend extend) => 
        extend.AddModule(new MyModule());

    public class MyModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<TheService>();
        }
    }

    public class TheService
    {
        public int Return33()
        {
            return 33;
        }
    }
}