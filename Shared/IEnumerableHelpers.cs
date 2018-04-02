using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Shared
{
	public static class IEnumerableHelpers
	{
		public static T RandomFirstOrDefault<T>(this IEnumerable<T> elements, Func<T, bool> predicate = null) 
		{
			predicate = predicate ?? ((t) => true); //If no predicate assume always true
			var filtered = elements.Where(predicate);
			if(filtered?.Any() != true) { return default(T); }
			var idx = new System.Random().Next(0, filtered.Count() - 1);
			return filtered.Skip(idx).First();
		}
	}
}