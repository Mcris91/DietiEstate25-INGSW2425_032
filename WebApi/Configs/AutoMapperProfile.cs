using AutoMapper;
using DietiEstate.Shared.Dtos.Responses;
using DietiEstate.Shared.Models.ListingModels;

namespace DietiEstate.WebApi.Configs;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Listing, ListingResponseDto>()
            .ForMember(listingDto => listingDto.Type, opt =>
                opt.MapFrom(src => src.Type.Name))
            .ForMember(listingDto => listingDto.Services, opt =>
                opt.MapFrom(src => src.ListingServices.Select(service => service.Name)))
            .ForMember(listingDto => listingDto.Tags, opt =>
                opt.MapFrom(src => src.ListingTags.Select(tag => tag.Name)))
            .ForMember(listingDto => listingDto.Images, opt =>
                opt.MapFrom(src => src.ListingImages.Select(image => image.Url)));
    }
}