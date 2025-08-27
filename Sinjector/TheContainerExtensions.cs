namespace Sinjector;

public static class TheContainerExtensions
{
    public static T Resolve<T>(this ISinjectorContainer container) => 
        (T)container.Resolve(typeof(T));
}