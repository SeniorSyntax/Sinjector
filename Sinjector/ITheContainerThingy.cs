using System;

namespace Sinjector;

public interface ITheContainerThingy : IDisposable
{
    object Resolve(Type type);
    T Resolve<T>();
}