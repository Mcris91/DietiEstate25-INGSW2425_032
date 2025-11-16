using DietiEstate.Core.Entities.ListingModels;

namespace DietiEstate.Application.Interfaces.Repositories;

public interface ITestRepository
{
    Task<IEnumerable<Listing>> GetListings();
}