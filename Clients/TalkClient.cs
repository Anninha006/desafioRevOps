/*
 * ============================================================
 * CLIENTE DA API DO TALK (uTalk)
 * ============================================================
 * Este cliente faz as requisições HTTP para a API do Talk.
 * Ele é responsável por:
 * 1. Configurar a autenticação (Bearer Token)
 * 2. Fazer a chamada GET para buscar dados de um chat
 * 3. Deserializar a resposta JSON
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
public interface ITalkClient
{
    Task<Chat?> GetChatAsync(string chatId);
}

/// <summary>
/// Implementação do cliente HTTP para a API do Talk
/// </summary>
public class TalkClient : ITalkClient
{
    // Cliente HTTP fornecido pelo framework (já vem configurado)
    private readonly HttpClient _httpClient;
    
    // Configurações do appsettings.json (token e organização)
    private readonly AppSettings _settings;
    
    // Opções para o deserializador JSON (ignora maiúsculas/minúsculas)
    private static readonly JsonSerializerOptions _options = new()
    {
        PropertyNameCaseInsensitive = true
    };

    /// <summary>
    /// Construtor - configura o HttpClient com autenticação
    /// </summary>
    public TalkClient(HttpClient httpClient, IOptions<AppSettings> settings)
    {
        _httpClient = httpClient;
        _settings = settings.Value;
        
        // Configura a URL base da API
        _httpClient.BaseAddress = new Uri("https://app-utalk.umbler.com/api/");
        
        // Adiciona o token de autenticação no header (Bearer Token)
        _httpClient.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", _settings.Talk2ApiToken);
        
        // Indica que queremos receber JSON como resposta
        _httpClient.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
    }

    /// <summary>
    /// Busca os dados de um chat específico na API do Talk
    /// </summary>
    /// <param name="chatId">ID do chat (ex: "aPZ-CitJaYbmlVCa")</param>
    /// <returns>Objeto Chat com todos os dados, ou null se não encontrar</returns>
    public async Task<Chat?> GetChatAsync(string chatId)
    {
        try
        {
            // Monta a URL da API: /v1/chats/{chatId}?organizationId={orgId}
            var response = await _httpClient.GetAsync(
                $"v1/chats/{chatId}?organizationId={_settings.OrganizationId}");
            
            // Se a requisição falhou, loga o erro e retorna null
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Erro na API Talk: {response.StatusCode} - {errorContent}");
                return null;
            }
            
            // Lê o JSON da resposta e converte para o objeto Chat
            var content = await response.Content.ReadAsStringAsync();
            var chat = JsonSerializer.Deserialize<Chat>(content, _options);
            
            return chat;
        }
        catch (Exception ex)
        {
            // Em caso de erro, loga e relança a exceção
            Console.WriteLine($"Erro ao chamar API Talk: {ex.Message}");
            throw;
        }
    }
}
