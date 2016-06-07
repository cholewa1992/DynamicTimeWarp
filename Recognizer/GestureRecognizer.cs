using System;
using System.Linq;
using Recognizer.Classifiers;

namespace Recognizer
{
    public class GestureRecognizer<T> : Recognizer<T>
    {
        private readonly FixedSizedQueue<T> _queue;


        public event Action<string, double> GestureRecognized;


        public GestureRecognizer(IClassifier<T> classifier, TemplateCollection<T> templates) : base(classifier, templates)
        {
            _queue = new FixedSizedQueue<T>(Templates.Max(t => t.Item2.Count()));
        }



        public void SubmitData(T data)
        {
            _queue.Enqueue(data);
            if (_queue.Capacity != _queue.Count) return;

            var result = TryRecognize(_queue);
            OnGestureRecognized(result.Item1, result.Item2);

        }

        protected virtual void OnGestureRecognized(string arg1, double arg2)
        {
            GestureRecognized?.Invoke(arg1, arg2);
        }
    }
}
