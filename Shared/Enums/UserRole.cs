namespace DietiEstate.Shared.Enums;

/// <summary>
/// Represents the role of the user in the system.
/// </summary>
public enum UserRole
{
    /// <summary>
    /// The SystemAdmin can create and manage every resource in the system.
    /// This role is reserved for system administrators.
    /// </summary>
    SystemAdmin,
    
    /// <summary>
    /// The SuperAdmin is the account associated with the owner of an estate agency.
    /// The SuperAdmin can create and manage accounts for estate agents.
    /// </summary>
    SuperAdmin,
    
    /// <summary>
    /// The SupportAdmin is the account associated with support staff at an estate agency.
    /// The SupportAdmin can create and manage accounts for estate agents.
    /// </summary>
    SupportAdmin,
    
    /// <summary>
    /// The EstateAgent can upload listings, schedule appointments with clients
    /// and manage offers for listings between other clients.
    /// </summary>
    EstateAgent,
    
    /// <summary>
    /// The Clients can make offers for listings and schedule
    /// appointments with Agents
    /// </summary>
    Client
}