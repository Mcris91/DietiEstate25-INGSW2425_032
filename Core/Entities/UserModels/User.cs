using DietiEstate.Core.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DietiEstate.Core.Entities.AgencyModels;

namespace DietiEstate.Core.Entities.UserModels;

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
    [PasswordPropertyText]
    public string Password { get; set; } = string.Empty;
    
    [Required]
    public UserRole Role { get; set; } = UserRole.Client;

    [ForeignKey(nameof(Agency))] 
    public Guid? AgencyId { get; set; } = null;
    
    public virtual Agency? Agency { get; set; }
}