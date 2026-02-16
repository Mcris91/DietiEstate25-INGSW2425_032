using DietiEstate.Core.Enums;

namespace DietiEstate.Core.Constants;

public static class UserScope
{
    public const string  SystemAdmin = "system-admin";
 
    public const string ReadListing = "read:listing";

    public const string WriteListing = "write:listing";

    public const string DeleteListing = "delete:listing";

    public const string ReadAgent = "read:agent";

    public const string WriteAgent = "write:agent";

    public const string DeleteAgent = "delete:agent";

    public const string ReadSupportAdmin = "read:support-admin";

    public const string WriteSupportAdmin = "write:support-admin";

    public const string DeleteSupportAdmin = "delete:support-admin";
    
    public const string ReadUser = "read:user";
    
    public const string WriteUser = "write:user";
    
    public const string DeleteUser = "delete:user";
    
    public const string ReadOffer = "read:offer";
    
    public const string WriteOffer = "write:offer";
    
    public const string DeleteOffer = "delete:offer";
    
    public const string ReadBooking = "read:booking";
    
    public const string WriteBooking = "write:booking";
    
    public const string DeleteBooking = "delete:booking";
    
    public const string ReadFavourite = "read:favourite";
    
    public const string WriteFavourite = "write:favourite";
    
    public const string DeleteFavourite = "delete:favourite";
    
    public static class Listing
    {
        public static readonly string[] All = [ReadListing, WriteListing, DeleteListing];
        public static readonly string[] ReadWrite = [ReadListing, WriteListing];
    }
    
    public static class Agent
    {
        public static readonly string[] All = [ReadAgent, WriteAgent, DeleteAgent];
        public static readonly string[] ReadWrite = [ReadAgent, WriteAgent];
    }
    
    public static class SupportAdmin
    {
        public static readonly string[] All = [ReadSupportAdmin, WriteSupportAdmin, DeleteSupportAdmin];
        public static readonly string[] ReadWrite = [ReadSupportAdmin, WriteSupportAdmin];
    }

    public static class User
    {
        public static readonly string[] All = [ReadUser, WriteUser, DeleteUser];
        public static readonly string[] ReadWrite = [ReadUser, WriteUser];
    }
    
    public static class Offer
    {
        public static readonly string[] All = [ReadOffer, WriteOffer, DeleteOffer];
        public static readonly string[] ReadWrite = [ReadOffer, WriteOffer];
    }
    
    public static class Booking
    {
        public static readonly string[] All = [ReadBooking, WriteBooking, DeleteBooking];
        public static readonly string[] ReadWrite = [ReadBooking, WriteBooking];
    }
    
    public static class Favourite
    {
        public static readonly string[] All = [ReadFavourite, WriteFavourite, DeleteFavourite];
        public static readonly string[] ReadWrite = [ReadFavourite, WriteFavourite];
    }
    
    public static readonly string[] AllScopes =
    [
        ReadListing, WriteListing, DeleteListing,
        ReadAgent, WriteAgent, DeleteAgent,
        ReadSupportAdmin, WriteSupportAdmin, DeleteSupportAdmin,
        ReadUser, WriteUser, DeleteUser,
        ReadOffer, WriteOffer, DeleteOffer,
        ReadBooking, WriteBooking, DeleteBooking,
        ReadFavourite, WriteFavourite, DeleteFavourite
    ];
}