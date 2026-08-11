namespace Application.UseCases.Common.Pagination;
public abstract record PaginationQuery(int? PageNumber, int? PageSize)
{
    public int NormalizedPageNumber => PageNumber ?? 1;
    public int NormalizedPageSize => PageSize ?? 10;
}