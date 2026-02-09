using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DietiEstate.Core.Entities.UserModels;

namespace DietiEstate.Core.Entities.AgencyModels;

public class Agency
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Name { get; set; } = string.Empty;
    
    public virtual ICollection<User> Employees { get; set; } = new List<User>();
}