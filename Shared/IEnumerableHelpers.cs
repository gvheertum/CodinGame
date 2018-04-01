using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Shared
{
	public static class IEnumerableHelpers
	{
		public static T RandomFirstOrDefault<T>(this IEnumerable<T> elements, Func<T, bool> predicate) 
		{
			var filtered = elements.Where(predicate);
			if(filtered?.Any() != true) { return default(T); }
			var idx = new System.Random().Next(0, filtered.Count() - 1);
			return elements.Skip(idx).First();
		}


	}
}