namespace DietiEstate.Application.Dtos.Responses;

public record PagedResponseDto<T>
{
    public int? PageSize { get; init; }

    public int? PageNumber { get; init; }

    public int? PrevPageNumber { get; init; }

    public int? NextPageNumber { get; init; }

    public int? TotalPages { get; init; }

    public IEnumerable<T> Items { get; }

    public PagedResponseDto(
        IEnumerable<T> query,
        int? responsePageSize = null,
        int? responsePageNumber = null)
    {
        var listQuery = query.ToList();
        Items = listQuery;
        PageSize = responsePageSize;
        PageNumber = responsePageNumber;
        
        if (!PageNumber.HasValue || !PageSize.HasValue) return;
        
        Items = listQuery.Skip((PageNumber.Value - 1) * PageSize.Value).Take(PageSize.Value);
        PrevPageNumber = PageNumber <= 1 ? null : PageNumber - 1;
        TotalPages = (int)Math.Ceiling((double)listQuery.Count / PageSize.Value);
        NextPageNumber = PageNumber >= TotalPages ? null : PageNumber + 1;
    }
}