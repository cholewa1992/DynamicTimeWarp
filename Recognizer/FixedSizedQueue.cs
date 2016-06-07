using System.Collections.Concurrent;

namespace Recognizer
{

	public class FixedSizedQueue<T> : ConcurrentQueue<T>
	{
		private readonly object _syncObject = new object();

		public int Capacity { get; }

		public FixedSizedQueue(int capacity)
		{
			Capacity = capacity;
		}

		public new void Enqueue(T obj)
		{
			base.Enqueue(obj);
			lock (_syncObject)
			{
				while (base.Count > Capacity)
				{
					T outObj;
					base.TryDequeue(out outObj);
				}
			}
		}
	}
}
