using System;
using System.Linq;
using System.Collections.Generic;

namespace dk.itu.jbec.DTW {

	public interface IDynamicTimeWrap<in T> {
		double CalculateDistance(T[] seqA, T[] seqB);
	}

	public static class DistanceFunctions {

		public static double Euclidean(IEnumerable<double> e1, IEnumerable<double> e2){
			return Math.Sqrt(e1.Zip(e2, (a,b) => Math.Pow(a-b,2)).Sum());
		}

		public static double Euclidean(double a, double b){
			return Math.Abs(a-b);
		}

	}

}
