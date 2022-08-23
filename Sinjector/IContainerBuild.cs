using System;
using Autofac;
using Autofac.Core;

namespace Sinjector
{
	public interface IContainerBuild
	{
		Container ContainerBuild(Action<ContainerBuilder> registrations);
	}
}