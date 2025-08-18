using System;

namespace Sinjector;

public interface IContainerBuild<T>
{
	ITheContainer ContainerBuild(Action<T> registrations);
}