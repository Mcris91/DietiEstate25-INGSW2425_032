using System.ComponentModel.DataAnnotations;

namespace DietiEstate.Application.Dtos.Requests;

public class ListingRequestDto
{
        public string Name { get; init; } = string.Empty;

        [MaxLength(5000)]
        public string Description { get; init; } = string.Empty;

        public Guid TypeId { get; init; }

        public string FeaturedImage { get; init; } = string.Empty;

        public string Address { get; init; } = string.Empty;

        public float Latitude { get; init; }

        public float Longitude { get; init; }

        public decimal Dimensions { get; init; }

        public decimal Price { get; init; }

        public int Rooms { get; init; }

        public int Floor { get; init; }

        public bool Available { get; init; } = false;

        public bool Elevator { get; init; } = false;

        public string EnergyClass { get; init; } = string.Empty;

        public int Views { get; init; } = 0;

        public string OwnerEmail { get; init; } = string.Empty;

        public Guid? AgentUserId { get; init; } = null;

        public List<Guid> Services { get; init; } = [];

        public List<Guid> Tags { get; init; } = [];

        public List<string> Images { get; init; } = [];
}