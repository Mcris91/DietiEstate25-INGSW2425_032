using DietiEstate.Application.Interfaces.Repositories;
using DietiEstate.Core.Entities.AgencyModels;
using DietiEstate.Infrastracture.Data;

namespace DietiEstate.Infrastracture.Repositories;

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