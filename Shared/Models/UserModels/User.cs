using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DietiEstate.Shared.Models.UserModels;

/// <summary>
/// Represents a user registered to the system.
/// </summary>
/// <remarks>This class is intended for internal use only; it is public only to allow for testing.</remarks>
public class User
{
    /// <summary>
    /// Gets or sets the unique identifier for the user.
    /// </summary>
    [Key] 
    public Guid Id { get; set; } = Guid.NewGuid();
    
    /// <summary>
    /// Gets or sets the first name for the user.
    /// </summary>
    [Required]
    public string FirstName { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the last name for the user.
    /// </summary>
    [Required]
    public string LastName { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the email for the user.
    /// </summary>
    [Required]
    public string Email { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the password for the user.
    /// </summary>
    [Required]
    [PasswordPropertyText]
    public string Password { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the role for the user.
    /// </summary>
    [Required]
    public UserRole Role { get; set; } = UserRole.Client;
}