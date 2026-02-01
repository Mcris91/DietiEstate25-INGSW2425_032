using DietiEstate.Core.Entities.AgencyModels;

namespace DietiEstate.Application.Interfaces.Repositories;

public interface IAgencyRepository
{
    Task AddAgencyAsync(Agency agency);
}