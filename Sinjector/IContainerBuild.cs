using System;
using Autofac;

namespace Sinjector;

public interface IContainerBuild
{
	IComponentContext ContainerBuild(Action<ContainerBuilder> registrations);
}