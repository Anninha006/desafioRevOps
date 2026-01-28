/*
 * ============================================================
 * SERVIÇO DE INFORMAÇÕES DE CHAT
 * ============================================================
 * Este serviço é responsável por:
 * 1. Buscar informações de chat na API do Talk
 * 2. Salvar o histórico de pesquisas no banco de dados
 * 3. Recuperar o histórico de pesquisas anteriores
 * ============================================================
 */

using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using REVOPS.DevChallenge.Clients;
using REVOPS.DevChallenge.Clients.Models;
using REVOPS.DevChallenge.Context;
using REVOPS.DevChallenge.Context.Entities;

namespace REVOPS.DevChallenge.Services;

public class ChatInfoService : IChatInfoService
{
    // Cliente para fazer requisições à API do Talk
    private readonly ITalkClient _talkClient;
    
    // Contexto do banco de dados (Entity Framework)
    private readonly ChallengeContext _context;

    // Injeção de dependências - o framework fornece automaticamente as instâncias
    public ChatInfoService(ITalkClient talkClient, ChallengeContext context)
    {
        _talkClient = talkClient;
        _context = context;
    }

    /// <summary>
    /// Busca os dados "crus" do chat direto da API, sem salvar no banco
    /// Útil quando queremos apenas espiar os dados
    /// </summary>
    public async Task<Chat?> GetChatFromApiAsync(string chatId)
    {
        return await _talkClient.GetChatAsync(chatId);
    }

    /// <summary>
    /// Busca as informações do chat na API E salva no histórico
    /// Esta é a função principal que a interface usa
    /// </summary>
    public async Task<ChatInfoRecord?> GetChatInfoAsync(string chatId)
    {
        // 1. Primeiro, buscamos os dados na API do Talk
        var chat = await _talkClient.GetChatAsync(chatId);
        
        // Se não encontrou, retorna nulo (a interface vai mostrar "não encontrado")
        if (chat == null)
            return null;

        // 2. Convertemos os dados da API para o nosso formato de banco de dados
        var record = MapChatToRecord(chat);
        
        // 3. Salvamos no banco de dados para ter histórico das pesquisas
        await _context.ChatInfoRecords.AddAsync(record);
        await _context.SaveChangesAsync();
        
        return record;
    }

    /// <summary>
    /// Recupera o histórico de pesquisas anteriores
    /// Ordenado do mais recente para o mais antigo
    /// </summary>
    public async Task<List<ChatInfoRecord>> GetHistoryAsync(int limit = 50)
    {
        return await _context.ChatInfoRecords
            .OrderByDescending(r => r.SearchedAtUTC)  // Mais recentes primeiro
            .Take(limit)                               // Limita a quantidade
            .ToListAsync();
    }

    /// <summary>
    /// Converte os dados que vieram da API para o formato do banco de dados
    /// Essa função "mapeia" um objeto para outro
    /// </summary>
    private ChatInfoRecord MapChatToRecord(Chat chat)
    {
        // Criamos o registro com as informações básicas
        var record = new ChatInfoRecord
        {
            ChatId = chat.Id,
            SearchedAtUTC = DateTime.UtcNow,  // Marca a hora atual da pesquisa
            
            // ---- Informações do atendente ----
            IsAnyAttendantAssigned = chat.OrganizationMember != null,
            AssignedMemberId = chat.OrganizationMember?.Id,
            
            // ---- Status do chat ----
            IsOpen = chat.Open,
            IsWaiting = chat.Waiting,
            WaitingSinceUTC = chat.WaitingSinceUTC,
            ChatCreatedAtUTC = chat.CreatedAtUTC,
            ClosedAtUTC = chat.ClosedAtUTC,
            
            // ---- Contadores ----
            TotalUnread = chat.TotalUnread ?? 0,      // Se for nulo, usa zero
            TotalAIResponses = chat.TotalAIResponses ?? 0
        };

        // ---- Informações do contato (cliente) ----
        if (chat.Contact != null)
        {
            record.ContactId = chat.Contact.Id;
            record.ContactName = chat.Contact.Name;
            record.ContactPhoneNumber = chat.Contact.PhoneNumber;
            record.ContactProfilePictureUrl = chat.Contact.ProfilePictureUrl;
            record.ContactIsBlocked = chat.Contact.IsBlocked ?? false;
            
            // Tags são salvas como JSON (texto) no banco
            if (chat.Contact.Tags?.Any() == true)
            {
                record.ContactTags = JsonSerializer.Serialize(
                    chat.Contact.Tags.Select(t => new { t.Name, t.Emoji, t.Color }));
            }
        }

        // ---- Canal de origem (WhatsApp, Instagram, etc) ----
        if (chat.Channel != null)
        {
            record.ChannelId = chat.Channel.Id;
            record.ChannelName = chat.Channel.Name;
        }

        // ---- Setor do atendimento ----
        if (chat.Sector != null)
        {
            record.SectorId = chat.Sector.Id;
            record.SectorName = chat.Sector.Name;
        }

        // ---- Tags do chat ----
        if (chat.Tags?.Any() == true)
        {
            record.ChatTags = JsonSerializer.Serialize(
                chat.Tags.Select(t => new { t.Name, t.Emoji, t.Color }));
        }

        // ---- Última mensagem do chat ----
        if (chat.LastMessage != null)
        {
            record.LastMessageContent = chat.LastMessage.Content;
            record.LastMessageAtUTC = chat.LastMessage.EventAtUTC;
            record.LastMessageSource = chat.LastMessage.Source;
        }

        // ---- Bot ativo (se houver algum em execução) ----
        var activeBot = chat.Bots?.FirstOrDefault(b => b.Status == "Waiting" || b.Status == "Running");
        if (activeBot != null)
        {
            record.ActiveBotName = activeBot.BotTitle;
            record.ActiveBotStatus = activeBot.Status;
        }

        return record;
    }
}
