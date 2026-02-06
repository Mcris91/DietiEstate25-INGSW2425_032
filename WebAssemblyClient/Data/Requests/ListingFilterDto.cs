namespace WebAssemblyClient.Data.Requests;

public class ListingFilterDto : BaseFilterDto
{
    public string TypeCode { get; set; } = string.Empty;
    
    public Guid? AgentId { get; init; }
    
    public IList<string>? Tags { get; set; }

    public decimal? MinPrice { get; set; }

    public decimal? MaxPrice { get; set; }

    public int? MinRooms { get; set; }

    public int? MaxRooms { get; set; }

    public decimal? MinSize { get; set; }

    public decimal? MaxSize { get; set; }

    public double? Latitude { get; set; }

    public double? Longitude { get; set; }

    public string EnergyClass { get; set; } = string.Empty;

    public string SortBy { get; init; } = "position";

    public string SortOrder { get; init; } = "desc";
}