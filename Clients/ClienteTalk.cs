/*
 * ============================================================
 * CLIENTE DA API DO TALK (uTalk)
 * ============================================================
 * Este cliente faz as requisições HTTP para a API do Talk.
 * ============================================================
 */

using Microsoft.Extensions.Options;
using REVOPS.DevChallenge.Clients.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace REVOPS.DevChallenge.Clients;

/// <summary>
/// Interface que define o contrato do cliente da API Talk
/// </summary>
public interface IClienteTalk
{
    Task<Chat?> GetChatAsync(string chatId);
}

/// <summary>
/// Implementação do cliente HTTP para a API do Talk
/// </summary>
public class ClienteTalk : IClienteTalk
{
    private readonly HttpClient _httpClient;
    private readonly AppSettings _settings;
    private static readonly JsonSerializerOptions _options = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public ClienteTalk(HttpClient httpClient, IOptions<AppSettings> settings)
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
            var response = await _httpClient.GetAsync(
                $"v1/chats/{chatId}?organizationId={_settings.OrganizationId}");
            
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Erro na API Talk: {response.StatusCode} - {errorContent}");
                return null;
            }
            
            var content = await response.Content.ReadAsStringAsync();
            var chat = JsonSerializer.Deserialize<Chat>(content, _options);
            
            return chat;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao chamar API Talk: {ex.Message}");
            throw;
        }
    }
}
