using System;

namespace Sinjector;

public interface IExtend : IQueryAttributes
{
	void AddService<TService>() where TService : class;
	void AddService<TService>(TService instance) where TService : class;
	void AddService(Type type);
	void Add(object actionOnBuilder);
}