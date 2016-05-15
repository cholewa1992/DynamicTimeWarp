using System;
using System.Linq;
using System.Collections.Generic;

namespace dk.itu.jbec.DTW {
	
	public static class DTWExtensions {
		public static Tuple<string,double> ClosestLabel<T,TLb>
		(this IDynamicTimeWrap<T> dtw, T[] q, Dictionary<string,T[]> values) {

			Tuple<string,double> best = Tuple.Create("none",double.PositiveInfinity);

			foreach(var kvp in values){

				//Calculating DTW
				double cost = dtw.CalculateDistance(q,kvp.Value);
				if(cost < best.Item2) best = Tuple.Create(kvp.Key,cost);

			}

			return best;
		}
	}
}
