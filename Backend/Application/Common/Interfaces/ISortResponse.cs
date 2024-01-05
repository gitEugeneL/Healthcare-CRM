namespace Application.Common.Interfaces;

public interface ISortResponse
{
    public bool SortByDate { get; init; }
    public bool SortOrderAsc { get; init; }
}
