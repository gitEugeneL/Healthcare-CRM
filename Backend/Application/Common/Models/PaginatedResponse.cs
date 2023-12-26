using System.ComponentModel.DataAnnotations;

namespace Application.Common.Models;

public abstract record PaginatedResponse
{
    [Range(1, int.MaxValue)]
    public int PageNumber { get; init; } = 1;
    
    [Range(5, 30)]
    public int PageSize { get; init; } = 10; 
}
