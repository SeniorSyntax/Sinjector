using System;

namespace Sinjector;

public interface ITheContainer : IDisposable
{
    object Resolve(Type type);
    T Resolve<T>();
}