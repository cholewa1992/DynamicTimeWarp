using System;

namespace dk.itu.jbec.DTW.LowerBound {

	public class Mckb<T> : IDTWLowerBound<T>
	{
		private readonly T[] _q;
		private readonly Func<T, T, double> _distance;
		private readonly double[] _u;

		public Mckb(T[] q, int r, Func<T, T, double> distance)
		{
			_q = q;
			_distance = distance;
			_u = new double[q.Length];

			//Finding the upper and lower bound for every i
			for (var i = 0; i < q.Length; i++)
			{
				int rl = i - r, rh = i + r;

				rl = rl > 0 ? rl : 0;
				rh = rh > q.Length ? q.Length : rh;

				for (var j = rl; j < rh; j++)
				{
					_u[i] = Math.Max(distance(q[i], q[j]), _u[i]);
				}
			}
		}

		/// <summary>
		/// Return the lower bound value for DTW(q,c)
		/// </summary>
		/// <param name="c">the compare sequence</param>
		/// <returns>A lowerbound for the DTW(q,c) distance</returns>
		public double LowerBound(T[] c)
		{
			double sum = 0;
			for (var i = 0; i < c.Length && i < _u.Length; i++)
			{
				sum += Math.Max(_distance(_q[i], c[i]) - _u[i], 0);
			}
			return sum;
		}
	}
}
