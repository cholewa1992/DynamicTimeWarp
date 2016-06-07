using System.Collections.Generic;

namespace Recognizer.Filters
{

	public class Normalizer : IFilter<IEnumerable<IList<double>>>
	{

		private readonly List<double> _max = new List<double> (6);
		private readonly List<double> _min = new List<double> (6);

		public IEnumerable<IList<double>> Filter(IEnumerable<IList<double>> data)
		{
			foreach (var series in data) {	

				var nArr = new double[series.Count];

				while (_max.Count < series.Count) {
					_max.Add (double.NegativeInfinity);
				}

				while (_min.Count < series.Count) {
					_min.Add (double.PositiveInfinity);
				}

				for (var i = 0; i < series.Count; i++) {

					if (series [i] > _max [i])
						_max [i] = series[i];

					if (series [i] < _min [i])
						_min [i] = series [i];

					nArr [i] = (series [i] - _min [i]) / (_max [i] - _min [i]);
				}
				yield return nArr;
			}
		}
	}
	
}
