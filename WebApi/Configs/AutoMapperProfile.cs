using AutoMapper;
using DietiEstate.Shared.Dtos.Responses;
using DietiEstate.Shared.Models.ListingModels;

namespace DietiEstate.WebApi.Configs;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Listing, GetListingResponseDto>();
    }
}