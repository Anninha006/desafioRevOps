/*
 * ============================================================
 * INTERFACE DO SERVIÇO DE CHAT
 * ============================================================
 * Define o "contrato" do que o serviço de chat precisa fazer.
 * Usar interface permite trocar a implementação facilmente
 * (útil para testes ou futuras mudanças).
 * ============================================================
 */

using REVOPS.DevChallenge.Clients.Models;
using REVOPS.DevChallenge.Context.Entities;

namespace REVOPS.DevChallenge.Services;

public interface IChatInfoService
{
    /// <summary>
    /// Busca informações do chat na API do Talk E salva no banco de dados.
    /// Use esta função quando quiser registrar a pesquisa no histórico.
    /// </summary>
    /// <param name="chatId">O ID do chat (algo como "aPZ-CitJaYbmlVCa")</param>
    /// <returns>O registro salvo, ou null se o chat não existir</returns>
    Task<ChatInfoRecord?> GetChatInfoAsync(string chatId);
    
    /// <summary>
    /// Recupera o histórico de todas as pesquisas anteriores.
    /// Ordenado do mais recente para o mais antigo.
    /// </summary>
    /// <param name="limit">Quantidade máxima de registros (padrão: 50)</param>
    Task<List<ChatInfoRecord>> GetHistoryAsync(int limit = 50);
    
    /// <summary>
    /// Busca os dados "crus" do chat direto da API, SEM salvar no banco.
    /// Útil quando você quer apenas obter os dados sem registrar no histórico.
    /// </summary>
    Task<Chat?> GetChatFromApiAsync(string chatId);
}
