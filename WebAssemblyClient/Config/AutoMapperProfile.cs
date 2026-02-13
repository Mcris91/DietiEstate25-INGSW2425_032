using AutoMapper;
using Microsoft.FluentUI.AspNetCore.Components.Icons.Filled;
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
        
        CreateMap<ListingImageResponseDto, ListingImageRequestDto>()
            .ForMember(dest => dest.Id, opt => 
                opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.PreviewUrl, opt => 
                opt.MapFrom(src => src.Url))
            .ForMember(dest => dest.Image, opt => 
                opt.Ignore());
    }
}