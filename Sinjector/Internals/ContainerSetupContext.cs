using System;
using System.Collections.Generic;
using Autofac;

namespace Sinjector.Internals
{
	internal class ContainerSetupContext : IContainerSetupContext
	{
		private readonly ContainerAdaptor _containerAdaptor;
		private readonly ExtensionQuerier _extensionQuerier;

		internal ContainerSetupContext(ContainerAdaptor containerAdaptor, ExtensionQuerier extensionQuerier)
		{
			_containerAdaptor = containerAdaptor;
			_extensionQuerier = extensionQuerier;
		}

		public object State { get; set; }

		public void AddService<TService>(bool instancePerLifeTimeScope = false) =>
			_containerAdaptor.AddService<TService>(instancePerLifeTimeScope);

		public void AddService<TService>(TService instance) where TService : class =>
			_containerAdaptor.AddService(instance);

		public void AddService(Type type, bool instancePerLifeTimeScope = false)=>
			_containerAdaptor.AddService(type, instancePerLifeTimeScope);

		public void AddModule(Module module) =>
			_containerAdaptor.AddModule(module);

		public ITestDoubleFor UseTestDouble<TTestDouble>() where TTestDouble : class =>
			_containerAdaptor.UseTestDouble<TTestDouble>();

		public ITestDoubleFor UseTestDouble<TTestDouble>(TTestDouble instance) where TTestDouble : class =>
			_containerAdaptor.UseTestDouble(instance);

		public ITestDoubleFor UseTestDoubleForType(Type type) =>
			_containerAdaptor.UseTestDoubleForType(type);

		public IEnumerable<T> QueryAllAttributes<T>() =>
			_extensionQuerier.Query<T>();
	}
}
