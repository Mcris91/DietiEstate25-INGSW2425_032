namespace DietiEstate.Shared.Dtos.Requests;

public class ListingRequestDto
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid TypeId { get; set; }
    public string FeaturedImage { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
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
    public Guid? AgentUserId { get; set; } = null!;
    
    public List<Guid> Services { get; set; } = [];
    public List<Guid> Tags { get; set; } = [];
    public List<string> Images { get; set; } = [];
}