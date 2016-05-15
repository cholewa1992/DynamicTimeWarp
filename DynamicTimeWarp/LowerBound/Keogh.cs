using System;

namespace dk.itu.jbec.DTW.LowerBound {

	public class Keogh<T> : IDTWLowerBound<T> where T : IComparable<T>
	{
		private readonly Func<T, T, double> _distance;
		private readonly T[] _l;
		private readonly T[] _u;

		private static bool Lt(T a, T b){ return a.CompareTo(b) < 0; }
		private static bool Gt(T a, T b){ return a.CompareTo(b) > 0; }

		public Keogh(T[] q, int r, Func<T, T, double> distance)
		{
			_distance = distance;

			_u = new T[q.Length];
			_l = new T[q.Length];

			//Finding the upper and lower bound for every i
			for (var i = 0; i < q.Length; i++)
			{
				int rl = i - r, rh = i + r;

				rl = rl > 0 ? rl : 0;
				rh = rh > q.Length ? q.Length : rh;

				_u[i] = q[rl];
				_l[i] = q[rl];

				for (var j = rl; j < rh; j++)
				{
					if (Gt(q[j], _u[i])) _u[i] = q[j];
					if (Lt(q[j], _l[i])) _l[i] = q[j];
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
				if (Gt(c[i], _u[i])) sum += _distance(c[i], _u[i]);
				else if (Lt(c[i], _l[i])) sum += _distance(c[i], _l[i]);
			}
			return sum;
		}
	}
	
}
