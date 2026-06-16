using System;
using System.Collections.Generic;

namespace Sinjector.Internals;

internal static class Extensions
{
	public static void ForEach<T>(this IEnumerable<T> source, Action<T> act)
	{
		foreach (var item in source)
			act(item);
	}
}