using DietiEstate.Application.Interfaces.Repositories;
using DietiEstate.Core.Entities.ListingModels;
using DietiEstate.Infrastracture.Data;
using Microsoft.EntityFrameworkCore;

namespace DietiEstate.Infrastracture.Repositories;

public class PropertyTypeRepository(DietiEstateDbContext context) : IPropertyTypeRepository
{
    public async Task<IEnumerable<PropertyType>> GetPropertyTypesAsync()
    {
        return await context.PropertyType.ToListAsync();
    }
    
    public async Task<PropertyType?> GetPropertyTypeByIdAsync(Guid propertyTypeId)
    {
        return await context.PropertyType.FindAsync(propertyTypeId);
    }

    public async Task<PropertyType?> GetPropertyTypeByCodeAsync(string code)
    {
        return await context.PropertyType.FirstOrDefaultAsync(pt => pt.Code == code);
    }

    public async Task AddPropertyTypeAsync(PropertyType propertyType)
    {
        await context.Database.BeginTransactionAsync();
        await context.PropertyType.AddAsync(propertyType);
        await context.SaveChangesAsync();
        await context.Database.CommitTransactionAsync();
    }
}