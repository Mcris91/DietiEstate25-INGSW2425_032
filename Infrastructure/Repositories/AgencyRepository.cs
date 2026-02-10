using DietiEstate.Application.Interfaces.Repositories;
using DietiEstate.Core.Entities.AgencyModels;
using DietiEstate.Infrastructure.Data;

namespace DietiEstate.Infrastructure.Repositories;

public class AgencyRepository(DietiEstateDbContext context) : IAgencyRepository
{
    public async Task AddAgencyAsync(Agency agency)
    {
        await context.Database.BeginTransactionAsync();
        await context.Agency.AddAsync(agency);
        await context.SaveChangesAsync();
        await context.Database.CommitTransactionAsync();
    }
}