/*
 * ============================================================
 * MODELOS DE DADOS DA API TALK - CHAT
 * ============================================================
 * Estas classes representam a estrutura de dados que a API
 * do Talk retorna quando buscamos informações de um chat.
 * ============================================================
 */

namespace REVOPS.DevChallenge.Clients.Models;

/// <summary>
/// Representa um chat/conversa no Talk
/// </summary>
public class Chat
{
    // ---- Identificação ----
    public string? Id { get; set; }
    public DateTime? CreatedAtUTC { get; set; }
    
    // ---- Status do chat ----
    public bool Open { get; set; }      // Está aberto?
    public bool Private { get; set; }   // É privado?
    public bool Waiting { get; set; }   // Aguardando atendimento?
    public DateTime? WaitingSinceUTC { get; set; }
    public DateTime? ClosedAtUTC { get; set; }
    
    // ---- Contadores ----
    public int? TotalUnread { get; set; }       // Mensagens não lidas
    public int? TotalAIResponses { get; set; }  // Respostas da IA
    
    // ---- Fluxos automáticos ----
    public bool UsingInactivityFlow { get; set; }
    public bool UsingWaitingFlow { get; set; }
    
    // ---- Relacionamentos ----
    public Contact? Contact { get; set; }                    // Cliente
    public Channel? Channel { get; set; }                    // Canal (WhatsApp, etc)
    public Sector? Sector { get; set; }                      // Setor de atendimento
    public OrganizationMember? OrganizationMember { get; set; }  // Atendente principal
    public List<OrganizationMember>? OrganizationMembers { get; set; }  // Todos atendentes
    public List<Tag>? Tags { get; set; }                     // Etiquetas do chat
    public LastMessage? LastMessage { get; set; }            // Última mensagem
    public List<BotInfo>? Bots { get; set; }                 // Bots ativos
    
    // ---- Timestamps ----
    public DateTime? EventAtUTC { get; set; }
}

/// <summary>
/// Membro da organização (atendente)
/// </summary>
public class OrganizationMember
{
    public string? Id { get; set; }
    public bool? Muted { get; set; }
    public int? TotalUnread { get; set; }
    public string? Name { get; set; }
}
