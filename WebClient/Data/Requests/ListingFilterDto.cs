namespace DietiEstate.WebClient.Data.Requests;

public class ListingFilterDto : BaseFilterDto
{
    public Guid? TypeId { get; init; }
    
    public Guid? AgentId { get; init; }
    
    public IReadOnlyList<Guid>? ServiceIds { get; init; }

    public IReadOnlyList<Guid>? TagIds { get; init; }

    public decimal? MinPrice { get; init; }

    public decimal? MaxPrice { get; init; }

    public decimal? MinRooms { get; init; }

    public decimal? MaxRooms { get; init; }

    public decimal? MinSize { get; init; }

    public decimal? MaxSize { get; init; }

    public string SortBy { get; init; } = "views";

    public string SortOrder { get; init; } = "desc";
}