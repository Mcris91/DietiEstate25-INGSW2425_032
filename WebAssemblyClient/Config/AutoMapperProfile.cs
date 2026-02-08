using AutoMapper;
using WebAssemblyClient.Data.Requests;
using WebAssemblyClient.Data.Responses;

namespace WebAssemblyClient.Config;

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