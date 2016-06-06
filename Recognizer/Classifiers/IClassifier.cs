using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using dk.itu.jbec.DTW;
using System.Collections.Concurrent;

namespace Recognizer
{

	public interface IClassifier<T> {

		Tuple<string,double> ClosestLabel(IEnumerable<T> comparee, IEnumerable<Tuple<string,IEnumerable<T>>> templates);

	}
	
}
