using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using REVOPS.DevChallenge.Clients;
using REVOPS.DevChallenge.Clients.Models;
using REVOPS.DevChallenge.Context;
using REVOPS.DevChallenge.Context.Entities;

namespace REVOPS.DevChallenge.Services;

public class ChatInfoService : IChatInfoService
{
    private readonly ITalkClient _talkClient;
    private readonly ChallengeContext _context;

    public ChatInfoService(ITalkClient talkClient, ChallengeContext context)
    {
        _talkClient = talkClient;
        _context = context;
    }

    public async Task<Chat?> GetChatFromApiAsync(string chatId)
    {
        return await _talkClient.GetChatAsync(chatId);
    }

    public async Task<ChatInfoRecord?> GetChatInfoAsync(string chatId)
    {
        var chat = await _talkClient.GetChatAsync(chatId);
        
        if (chat == null)
            return null;

        var record = MapChatToRecord(chat);
        
        await _context.ChatInfoRecords.AddAsync(record);
        await _context.SaveChangesAsync();
        
        return record;
    }

    public async Task<List<ChatInfoRecord>> GetHistoryAsync(int limit = 50)
    {
        return await _context.ChatInfoRecords
            .OrderByDescending(r => r.SearchedAtUTC)
            .Take(limit)
            .ToListAsync();
    }

    private ChatInfoRecord MapChatToRecord(Chat chat)
    {
        var record = new ChatInfoRecord
        {
            ChatId = chat.Id,
            SearchedAtUTC = DateTime.UtcNow,
            
            // Attendant info
            IsAnyAttendantAssigned = chat.OrganizationMember != null,
            AssignedMemberId = chat.OrganizationMember?.Id,
            
            // Chat status
            IsOpen = chat.Open,
            IsWaiting = chat.Waiting,
            WaitingSinceUTC = chat.WaitingSinceUTC,
            ChatCreatedAtUTC = chat.CreatedAtUTC,
            ClosedAtUTC = chat.ClosedAtUTC,
            
            // Counts
            TotalUnread = chat.TotalUnread ?? 0,
            TotalAIResponses = chat.TotalAIResponses ?? 0
        };

        // Contact info
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

        // Channel
        if (chat.Channel != null)
        {
            record.ChannelId = chat.Channel.Id;
            record.ChannelName = chat.Channel.Name;
        }

        // Sector
        if (chat.Sector != null)
        {
            record.SectorId = chat.Sector.Id;
            record.SectorName = chat.Sector.Name;
        }

        // Chat tags
        if (chat.Tags?.Any() == true)
        {
            record.ChatTags = JsonSerializer.Serialize(
                chat.Tags.Select(t => new { t.Name, t.Emoji, t.Color }));
        }

        // Last message
        if (chat.LastMessage != null)
        {
            record.LastMessageContent = chat.LastMessage.Content;
            record.LastMessageAtUTC = chat.LastMessage.EventAtUTC;
            record.LastMessageSource = chat.LastMessage.Source;
        }

        // Active bot
        var activeBot = chat.Bots?.FirstOrDefault(b => b.Status == "Waiting" || b.Status == "Running");
        if (activeBot != null)
        {
            record.ActiveBotName = activeBot.BotTitle;
            record.ActiveBotStatus = activeBot.Status;
        }

        return record;
    }
}
