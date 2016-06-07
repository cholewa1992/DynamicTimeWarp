using System;
using System.Linq;
using System.Collections.Generic;
using Recognizer.Classifiers;
using Recognizer.Filters;

namespace Recognizer
{

	public class Recognizer<T> {
		
		private readonly IClassifier<T> _classifier;
		protected readonly TemplateCollection<T> Templates;

		private readonly List<IFilter<IEnumerable<T>>> _filters = new List<IFilter<IEnumerable<T>>> ();
	
		public Recognizer(IClassifier<T> classifier, TemplateCollection<T> templates){
			Templates = templates;
			_classifier = classifier;

		}

		public void AddFilter(IFilter<IEnumerable<T>> filter){
			_filters.Add (filter);
		}

		private IEnumerable<T> Filter (IEnumerable<T> data)
		{
		    return _filters.Aggregate(data, (current, filter) => filter.Filter(current)).ToArray();
		}

	    private IEnumerable<Tuple<string,IEnumerable<T>>> Filter (IEnumerable<Tuple<string,IEnumerable<T>>> data)
	    {
	        return _filters.Aggregate(data, (current, filter) => filter.Filter(current));
	    }

	    public Tuple<string,double> TryRecognize(IEnumerable<T> data){

			var q = Filter (data).ToArray ();
			var c = Filter (Templates);

			return _classifier.ClosestLabel (q,c); 

		}

		public IList<string> GetLabels(){
			return Templates.Select (t => t.Item1).Distinct ().ToList ();
		}

		public int[][] ConfussionMatrix(){

			var labels = GetLabels ();

			var conf = new int[labels.Count][];
			for (var i = 0; i < conf.Length; i++)
				conf [i] = new int[labels.Count];

			foreach (var t1 in Templates) {

				var q = Filter (t1.Item2);
				var c = Filter (Templates.Where (t => t.Equals(t1)));

				var t2 = _classifier.ClosestLabel (q,c);

				var i1 = labels.IndexOf (t1.Item1);
				var i2 = labels.IndexOf (t2.Item1);

				if (i1 >= 0 && i2 >= 0)
					conf [i2][i1]++;
			}

			return conf;
		}
	}
	
}
