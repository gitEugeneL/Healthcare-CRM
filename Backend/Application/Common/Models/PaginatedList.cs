namespace Application.Common.Models;

public class PaginatedList<T>(IReadOnlyCollection<T> items, int totalItemsCount, int pageNumber, int pageSize)
{
    public IReadOnlyCollection<T> Items { get; } = items;
    public int PageNumber { get; } = pageNumber;
    public int TotalPages { get; } = (int) Math.Ceiling(totalItemsCount / (double) pageSize);
    public int TotalItemsCount { get; } = totalItemsCount;
}
