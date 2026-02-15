using System.ComponentModel.DataAnnotations;
using WebAssemblyClient.Data.Common;

namespace WebAssemblyClient.Data.Requests;

public class ListingRequestDto
{
    [Required(ErrorMessage = "Inserisci un nome per l'immobile")]
    [RegularExpression(@".*[a-zA-Z0-9].*", ErrorMessage = "Inserisci del testo")]
    public string Name { get; set; } = string.Empty;

    [MaxLength(5000)]
    [Required(ErrorMessage = "Inserisci una descrizione per l'immobile")]
    [RegularExpression(@".*[a-zA-Z0-9].*", ErrorMessage = "Inserisci del testo")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Inserisci un'immagine di copertina per l'immobile")]
    public byte[] FeaturedImage { get; set; } = [];
    
    public string FeaturedImageUrl { get; set; } = string.Empty;

    [Required(ErrorMessage = "Inserisci l'indirizzo dell'immobile")]
    [RegularExpression(@".*[a-zA-Z0-9].*", ErrorMessage = "Inserisci del testo")]
    public string Address { get; set; } = string.Empty;

    public string City { get; set; } = string.Empty;

    public float Latitude { get; set; }

    public float Longitude { get; set; }

    [Required(ErrorMessage = "Inserisci le dimensioni dell'immobile")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Le dimensioni devono essere maggiori di zero")]
    public decimal Dimensions { get; set; }

    [Required(ErrorMessage = "Inserisci il prezzo dell'immobile")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Il prezzo deve essere maggiore di zero")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Inserisci il numero di stanze dell'immobile")]
    [Range(1, int.MaxValue, ErrorMessage = "Il numero di stanze deve essere maggiore di zero")]
    public int Rooms { get; set; }

    [Required(ErrorMessage = "Inserisci il piano dell'immobile")]
    [Range(0, int.MaxValue, ErrorMessage = "Il piano deve essere almeno piano terra (0)")]
    public int Floor { get; set; }

    public bool Available { get; set; } = true;

    public bool Elevator { get; set; }
    
    public bool Doorkeeper { get; set; }

    public bool AirConditioning { get; set; }
    
    public bool ParkingSpace { get; set; }

    [Required(ErrorMessage = "Seleziona la classe energetica dell'immobile")]
    public string EnergyClass { get; set; } = string.Empty;

    public int Views { get; set; }

    public string OwnerEmail { get; set; } = string.Empty;

    public Guid? AgentUserId { get; set; } = null;
    
    [Required(ErrorMessage = "Seleziona la tipologia dell'immobile")]
    public string TypeCode { get; set; } = string.Empty;
    
    public List<ListingImageRequestDto> Images { get; set; } = [];

    public List<ListingServiceDto> Services { get; set; } = [];

    public List<ListingTagDto> Tags { get; set; } = [];
}
