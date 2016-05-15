using System;

namespace dk.itu.jbec.DTW.LowerBound {

	public interface IDTWLowerBound<in T>
	{
		double LowerBound(T[] c);
	}
	
}
