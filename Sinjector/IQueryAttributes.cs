using System.Collections.Generic;

namespace Sinjector
{
	public interface IQueryAttributes
	{
		IEnumerable<T> QueryAllAttributes<T>();
	}
}
