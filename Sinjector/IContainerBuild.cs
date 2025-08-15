using System;

namespace Sinjector;

public interface IContainerBuild<T>
{
	ITheContainerThingy ContainerBuild(Action<T> registrations);
}