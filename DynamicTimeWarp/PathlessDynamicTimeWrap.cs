using System;
using System.Linq;
using System.Collections.Generic;

namespace dk.itu.jbec.DTW {

	public class PathlessDynamicTimeWrap<T> : IDynamicTimeWrap<T> {

		public readonly int W;
		public readonly Func<T, T, double> Dist;

		public PathlessDynamicTimeWrap(int w, Func<T, T, double> distFunction)
		{
			W = w;
			Dist = distFunction;
		}

		public double CalculateDistance(T[] seqA, T[] seqB)
		{
			var w = Math.Max(W, Math.Abs(seqA.Length - seqB.Length));

			double[] prev = new double[0];
			double[] cur = new double[0];

			for (int i = 0; i < seqA.Length; i++) {
				int len = 1 +
					Math.Min(i + w,
						Math.Min(w * 2,
							w + (seqB.Length - (i + 1))));

				cur = new double[Math.Min(len, seqB.Length)];

				for (int j = 0; j < cur.Length; j++) {
					int seqJ = j + Math.Max(0, i - w);
					double cost = Dist(seqA[i], seqB[seqJ]);
					int offsetJ = seqJ == j ? j : j + 1;

					if (i == 0 && j == 0) {
						cur[j] = cost;
						continue;
					}

					cur[j] = cost +
						Math.Min(GetVal(prev, offsetJ-1),
							Math.Min(GetVal(prev, offsetJ),
								GetVal(cur, j - 1)));
				}

				prev = cur;
			}

			return cur[cur.Length - 1];
		}

		public double GetVal (double[] a, int x)
		{
			return (x < 0 || x >= a.Length) ?
				double.PositiveInfinity : a[x];
		}
	}

}
