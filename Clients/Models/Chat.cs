namespace REVOPS.DevChallenge.Clients.Models;

public class Chat
{
    public string? Id { get; set; }
    public DateTime? CreatedAtUTC { get; set; }
    
    // Flags de status
    public bool Open { get; set; }
    public bool Private { get; set; }
    public bool Waiting { get; set; }
    public DateTime? WaitingSinceUTC { get; set; }
    public DateTime? ClosedAtUTC { get; set; }
    
    // Contadores - nullable para tratar retorno nulo da API
    public int? TotalUnread { get; set; }
    public int? TotalAIResponses { get; set; }
    
    // Flags de fluxo
    public bool UsingInactivityFlow { get; set; }
    public bool UsingWaitingFlow { get; set; }
    
    // Entidades relacionadas
    public Contact? Contact { get; set; }
    public Channel? Channel { get; set; }
    public Sector? Sector { get; set; }
    public OrganizationMember? OrganizationMember { get; set; }
    public List<OrganizationMember>? OrganizationMembers { get; set; }
    public List<Tag>? Tags { get; set; }
    public LastMessage? LastMessage { get; set; }
    public List<BotInfo>? Bots { get; set; }
    
    // Timestamps
    public DateTime? EventAtUTC { get; set; }
}

public class OrganizationMember
{
    public string? Id { get; set; }
    public bool? Muted { get; set; }
    public int? TotalUnread { get; set; }
    public string? Name { get; set; } // Nome do membro (se disponível)
}
