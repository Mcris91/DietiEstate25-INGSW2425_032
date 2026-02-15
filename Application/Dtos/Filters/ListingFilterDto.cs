namespace DietiEstate.Application.Dtos.Filters;

public class ListingFilterDto : BaseFilterDto
{
    public string TypeCode { get; init; } = string.Empty;
    public IReadOnlyList<string>? Tags { get; init; }

    public decimal? MinPrice { get; init; }

    public decimal? MaxPrice { get; init; }

    public decimal? MinRooms { get; init; }

    public decimal? MaxRooms { get; init; }

    public decimal? MinSize { get; init; }

    public decimal? MaxSize { get; init; }

    public double? Latitude { get; set; } = 41.9028;

    public double? Longitude { get; set; } = 12.4964;

    public string EnergyClass { get; init; } = string.Empty;

    public string SortBy { get; init; } = "position";

    public string SortOrder { get; init; } = "desc";
}