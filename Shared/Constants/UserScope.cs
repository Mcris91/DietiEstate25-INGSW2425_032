using DietiEstate.Shared.Models.UserModels;

namespace DietiEstate.Shared.Constants;

/// <summary>
/// Provides constants and structured groupings for user scope values used across authentication and authorization policies.
/// </summary>
public static class UserScope
{
    /// <summary>
    /// Grant or restrict access to users attempting to read <see cref="Listing"/> data.
    /// </summary>
    public const string ReadListing = "read:listing";

    /// <summary>
    /// Grant or restrict access to users attempting to create, update, or modify <see cref="Listing"/> data.
    /// </summary>
    public const string WriteListing = "write:listing";

    /// <summary>
    /// Grant or restrict access to users attempting to delete <see cref="Listing"/> data.
    /// </summary>
    public const string DeleteListing = "delete:listing";

    /// <summary>
    /// Grant or restrict access to users attempting to read <see cref="UserRole.Agent"/> data.
    /// </summary>
    public const string ReadAgent = "read:agent";

    /// <summary>
    /// Grant or restrict access to users attempting to create, update, or modify <see cref="UserRole.Agent"/> data.
    /// </summary>
    public const string WriteAgent = "write:agent";

    /// <summary>
    /// Grant or restrict access to users attempting to delete <see cref="UserRole.Agent"/> data.
    /// </summary>
    public const string DeleteAgent = "delete:agent";

    /// <summary>
    /// Grant or restrict access to users attempting to read <see cref="UserRole.Admin"/> data.
    /// </summary>
    public const string ReadSupportAdmin = "read:support-admin";

    /// <summary>
    /// Grant or restrict access to users attempting to create, update, or modify <see cref="UserRole.Admin"/> data.
    /// </summary>
    public const string WriteSupportAdmin = "write:support-admin";

    /// <summary>
    /// Grant or restrict access to users attempting to delete <see cref="UserRole.Admin"/> data.
    /// </summary>
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