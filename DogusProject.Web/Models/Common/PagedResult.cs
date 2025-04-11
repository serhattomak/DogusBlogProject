namespace DogusProject.Web.Models.Common;

public class PagedResult<T>
{
	public List<T> Items { get; set; } = new();
	public int TotalCount { get; set; }
	public int CurrentPage { get; set; }
	public int PageSize { get; set; }

	public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

	public PagedResult() { }

	public PagedResult(List<T> items, int totalCount, int currentPage, int pageSize)
	{
		Items = items;
		TotalCount = totalCount;
		CurrentPage = currentPage;
		PageSize = pageSize;
	}
}