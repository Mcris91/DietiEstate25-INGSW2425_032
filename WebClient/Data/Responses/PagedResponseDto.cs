namespace DietiEstate.WebClient.Data.Responses;

public record PagedResponseDto<T>
{
    public int? PageSize { get; init; }

    public int? PageNumber { get; init; }

    public int? PrevPageNumber { get; init; }

    public int? NextPageNumber { get; init; }

    public int? TotalPages { get; init; }

    public IEnumerable<T> Items { get; } = [];
}