/*
 * ============================================================
 * MODELOS DE DADOS DA API TALK - ENTIDADES AUXILIARES
 * ============================================================
 * Contato, Canal, Setor, Tags, Mensagens e Bots
 * ============================================================
 */

namespace REVOPS.DevChallenge.Clients.Models;

/// <summary>
/// Contato/Cliente do chat
/// </summary>
public class Contact
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? PhoneNumber { get; set; }
    public string? ProfilePictureUrl { get; set; }  // Foto do perfil
    public DateTime? LastActiveUTC { get; set; }
    public bool? IsBlocked { get; set; }
    public string? ContactType { get; set; }
    public List<Tag>? Tags { get; set; }
}

/// <summary>
/// Etiqueta/Tag para categorização
/// </summary>
public class Tag
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? Emoji { get; set; }
    public string? Color { get; set; }
    public string? Description { get; set; }
}

/// <summary>
/// Canal de comunicação (WhatsApp, Instagram, etc)
/// </summary>
public class Channel
{
    public string? Id { get; set; }
    public string? Name { get; set; }
}

/// <summary>
/// Setor de atendimento
/// </summary>
public class Sector
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public bool? Default { get; set; }
}

/// <summary>
/// Última mensagem do chat
/// </summary>
public class LastMessage
{
    public string? Id { get; set; }
    public DateTime? CreatedAtUTC { get; set; }
    public string? Content { get; set; }
    public string? MessageType { get; set; }
    public string? Source { get; set; }       // "Contact" = cliente, "Team" = atendente
    public DateTime? EventAtUTC { get; set; }
}

/// <summary>
/// Informações de um bot ativo no chat
/// </summary>
public class BotInfo
{
    public string? Status { get; set; }       // "Waiting", "Running", etc
    public string? BotId { get; set; }
    public string? BotTitle { get; set; }
    public string? BotInstanceId { get; set; }
    public string? TriggerName { get; set; }
}
