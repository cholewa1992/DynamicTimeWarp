namespace Recognizer.Filters
{

	public interface IFilter<T> 
	{
		T Filter (T data);
	}
	
}
