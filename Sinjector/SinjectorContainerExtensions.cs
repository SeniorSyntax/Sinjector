namespace Sinjector;

public static class SinjectorContainerExtensions
{
    public static T Resolve<T>(this ISinjectorContainer container) => 
        (T)container.Resolve(typeof(T));
}