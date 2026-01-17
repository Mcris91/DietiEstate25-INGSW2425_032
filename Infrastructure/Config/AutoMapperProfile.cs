using AutoMapper;
using DietiEstate.Application.Dtos.Common;
using DietiEstate.Application.Dtos.Requests;
using DietiEstate.Application.Dtos.Responses;
using DietiEstate.Core.ValueObjects;
using DietiEstate.Core.Entities.ListingModels;
using DietiEstate.Core.Entities.OfferModels;
using DietiEstate.Core.Entities.UserModels;

namespace DietiEstate.Infrastracture.Config;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        // User
        CreateMap<UserRequestDto, User>();
        CreateMap<User, UserResponseDto>();
        CreateMap<User, LoginResponseDto>();
        CreateMap<AdminUserTemplate, User>();
        
        // Listing
        CreateMap<ListingRequestDto, Listing>();
        CreateMap<Listing, ListingResponseDto>()
            .ForMember(listingDto => listingDto.Type, opt =>
                opt.MapFrom(src => new ListingTypeDto
                {
                    Code = src.Type.Code,
                    Name = src.Type.Name
                }))
            .ForMember(listingDto => listingDto.Services, opt =>
                opt.MapFrom(src => src.ListingServices.Select(service => new ListingServiceDto
                {
                    Id = service.Id,
                    Name = service.Name
                })))
            .ForMember(listingDto => listingDto.Tags, opt =>
                opt.MapFrom(src => src.ListingTags.Select(tag => new ListingTagDto
                {
                    Id = tag.Id,
                    Name = tag.Name
                })))
            .ForMember(listingDto => listingDto.Images, opt =>
                opt.MapFrom(src => src.ListingImages.Select(image => new ListingImageResponseDto
                {
                    Id = image.Id,
                    Url = image.Url
                }))); 
        CreateMap<PropertyTypeRequestDto, PropertyType>();
        CreateMap<PropertyType, PropertyTypeResponseDto>();
        CreateMap<ListingTypeDto, PropertyType>();
        
        // Offer
        CreateMap<OfferRequestDto, Offer>();
        CreateMap<Offer, OfferResponseDto>()
            .ForMember(offerDto => offerDto.CustomerName, opt => 
                opt.MapFrom(src => src.Customer.FirstName))
            .ForMember(offerDto => offerDto.CustomerLastName, opt => 
                opt.MapFrom(src => src.Customer.LastName))
            .ForMember(offerDto => offerDto.CustomerEmail, opt => 
                opt.MapFrom(src => src.Customer.Email))
            .ForMember(offerDto => offerDto.ListingName, opt => 
                opt.MapFrom(src => src.Listing.Name))
            .ForMember(offerDto => offerDto.ListingPrice, opt => 
                opt.MapFrom(src => src.Listing.Price));
    }
}