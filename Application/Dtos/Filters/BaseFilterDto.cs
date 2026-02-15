namespace DietiEstate.Application.Dtos.Filters;

public abstract class BaseFilterDto
{
    public Guid? AgencyId { get; set; }
    public Guid? AgentId { get; set; }
}