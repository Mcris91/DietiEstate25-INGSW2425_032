using DietiEstate.Core.Entities.FavouritesModels;

namespace DietiEstate.Application.Interfaces.Repositories;

public interface IFavouritesRepository
{
    Task<Favourite?> GetFavouriteAsync(Guid userId, Guid listingId);
    Task<IEnumerable<Favourite>> GetFavouritesAsync(Guid userId);
    Task CreateFavouriteAsync(Favourite favourite);
    Task DeleteFavouriteAsync(Favourite favourite);
}