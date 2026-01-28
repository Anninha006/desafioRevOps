using Microsoft.Extensions.Options;
using REVOPS.DevChallenge.Clients.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace REVOPS.DevChallenge.Clients;

public interface ITalkClient
{
    Task<Chat?> GetChatAsync(string chatId);
    Task<bool> UpdateContactAsync(string contactId, object contactData);
}

public class TalkClient : ITalkClient
{
    private readonly HttpClient _httpClient;
    private readonly AppSettings _settings;
    private static readonly JsonSerializerOptions _options = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public TalkClient(HttpClient httpClient, IOptions<AppSettings> settings)
    {
        _httpClient = httpClient;
        _settings = settings.Value;
        
        _httpClient.BaseAddress = new Uri("https://app-utalk.umbler.com/api/");
        _httpClient.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", _settings.Talk2ApiToken);
        _httpClient.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task<Chat?> GetChatAsync(string chatId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"v1/chats/{chatId}?organizationId={_settings.OrganizationId}");
            
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Talk API Error: {response.StatusCode} - {errorContent}");
                return null;
            }
            
            var content = await response.Content.ReadAsStringAsync();
            var chat = JsonSerializer.Deserialize<Chat>(content, _options);
            
            return chat;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error calling Talk API: {ex.Message}");
            throw;
        }
    }

    public async Task<bool> UpdateContactAsync(string contactId, object contactData)
    {
        try
        {
            var json = JsonSerializer.Serialize(contactData, _options);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PatchAsync($"v1/contacts/{contactId}?organizationId={_settings.OrganizationId}", content);
            
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Talk API Update Error: {response.StatusCode} - {errorContent}");
                return false;
            }
            
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating contact in Talk API: {ex.Message}");
            return false;
        }
    }
}
