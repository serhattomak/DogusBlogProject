namespace DogusProject.Domain.Common;

public class PagedResult<T>
{
	public List<T> Items { get; set; } = new();
	public int TotalPages { get; set; }
	public int CurrentPage { get; set; }
}