using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Recognizer
{

	public class TemplateCollection<T> : IEnumerable<Tuple<string,IEnumerable<T>>>{
	    readonly List<Tuple<string,IEnumerable<T>>> _templates = new List<Tuple<string,IEnumerable<T>>>();

		public void Add(string label, IEnumerable<T> sequence){
			_templates.Add(Tuple.Create(label,sequence));
		}
			
		public IEnumerator<Tuple<string, IEnumerable<T>>> GetEnumerator ()
		{
		    return _templates.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator ()
		{
			return GetEnumerator();
		}
	}
	
}
