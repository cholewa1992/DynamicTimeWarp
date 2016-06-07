using System;
using Recognizer;

namespace GestureRecognizer
{
    class GestureRecognizer<T>
    {
        private readonly Recognizer<T> _recognizer;
        private FixedSizedQueue<T> _queue;
        private int _count;


        public GestureRecognizer(Recognizer<T> recognizer)
        {
            _recognizer = recognizer;
            _queue = new FixedSizedQueue<T>(recognizer.GetMaxLength());
        }

        public event Action<string, double> GestureRecognized;

        public void SubmitData(T data)
        {
            _queue.Enqueue(data);

            if (_queue.Capacity == _queue.Count && ++_count % (_queue.Capacity*75/100) == 0)
            {
                var result = _recognizer.TryRecognize(_queue);
                if (result.Item2 < 0.9) {
                    _count--;
                    return;
                }
                OnGestureRecognized(result.Item1,result.Item2);
            }
        }

        protected virtual void OnGestureRecognized(string arg1, double arg2)
        {
            GestureRecognized?.Invoke(arg1, arg2);
        }
    }
	
	/*var queue = new FixedSizedQueue<IList<double>>(cap);

	var dtw = new PathlessDynamicTimeWrap<IList<double>>(cap * 10 / 100, DistanceFunctions.Euclidean);
	var classifier = new DynamicTimeWrapClassifier<IList<double>> (dtw);
	var recognizer = new Recognizer<IList<double>> (classifier, templates);
	recognizer.AddFilter (new Normalizer ());*/

}
