using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using dk.itu.jbec.DTW;
using System.Collections.Concurrent;

namespace Recognizer
{

	public class Recognizer<T> {
		
		private IClassifier<T> _classifier;
		private TemplateCollection<T> _templates;

		private List<IFilter<IEnumerable<T>>> _filters = new List<IFilter<IEnumerable<T>>> ();
	
		public Recognizer(IClassifier<T> classifier, TemplateCollection<T> templates){
			_templates = templates;
			_classifier = classifier;

		}

		public void AddFilter(IFilter<IEnumerable<T>> filter){
			_filters.Add (filter);
		}

		private IEnumerable<T> Filter (IEnumerable<T> data){

			var curr = data;

			foreach (var filter in _filters) {
				curr = filter.Filter (curr).ToArray ();
			}

			return curr;

		}

		private IEnumerable<Tuple<string,IEnumerable<T>>> Filter (IEnumerable<Tuple<string,IEnumerable<T>>> data){
			
			var curr = data;

			foreach(var filter in _filters){
				curr = filter.Filter (curr);
			}

			return curr;

		}

		public Tuple<string,double> TryRecognize(IEnumerable<T> data){

			var q = Filter (data).ToArray ();
			var c = Filter (_templates);

			return _classifier.ClosestLabel (q,c); 

		}

		public IList<string> GetLabels(){
			return _templates.Select (t => t.Item1).Distinct ().ToList ();
		}

		public int[][] ConfussionMatrix(){

			var labels = GetLabels ();

			int[][] conf = new int[labels.Count][];
			for (int i = 0; i < conf.Length; i++)
				conf [i] = new int[labels.Count];

			foreach (var t1 in _templates) {

				var q = Filter (t1.Item2);
				var c = Filter (_templates.Where (t => t != t1));

				var t2 = _classifier.ClosestLabel (q,c);

				int i1 = labels.IndexOf (t1.Item1);
				int i2 = labels.IndexOf (t2.Item1);

				if (i1 >= 0 && i2 >= 0)
					conf [i2][i1]++;
			}

			return conf;
		}
	}
	
}
