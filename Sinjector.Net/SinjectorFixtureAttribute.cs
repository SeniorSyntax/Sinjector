using System;

namespace Sinjector;

public class SinjectorFixtureAttribute : SinjectorFixtureBaseAttribute
{
    protected override ITheContainerBuilder CreateBuilder()
    {
        return new NetIocBuilder();
    }
}

public class NetIocBuilder : ITheContainerBuilder
{
    public void RegisterTestDoubleType(Type type, Type[] asTypes)
    {
        throw new NotImplementedException();
    }

    public void RegisterTestDoubleInstance(object instance, Type[] asTypes, bool propHack)
    {
        throw new NotImplementedException();
    }

    public object AddService<TService>(bool instancePerLifeTimeScope)
    {
        throw new NotImplementedException();
    }

    public object AddService(Type type, bool instancePerLifeTimeScope)
    {
        throw new NotImplementedException();
    }

    public void AddService<TService>(TService instance) where TService : class
    {
        throw new NotImplementedException();
    }

    public void Add(object thing)
    {
        throw new NotImplementedException();
    }

    public ITheContainer Build()
    {
        throw new NotImplementedException();
    }
}

public class NetIocContainer : ITheContainer
{
    public void Dispose()
    {
        throw new NotImplementedException();
    }

    public object Resolve(Type type)
    {
        throw new NotImplementedException();
    }

    public T Resolve<T>()
    {
        throw new NotImplementedException();
    }
}