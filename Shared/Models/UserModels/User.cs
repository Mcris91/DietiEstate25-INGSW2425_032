using System.ComponentModel.DataAnnotations;

namespace DietiEstate.Shared.Models.UserModels;

public class User
{
    [Key] 
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [Required]
    public string FirstName { get; set; } = string.Empty;
    
    [Required]
    public string LastName { get; set; } = string.Empty;
    
    [Required]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    public UserRole Role { get; set; } = UserRole.Client;

}