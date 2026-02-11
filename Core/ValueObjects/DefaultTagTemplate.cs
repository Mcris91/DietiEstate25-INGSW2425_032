using System.Text.Json.Serialization;

namespace DietiEstate.Core.ValueObjects;

public class DefaultTagTemplate
{
    [JsonPropertyName("NAME")]
    public string Name { get; set; } = string.Empty;
    
    [JsonPropertyName("TEXT")]
    public string Text { get; set; } = string.Empty;
}