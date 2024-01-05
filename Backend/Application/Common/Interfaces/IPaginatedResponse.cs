using System.ComponentModel.DataAnnotations;

namespace Application.Common.Interfaces;

public interface IPaginatedResponse
{
    [Range(1, int.MaxValue)]
    public int PageNumber { get; init; }
    
    [Range(5, 30)]
    public int PageSize { get; init; } 
}
