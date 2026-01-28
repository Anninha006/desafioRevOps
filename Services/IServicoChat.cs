/*
 * ============================================================
 * INTERFACE DO SERVIÇO DE CHAT
 * ============================================================
 * Define o "contrato" do que o serviço de chat precisa fazer.
 * ============================================================
 */

using REVOPS.DevChallenge.Clients.Models;
using REVOPS.DevChallenge.Context.Entities;

namespace REVOPS.DevChallenge.Services;

public interface IServicoChat
{
    /// <summary>
    /// Busca informações do chat na API do Talk E salva no banco de dados.
    /// </summary>
    Task<RegistroDeChat?> GetChatInfoAsync(string chatId);
    
    /// <summary>
    /// Recupera o histórico de todas as pesquisas anteriores.
    /// </summary>
    Task<List<RegistroDeChat>> GetHistoryAsync(int limit = 50);
    
    /// <summary>
    /// Busca os dados "crus" do chat direto da API, SEM salvar no banco.
    /// </summary>
    Task<Chat?> GetChatFromApiAsync(string chatId);
}
