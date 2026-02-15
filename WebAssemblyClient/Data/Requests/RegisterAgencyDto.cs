using System.ComponentModel.DataAnnotations;
using Microsoft.FluentUI.AspNetCore.Components.Icons.Filled;

namespace WebAssemblyClient.Data.Requests;

public class RegisterAgencyDto
{
    [Required(ErrorMessage = "Inserisci i tuoi dati")]
    public string FirstName { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Inserisci i tuoi dati")]
    public string LastName { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Inserisci un nome per l'azienda")]
    public string AgencyName { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "La mail è richiesta")]
    public string Email  { get; set; } = string.Empty;
}