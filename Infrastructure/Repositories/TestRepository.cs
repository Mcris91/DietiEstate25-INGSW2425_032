using DietiEstate.Application.Interfaces.Repositories;
using DietiEstate.Core.Entities.ListingModels;
using DietiEstate.Infrastracture.Data;
using Microsoft.EntityFrameworkCore;

namespace DietiEstate.Infrastracture.Repositories;

public class TestRepository(DietiEstateDbContext context) : ITestRepository
{
    public async Task<IEnumerable<Listing>> GetListings()
    {
        return await context.Listing.ToListAsync();
    }
}