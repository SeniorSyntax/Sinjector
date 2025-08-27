using System;

namespace Sinjector;

public interface ISinjectorContainer : IDisposable
{
    object Resolve(Type type);
}