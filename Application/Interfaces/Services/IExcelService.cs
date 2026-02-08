using DietiEstate.Core.Entities.ListingModels;

namespace DietiEstate.Application.Interfaces.Services;

public interface IExcelService
{
    Task<byte[]> GetReportForAgent(IList<Listing>  agentListings);
}