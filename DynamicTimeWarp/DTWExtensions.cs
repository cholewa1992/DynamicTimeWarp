using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;


namespace dk.itu.jbec.DTW {
	
	public static class DTWExtensions {
		public static Tuple<string,double> ClosestLabel<T>
		(this IDynamicTimeWrap<T> dtw, IList<T> comparee, IEnumerable<Tuple<string,IList<T>>> values) {

			Tuple<string,double> best = Tuple.Create("none",double.PositiveInfinity);

			foreach(var kvp in values){

				//Calculating DTW
				double cost = dtw.CalculateDistance(comparee,kvp.Item2);
				if(cost < best.Item2) best = Tuple.Create(kvp.Item1,cost);

			}

			return best;
		}

		public static Tuple<string,double> ClosestLabel<T>
		(this IDynamicTimeWrap<T> dtw, IEnumerable<T> comparee, IEnumerable<Tuple<string,IEnumerable<T>>> values) {

			IList<T> q = comparee as IList<T> ?? comparee.ToList();
			IEnumerable<Tuple<string,IList<T>>> c =
				values.Select (t => Tuple.Create (t.Item1, t.Item2 as IList<T> ?? t.Item2.ToList ()));

			return dtw.ClosestLabel (q, c);
		}
	}
}