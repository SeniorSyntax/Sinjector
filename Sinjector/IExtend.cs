using System;
using Autofac;

namespace Sinjector;

public interface IExtend : IHoldState, IQueryAttributes
{
	void AddService<TService>(bool instancePerLifeTimeScope = false);
	void AddService<TService>(TService instance) where TService : class;
	void AddService(Type type, bool instancePerLifeTimeScope = false);
	void AddModule(Module module);
}