using DietiEstate.Core.Entities.BookingModels;
using DietiEstate.Core.Entities.ListingModels;
using DietiEstate.Core.Entities.Common;
using DietiEstate.Core.Entities.OfferModels;
using DietiEstate.Core.Entities.UserModels;
using Microsoft.EntityFrameworkCore;

namespace DietiEstate.Infrastracture.Data;

public class  DietiEstateDbContext(DbContextOptions<DietiEstateDbContext> options) : DbContext(options)
{
    public DietiEstateDbContext() : this(new DbContextOptions<DietiEstateDbContext>()) {}

    public virtual DbSet<Listing> Listing { get; set; }
    
    public virtual DbSet<Offer> Offer { get; set; }
 
    public virtual DbSet<Image> Image { get; set; }

    public virtual DbSet<Service> Service { get; set; }

    public virtual DbSet<Tag> Tag { get; set; }

    public virtual DbSet<PropertyType> PropertyType { get; set; }

    public virtual DbSet<User> User { get; set; }

    public virtual DbSet<UserSession> UserSession { get; set; }

    public virtual DbSet<UserVerification> UserVerification { get; set; }
    
    public virtual DbSet<Booking>  Booking { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Listing>()
            .HasMany(l => l.ListingImages)
            .WithMany(i => i.Listings)
            .UsingEntity("ListingImage");
        
        modelBuilder.Entity<Image>(entity =>
        {
            entity.HasIndex(i => i.Url)
                .IsUnique();
        });
        
        modelBuilder.Entity<PropertyType>(entity =>
        {
            entity.HasIndex(pt => pt.Code)
                .IsUnique();
        });
    }
}
