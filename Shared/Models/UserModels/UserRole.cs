namespace DietiEstate.Shared.Models.UserModels;

/// <summary>
/// Represents the role of the user in the system.
/// </summary>
public enum UserRole
{
    /// <summary>
    /// The SuperAdmin can create and manage accounts for other admins.
    /// </summary>
    SuperAdmin,
    
    /// <summary>
    /// The Admin can create and manage accounts for estate agents.
    /// </summary>
    Admin,
    
    /// <summary>
    /// The Agent can upload listings, schedule appointments with clients
    /// and manage offers for listings between other clients.
    /// </summary>
    Agent,
    
    /// <summary>
    /// The Clients can make offers for listings and schedule
    /// appointments with Agents
    /// </summary>
    Client
}