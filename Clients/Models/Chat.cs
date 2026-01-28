namespace REVOPS.DevChallenge.Clients.Models;

public class Chat
{
    public string? Id { get; set; }
    public DateTime? CreatedAtUTC { get; set; }
    
    // Status flags
    public bool Open { get; set; }
    public bool Private { get; set; }
    public bool Waiting { get; set; }
    public DateTime? WaitingSinceUTC { get; set; }
    public DateTime? ClosedAtUTC { get; set; }
    
    // Counts - nullable to handle API returning null
    public int? TotalUnread { get; set; }
    public int? TotalAIResponses { get; set; }
    
    // Flow flags
    public bool UsingInactivityFlow { get; set; }
    public bool UsingWaitingFlow { get; set; }
    
    // Related entities
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
}
