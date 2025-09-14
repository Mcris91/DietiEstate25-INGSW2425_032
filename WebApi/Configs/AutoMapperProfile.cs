using AutoMapper;
using DietiEstate.Shared.Dtos.Requests;
using DietiEstate.Shared.Dtos.Responses;
using DietiEstate.Shared.Models.ListingModels;
using DietiEstate.Shared.Models.UserModels;

namespace DietiEstate.WebApi.Configs;

/// <summary>
/// Defines the AutoMapper profile to map the entities to DTOs and vice versa.
/// </summary>
public class AutoMapperProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AutoMapperProfile" /> class.
    /// </summary>
    public AutoMapperProfile()
    {
        CreateMap<SignUpRequestDto, User>();
        CreateMap<ListingRequestDto, Listing>();
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