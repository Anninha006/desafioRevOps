/*
 * ============================================================
 * ENTIDADES DO BANCO DE DADOS
 * ============================================================
 */

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace REVOPS.DevChallenge.Context.Entities;

/// <summary>
/// Registro de uma pesquisa de chat - esta é a tabela PRINCIPAL
/// </summary>
[Table("RegistroDeChats")] // Mantém o nome da tabela original no banco
public class RegistroDeChat
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    public string? ChatId { get; set; }
    public DateTime SearchedAtUTC { get; set; } = DateTime.UtcNow;
    
    public bool IsAnyAttendantAssigned { get; set; }
    public string? AssignedMemberId { get; set; }
    
    public bool IsOpen { get; set; }
    public bool IsWaiting { get; set; }
    public DateTime? WaitingSinceUTC { get; set; }
    public DateTime? ChatCreatedAtUTC { get; set; }
    public DateTime? ClosedAtUTC { get; set; }
    
    public string? ContactId { get; set; }
    public string? ContactName { get; set; }
    public string? ContactPhoneNumber { get; set; }
    public string? ContactProfilePictureUrl { get; set; }
    public bool ContactIsBlocked { get; set; }
    
    public string? ChannelId { get; set; }
    public string? ChannelName { get; set; }
    public string? SectorId { get; set; }
    public string? SectorName { get; set; }
    
    public string? ChatTags { get; set; }
    public string? ContactTags { get; set; }
    
    public int TotalUnread { get; set; }
    public int TotalAIResponses { get; set; }
    
    public string? LastMessageContent { get; set; }
    public DateTime? LastMessageAtUTC { get; set; }
    public string? LastMessageSource { get; set; }  // "Contact" ou "Team"
    
    public string? ActiveBotName { get; set; }
    public string? ActiveBotStatus { get; set; }
    
    [NotMapped]
    public double? WaitingTimeMinutes => WaitingSinceUTC.HasValue 
        ? (SearchedAtUTC - WaitingSinceUTC.Value).TotalMinutes 
        : null;
}

/// <summary>
/// Entidade legada - mantida apenas para compatibilidade
/// </summary>
[Table("ChatInfos")]
public class ChatInfo
{
    [Key]
    public string? ChatId { get; set; }
    public bool IsAnyAttendantAssigned { get; set; }
}
