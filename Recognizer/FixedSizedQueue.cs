using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using dk.itu.jbec.DTW;
using System.Collections.Concurrent;

namespace Recognizer
{

	public class FixedSizedQueue<T> : ConcurrentQueue<T>
	{
		private readonly object syncObject = new object();

		public int Capacity { get; private set; }

		public FixedSizedQueue(int capacity)
		{
			Capacity = capacity;
		}

		public new void Enqueue(T obj)
		{
			base.Enqueue(obj);
			lock (syncObject)
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
