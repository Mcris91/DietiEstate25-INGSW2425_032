using System.Text;
using System.Text.Json;

namespace WebAssemblyClient.ApiService;

public abstract class BaseApiService(HttpClient httpClient, JsonSerializerOptions jsonSerializerOptions)
{
    protected async Task<T> GetAsync<T>(string uri)
    {
        var response = await httpClient.GetAsync(uri);
        if (!response.IsSuccessStatusCode) throw new HttpRequestException($"Exception in ApiCall: {response.StatusCode} - {response.ReasonPhrase}"); 
        var json = await response.Content.ReadAsStringAsync();
        var deserializedObject = JsonSerializer.Deserialize<T>(json, jsonSerializerOptions) ?? throw new InvalidOperationException("Deserialization returned null.");
        return deserializedObject;
    }

    protected async Task<TResponse> PostAsync<TRequest, TResponse>(string uri, TRequest data)
    {
        var content = new StringContent(JsonSerializer.Serialize(data, jsonSerializerOptions), Encoding.UTF8, "application/json");
        var response = await httpClient.PostAsync(uri, content);
        if (!response.IsSuccessStatusCode) throw new HttpRequestException($"Exception in ApiCall: {response.StatusCode} - {response.ReasonPhrase}"); 
        var json = await response.Content.ReadAsStringAsync(); 
        return JsonSerializer.Deserialize<TResponse>(json, jsonSerializerOptions) ?? throw new InvalidOperationException("Deserialization returned null.");
    }
}