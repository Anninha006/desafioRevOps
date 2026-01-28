using REVOPS.DevChallenge.Clients.Models;
using REVOPS.DevChallenge.Context.Entities;

namespace REVOPS.DevChallenge.Services;

public interface IChatInfoService
{
    /// <summary>
    /// Fetches chat information from Talk API and saves to database
    /// </summary>
    Task<ChatInfoRecord?> GetChatInfoAsync(string chatId);
    
    /// <summary>
    /// Gets all previously searched chats from the database
    /// </summary>
    Task<List<ChatInfoRecord>> GetHistoryAsync(int limit = 50);
    
    /// <summary>
    /// Gets the raw chat data from Talk API without saving
    /// </summary>
    Task<Chat?> GetChatFromApiAsync(string chatId);
}
