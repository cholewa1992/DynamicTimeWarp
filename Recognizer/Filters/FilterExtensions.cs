using System;
using System.Collections.Generic;
using System.Linq;

namespace Recognizer.Filters
{

	public static class FilterExtensions {
		public static IEnumerable<Tuple<string,T>> Filter<T>(this IFilter<T> filter, IEnumerable<Tuple<string,T>> data)
		{
		    return data.Select(template => Tuple.Create (template.Item1, filter.Filter (template.Item2)));
		}
	} 
	
}