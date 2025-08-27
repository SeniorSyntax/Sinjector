namespace Sinjector;

public static class TheContainerExtensions
{
    public static T Resolve<T>(this ITheContainer container) => 
        (T)container.Resolve(typeof(T));
}