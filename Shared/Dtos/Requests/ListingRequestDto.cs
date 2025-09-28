using System.ComponentModel.DataAnnotations;
using DietiEstate.Shared.Models.ListingModels;

namespace DietiEstate.Shared.Dtos.Requests;

/// <summary>
/// Represents a data transfer object for creating or updating a <see cref="Listing"/> entity.
/// </summary>
public class ListingRequestDto
{
        /// <summary>
        /// Gets or sets the name of the property.
        /// </summary>
        public string Name { get; init; } = string.Empty;

        /// <summary>
        /// Gets or sets the description of the property.
        /// </summary>
        [MaxLength(5000)]
        public string Description { get; init; } = string.Empty;

        /// <summary>
        /// Gets or sets the identifier of the property type.
        /// </summary>
        public Guid TypeId { get; init; }

        /// <summary>
        /// Gets or sets the URL or path of the featured image.
        /// </summary>
        public string FeaturedImage { get; init; } = string.Empty;

        /// <summary>
        /// Gets or sets the address of the property.
        /// </summary>
        public string Address { get; init; } = string.Empty;

        /// <summary>
        /// Gets or sets the latitude coordinate of the property.
        /// </summary>
        public float Latitude { get; init; }

        /// <summary>
        /// Gets or sets the longitude coordinate of the property.
        /// </summary>
        public float Longitude { get; init; }

        /// <summary>
        /// Gets or sets the dimensions of the property in square meters.
        /// </summary>
        public decimal Dimensions { get; init; }

        /// <summary>
        /// Gets or sets the price of the property.
        /// </summary>
        public decimal Price { get; init; }

        /// <summary>
        /// Gets or sets the number of rooms in the property.
        /// </summary>
        public int Rooms { get; init; }

        /// <summary>
        /// Gets or sets the floor number of the property.
        /// </summary>
        public int Floor { get; init; }

        /// <summary>
        /// Gets or sets a value indicating whether the property is available.
        /// </summary>
        public bool Available { get; init; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether the building has an elevator.
        /// </summary>
        public bool Elevator { get; init; } = false;

        /// <summary>
        /// Gets or sets the energy class of the property.
        /// </summary>
        public string EnergyClass { get; init; } = string.Empty;

        /// <summary>
        /// Gets or sets the number of views of the listing.
        /// </summary>
        public int Views { get; init; } = 0;

        /// <summary>
        /// Gets or sets the email address of the property owner.
        /// </summary>
        public string OwnerEmail { get; init; } = string.Empty;

        /// <summary>
        /// Gets or sets the optional identifier of the agent user.
        /// </summary>
        public Guid? AgentUserId { get; init; } = null;

        /// <summary>
        /// Gets or sets the collection of service identifiers associated with the property.
        /// </summary>
        public List<Guid> Services { get; init; } = [];

        /// <summary>
        /// Gets or sets the collection of tag identifiers associated with the property.
        /// </summary>
        public List<Guid> Tags { get; init; } = [];

        /// <summary>
        /// Gets or sets the collection of image URLs related to the property.
        /// </summary>
        public List<string> Images { get; init; } = [];
}