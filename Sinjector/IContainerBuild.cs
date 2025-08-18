using System;

namespace Sinjector;

public interface IContainerBuild
{
	ITheContainer ContainerBuild(Action<ITheContainerBuilder> registrations);
}