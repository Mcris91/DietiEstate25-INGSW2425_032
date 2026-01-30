namespace DietiEstate.WebClient.Data.Requests;

public class ListingFilterDto : BaseFilterDto
{
    public Guid? TypeId { get; init; }
    
    public Guid? AgentId { get; init; }
    
    public IReadOnlyList<Guid>? ServiceIds { get; init; }

    public IReadOnlyList<Guid>? TagIds { get; init; }

    public decimal? MinPrice { get; set; }

    public decimal? MaxPrice { get; set; }

    public int? MinRooms { get; set; }

    public int? MaxRooms { get; set; }

    public decimal? MinSize { get; set; }

    public decimal? MaxSize { get; set; }

    public string SortBy { get; init; } = "views";

    public string SortOrder { get; init; } = "desc";
}