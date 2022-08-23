using Autofac.Builder;

namespace Sinjector
{
	public interface IContainerRegistrationSetup
	{
		void ContainerRegistrationSetup<TLimit, TConcreteReflectionActivatorData, TRegistrationStyle>
			(IRegistrationBuilder<TLimit, TConcreteReflectionActivatorData, TRegistrationStyle> registration) 
			where TConcreteReflectionActivatorData : ConcreteReflectionActivatorData;
	}
}
