using DietiEstate.Core.Entities.UserModels;

namespace DietiEstate.Core.Entities.AgencyModels;

public class Agency
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Name { get; set; } = string.Empty;
    
    public virtual User Administrator { get; set; }
}