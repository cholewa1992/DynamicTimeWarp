using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using dk.itu.jbec.DTW;
using System.Collections.Concurrent;

namespace Recognizer
{

	public class DynamicTimeWrapClassifier<T> : IClassifier<T>
	{
		private IDynamicTimeWrap<T> _dtw;

		public DynamicTimeWrapClassifier(IDynamicTimeWrap<T> dtw){
			_dtw = dtw;
		}

		public Tuple<string, double> ClosestLabel (IEnumerable<T> comparee, IEnumerable<Tuple<string,IEnumerable<T>>> templates)
		{
			return _dtw.ClosestLabel (comparee, templates);
		}
			
	}
	
}
