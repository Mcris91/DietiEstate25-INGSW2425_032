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
    
    public static readonly string[] AllScopes =
    [
        ReadListing, WriteListing, DeleteListing,
        ReadAgent, WriteAgent, DeleteAgent,
        ReadSupportAdmin, WriteSupportAdmin, DeleteSupportAdmin
    ];
}