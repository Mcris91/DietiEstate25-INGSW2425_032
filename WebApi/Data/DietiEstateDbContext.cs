using DietiEstate.Shared.Models.ListingModels;
using DietiEstate.Shared.Models.Shared;
using Microsoft.EntityFrameworkCore;

namespace DietiEstate.WebApi.Data;

public class DietiEstateDbContext(DbContextOptions<DietiEstateDbContext> options) : DbContext(options)
{
    public DietiEstateDbContext() : this(new DbContextOptions<DietiEstateDbContext>()) {}
    
    /// <summary>
    /// Gets or sets the DbSet for managing <see cref="Listing"/> entities.
    /// </summary>
    public virtual DbSet<Listing> Listing { get; set; }
    
    /// <summary>
    /// Gets or sets the DbSet for managing <see cref="Image"/> entities.
    /// </summary>
    public virtual DbSet<Image> Image { get; set; }
    
    /// <summary>
    /// Gets or sets the DbSet for managing <see cref="Service"/> entities.
    /// </summary>
    public virtual DbSet<Service> Service { get; set; }
    
    /// <summary>
    /// Gets or sets the DbSet for managing <see cref="Tag"/> entities.
    /// </summary>
    public virtual DbSet<Tag> Tag { get; set; }
    
    /// <summary>
    /// Gets or sets the DbSet for managing <see cref="PropertyType"/> entities.
    /// </summary>
    public virtual DbSet<PropertyType> PropertyType { get; set; }
}