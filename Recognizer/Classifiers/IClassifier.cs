using System;
using System.Collections.Generic;

namespace Recognizer.Classifiers
{

	public interface IClassifier<T> {

		Tuple<string,double> ClosestLabel(IEnumerable<T> comparee, IEnumerable<Tuple<string,IEnumerable<T>>> templates);

	}
	
}
