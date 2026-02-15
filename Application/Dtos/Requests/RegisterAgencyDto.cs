namespace DietiEstate.Application.Dtos.Requests;

public class RegisterAgencyDto
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string AgencyName { get; init; } = string.Empty;
    public string Email  { get; init; } = string.Empty;
}