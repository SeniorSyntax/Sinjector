using System;
using Autofac;

namespace Sinjector;

public interface IContainerBuild
{
	ITheContainerThingy ContainerBuild(Action<ContainerBuilder> registrations);
}