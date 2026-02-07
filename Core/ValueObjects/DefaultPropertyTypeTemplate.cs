using System.Text.Json.Serialization;


namespace DietiEstate.Core.ValueObjects;

public class DefaultPropertyTypeTemplate
{
    [JsonPropertyName("NAME")]
    public string Name { get; set; } = string.Empty;
    
    [JsonPropertyName("CODE")]
    public string Code { get; set; } = string.Empty;
}