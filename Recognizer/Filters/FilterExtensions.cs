using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using dk.itu.jbec.DTW;
using System.Collections.Concurrent;

namespace Recognizer
{

	public static class FilterExtensions {
		public static IEnumerable<Tuple<string,T>> Filter<T>(this IFilter<T> filter, IEnumerable<Tuple<string,T>> data){
			foreach (var template in data) {
				yield return Tuple.Create (template.Item1, filter.Filter (template.Item2));
			}
		}
	} 
	
}