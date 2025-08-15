using System;

namespace Sinjector;

public interface ITheContainerThingy
{
    object Resolve(Type type);
    T Resolve<T>();
}