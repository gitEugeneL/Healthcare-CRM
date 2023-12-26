namespace Application.Common.Models;

public class PaginatedList<T>(IEnumerable<T> items, int totalItemsCount, int pageNumber, int pageSize)
{
    public IEnumerable<T> Items { get; } = items;
    public int PageNumber { get; } = pageNumber;
    public int TotalPages { get; } = (int) Math.Ceiling(totalItemsCount / (double) pageSize);
    public int TotalItemsCount { get; } = totalItemsCount;
}
