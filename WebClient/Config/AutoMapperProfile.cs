using AutoMapper;
using DietiEstate.WebClient.Data.Common;
using DietiEstate.WebClient.Data.Requests;
using DietiEstate.WebClient.Data.Responses;

namespace DietiEstate.WebClient.Config;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<ListingResponseDto, ListingRequestDto>()
            .ForMember(listingRequest => listingRequest.FeaturedImage, opt => 
                opt.Ignore())
            .ForMember(listingRequest => listingRequest.FeaturedImageUrl, opt => 
            opt.MapFrom(src => src.FeaturedImage));
    }
}