using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using dk.itu.jbec.DTW;
using System.Collections.Concurrent;

namespace Recognizer
{

	public class TemplateCollection<T> : IEnumerable<Tuple<string,IEnumerable<T>>>{

		List<Tuple<string,IEnumerable<T>>> templates = new List<Tuple<string,IEnumerable<T>>>();

		public TemplateCollection(){}

		public void Add(string label, IEnumerable<T> sequence){
			templates.Add(Tuple.Create(label,sequence));
		}
			
		public IEnumerator<Tuple<string, IEnumerable<T>>> GetEnumerator ()
		{
			return templates.GetEnumerator ();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator ()
		{
			return GetEnumerator();
		}
	}
	
}
