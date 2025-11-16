using System.ComponentModel.DataAnnotations;
using DietiEstate.WebClient.Data.Common;

namespace DietiEstate.WebClient.Data.Requests;

public class ListingRequestDto
{
    public string Name { get; set; } = string.Empty;

    [MaxLength(5000)]
    public string Description { get; set; } = string.Empty;

    public string FeaturedImage { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public string City { get; set; } = string.Empty;

    public float Latitude { get; set; }

    public float Longitude { get; set; }

    public decimal Dimensions { get; set; }

    public decimal Price { get; set; }

    public int Rooms { get; set; }

    public int Floor { get; set; }

    public bool Available { get; set; } = false;

    public bool Elevator { get; set; } = false;

    public string EnergyClass { get; set; } = string.Empty;

    public int Views { get; set; } = 0;

    public string OwnerEmail { get; set; } = string.Empty;

    public Guid? AgentUserId { get; set; } = null;

    public ListingTypeDto Type { get; set; } = new();

    public List<ListingImageDto> Images { get; set; } = [];

    public List<ListingServiceDto> Services { get; set; } = [];

    public List<ListingTagDto> Tags { get; set; } = [];
}
