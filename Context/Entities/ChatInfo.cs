using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace REVOPS.DevChallenge.Context.Entities;

/// <summary>
/// Entity for storing chat search history with rich information
/// </summary>
public class ChatInfoRecord
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    /// <summary>
    /// The Talk chat ID
    /// </summary>
    public string? ChatId { get; set; }
    
    /// <summary>
    /// When this search was performed
    /// </summary>
    public DateTime SearchedAtUTC { get; set; } = DateTime.UtcNow;
    
    // Attendant info
    public bool IsAnyAttendantAssigned { get; set; }
    public string? AssignedMemberId { get; set; }
    
    // Chat status
    public bool IsOpen { get; set; }
    public bool IsWaiting { get; set; }
    public DateTime? WaitingSinceUTC { get; set; }
    public DateTime? ChatCreatedAtUTC { get; set; }
    public DateTime? ClosedAtUTC { get; set; }
    
    // Contact info
    public string? ContactId { get; set; }
    public string? ContactName { get; set; }
    public string? ContactPhoneNumber { get; set; }
    public string? ContactProfilePictureUrl { get; set; }
    public bool ContactIsBlocked { get; set; }
    
    // Channel and Sector
    public string? ChannelId { get; set; }
    public string? ChannelName { get; set; }
    public string? SectorId { get; set; }
    public string? SectorName { get; set; }
    
    // Tags (stored as JSON)
    public string? ChatTags { get; set; }
    public string? ContactTags { get; set; }
    
    // Counts
    public int TotalUnread { get; set; }
    public int TotalAIResponses { get; set; }
    
    // Last message info
    public string? LastMessageContent { get; set; }
    public DateTime? LastMessageAtUTC { get; set; }
    public string? LastMessageSource { get; set; }
    
    // Bot info
    public string? ActiveBotName { get; set; }
    public string? ActiveBotStatus { get; set; }
    
    /// <summary>
    /// Calculated waiting time in minutes when searched
    /// </summary>
    [NotMapped]
    public double? WaitingTimeMinutes => WaitingSinceUTC.HasValue 
        ? (SearchedAtUTC - WaitingSinceUTC.Value).TotalMinutes 
        : null;
}

/// <summary>
/// Legacy entity - kept for backward compatibility
/// </summary>
public class ChatInfo
{
    [Key]
    public string? ChatId { get; set; }
    public bool IsAnyAttendantAssigned { get; set; }
}
