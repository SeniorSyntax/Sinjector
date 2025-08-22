using System;

namespace Sinjector;

public interface IExtend : IHoldState, IQueryAttributes
{
	void AddService<TService>(bool instancePerLifeTimeScope = false) where TService : class;
	void AddService<TService>(TService instance) where TService : class;
	void AddService(Type type, bool instancePerLifeTimeScope = false);
	void Add(object module);
}