using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using WebAssemblyClient.Data.Common;

namespace WebAssemblyClient.Data.Requests;

public class UserRequestDto
{
    [Required(ErrorMessage = "La mail è richiesta")]
    [EmailAddress(ErrorMessage = "Inserisci una mail valida")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "La password è richiesta")]
    [RegularExpression(@"^(?=.*[A-Z])(?=.*\d).{8,}$", ErrorMessage = "La password deve essere lunga almeno 8 caratteri e deve contenere almeno una lettera maiuscola ed un numero")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Il nome è richiesto")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Il cognome è richiesto")]
    public string LastName { get; set; } = string.Empty;
}