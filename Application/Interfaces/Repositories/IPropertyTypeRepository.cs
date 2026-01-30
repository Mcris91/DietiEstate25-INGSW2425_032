using DietiEstate.Core.Entities.ListingModels;

namespace DietiEstate.Application.Interfaces.Repositories;

public interface IPropertyTypeRepository
{
    Task<IEnumerable<PropertyType>> GetPropertyTypesAsync();
    
    Task<PropertyType?> GetPropertyTypeByIdAsync(Guid propertyTypeId);

    Task<PropertyType?> GetPropertyTypeByCodeAsync(string code);
    
    Task AddPropertyTypeAsync(PropertyType propertyType);
}