using System;

namespace Sinjector;

public interface IContainerBuild
{
	ISinjectorContainer ContainerBuild(Action<ISinjectorContainerBuilder> registrations);
}