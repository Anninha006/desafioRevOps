/*
 * ============================================================
 * ENTIDADES DO BANCO DE DADOS
 * ============================================================
 * Estas classes representam as tabelas do banco de dados.
 * Cada propriedade = uma coluna na tabela.
 * ============================================================
 */

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace REVOPS.DevChallenge.Context.Entities;

/// <summary>
/// Registro de uma pesquisa de chat - esta é a tabela PRINCIPAL
/// Cada vez que você pesquisa um chat, um novo registro é criado aqui
/// </summary>
public class ChatInfoRecord
{
    // ====================================================
    // IDENTIFICAÇÃO
    // ====================================================
    
    /// <summary>
    /// ID único no banco (gerado automaticamente)
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    /// <summary>
    /// ID do chat na API do Talk (ex: "aPZ-CitJaYbmlVCa")
    /// </summary>
    public string? ChatId { get; set; }
    
    /// <summary>
    /// Data/hora em que a pesquisa foi realizada
    /// </summary>
    public DateTime SearchedAtUTC { get; set; } = DateTime.UtcNow;
    
    // ====================================================
    // ATENDENTE
    // ====================================================
    
    /// <summary>
    /// Se tem algum atendente atribuído ao chat
    /// </summary>
    public bool IsAnyAttendantAssigned { get; set; }
    
    /// <summary>
    /// ID do atendente (se houver)
    /// </summary>
    public string? AssignedMemberId { get; set; }
    
    // ====================================================
    // STATUS DO CHAT
    // ====================================================
    
    /// <summary>
    /// Se o chat está aberto (true) ou fechado (false)
    /// </summary>
    public bool IsOpen { get; set; }
    
    /// <summary>
    /// Se está aguardando atendimento
    /// </summary>
    public bool IsWaiting { get; set; }
    
    public DateTime? WaitingSinceUTC { get; set; }
    public DateTime? ChatCreatedAtUTC { get; set; }
    public DateTime? ClosedAtUTC { get; set; }
    
    // ====================================================
    // INFORMAÇÕES DO CONTATO (CLIENTE)
    // ====================================================
    
    public string? ContactId { get; set; }
    public string? ContactName { get; set; }
    public string? ContactPhoneNumber { get; set; }
    public string? ContactProfilePictureUrl { get; set; }
    public bool ContactIsBlocked { get; set; }
    
    // ====================================================
    // CANAL E SETOR
    // ====================================================
    
    /// <summary>
    /// Canal de origem (ex: WhatsApp, Instagram)
    /// </summary>
    public string? ChannelId { get; set; }
    public string? ChannelName { get; set; }
    
    /// <summary>
    /// Setor de atendimento
    /// </summary>
    public string? SectorId { get; set; }
    public string? SectorName { get; set; }
    
    // ====================================================
    // TAGS (etiquetas) - Armazenadas como JSON
    // ====================================================
    
    public string? ChatTags { get; set; }
    public string? ContactTags { get; set; }
    
    // ====================================================
    // CONTADORES
    // ====================================================
    
    public int TotalUnread { get; set; }
    public int TotalAIResponses { get; set; }
    
    // ====================================================
    // ÚLTIMA MENSAGEM
    // ====================================================
    
    public string? LastMessageContent { get; set; }
    public DateTime? LastMessageAtUTC { get; set; }
    public string? LastMessageSource { get; set; }  // "Contact" ou "Team"
    
    // ====================================================
    // BOT ATIVO
    // ====================================================
    
    public string? ActiveBotName { get; set; }
    public string? ActiveBotStatus { get; set; }
    
    // ====================================================
    // PROPRIEDADE CALCULADA (não salva no banco)
    // ====================================================
    
    /// <summary>
    /// Tempo de espera em minutos (calculado, não armazenado)
    /// </summary>
    [NotMapped]
    public double? WaitingTimeMinutes => WaitingSinceUTC.HasValue 
        ? (SearchedAtUTC - WaitingSinceUTC.Value).TotalMinutes 
        : null;
}

/// <summary>
/// Entidade legada - mantida apenas para compatibilidade com código antigo
/// </summary>
public class ChatInfo
{
    [Key]
    public string? ChatId { get; set; }
    public bool IsAnyAttendantAssigned { get; set; }
}
