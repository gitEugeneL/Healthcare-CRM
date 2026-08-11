namespace Application.UseCases.Common.Pagination;

public sealed record PaginationResult<T>(IReadOnlyList<T> Items, int TotalItems, int PageNumber, int PageSize)
{
    public int TotalPages => PageSize > 0 
        ? (int)Math.Ceiling(TotalItems / (double)PageSize) 
        : 0;
}