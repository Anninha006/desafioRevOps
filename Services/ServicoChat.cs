/*
 * ============================================================
 * SERVIÇO DE INFORMAÇÕES DE CHAT
 * ============================================================
 */

using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using REVOPS.DevChallenge.Clients;
using REVOPS.DevChallenge.Clients.Models;
using REVOPS.DevChallenge.Context;
using REVOPS.DevChallenge.Context.Entities;

namespace REVOPS.DevChallenge.Services;

public class ServicoChat : IServicoChat
{
    private readonly IClienteTalk _ClienteTalk;
    private readonly ContextoBanco _context;

    public ServicoChat(IClienteTalk ClienteTalk, ContextoBanco context)
    {
        _ClienteTalk = ClienteTalk;
        _context = context;
    }

    public async Task<Chat?> GetChatFromApiAsync(string chatId)
    {
        return await _ClienteTalk.GetChatAsync(chatId);
    }

    public async Task<RegistroDeChat?> GetChatInfoAsync(string chatId)
    {
        var chat = await _ClienteTalk.GetChatAsync(chatId);
        
        if (chat == null)
            return null;

        var record = MapChatToRecord(chat);
        
        await _context.RegistroDeChats.AddAsync(record);
        await _context.SaveChangesAsync();
        
        return record;
    }

    public async Task<List<RegistroDeChat>> GetHistoryAsync(int limit = 50)
    {
        return await _context.RegistroDeChats
            .OrderByDescending(r => r.SearchedAtUTC)
            .Take(limit)
            .ToListAsync();
    }

    private RegistroDeChat MapChatToRecord(Chat chat)
    {
        var record = new RegistroDeChat
        {
            ChatId = chat.Id,
            SearchedAtUTC = DateTime.UtcNow,
            IsAnyAttendantAssigned = chat.OrganizationMember != null,
            AssignedMemberId = chat.OrganizationMember?.Id,
            IsOpen = chat.Open,
            IsWaiting = chat.Waiting,
            WaitingSinceUTC = chat.WaitingSinceUTC,
            ChatCreatedAtUTC = chat.CreatedAtUTC,
            ClosedAtUTC = chat.ClosedAtUTC,
            TotalUnread = chat.TotalUnread ?? 0,
            TotalAIResponses = chat.TotalAIResponses ?? 0
        };

        if (chat.Contact != null)
        {
            record.ContactId = chat.Contact.Id;
            record.ContactName = chat.Contact.Name;
            record.ContactPhoneNumber = chat.Contact.PhoneNumber;
            record.ContactProfilePictureUrl = chat.Contact.ProfilePictureUrl;
            record.ContactIsBlocked = chat.Contact.IsBlocked ?? false;
            
            if (chat.Contact.Tags?.Any() == true)
            {
                record.ContactTags = JsonSerializer.Serialize(
                    chat.Contact.Tags.Select(t => new { t.Name, t.Emoji, t.Color }));
            }
        }

        if (chat.Channel != null)
        {
            record.ChannelId = chat.Channel.Id;
            record.ChannelName = chat.Channel.Name;
        }

        if (chat.Sector != null)
        {
            record.SectorId = chat.Sector.Id;
            record.SectorName = chat.Sector.Name;
        }

        if (chat.Tags?.Any() == true)
        {
            record.ChatTags = JsonSerializer.Serialize(
                chat.Tags.Select(t => new { t.Name, t.Emoji, t.Color }));
        }

        if (chat.LastMessage != null)
        {
            record.LastMessageContent = chat.LastMessage.Content;
            record.LastMessageAtUTC = chat.LastMessage.EventAtUTC;
            record.LastMessageSource = chat.LastMessage.Source;
        }

        var activeBot = chat.Bots?.FirstOrDefault(b => b.Status == "Waiting" || b.Status == "Running");
        if (activeBot != null)
        {
            record.ActiveBotName = activeBot.BotTitle;
            record.ActiveBotStatus = activeBot.Status;
        }

        return record;
    }
}
