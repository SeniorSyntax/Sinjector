

using NUnit.Framework;
using SharpTestsEx;

namespace Sinjector.Test;

[SinjectorFixture]
public class AddServiceTest : IExtendSystem
{
    public IService TheIService;

    [Test]
    public void ShouldResolveServiceUsingInterface()
    {
       TheIService.Value().Should().Be.EqualTo(37);
    } 
    
    public void Extend(IExtend extend)
    {
        extend.AddService<Service>();
    }

    public class Service : IService
    {
        public int Value() => 37;
    }
    
    public interface IService
    {
        int Value();
    }
}