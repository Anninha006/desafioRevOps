namespace REVOPS.DevChallenge.Clients.Models;

public class Contact
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? PhoneNumber { get; set; }
    public string? ProfilePictureUrl { get; set; }
    public DateTime? LastActiveUTC { get; set; }
    public bool? IsBlocked { get; set; }
    public string? ContactType { get; set; }
    public List<Tag>? Tags { get; set; }
}

public class Tag
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? Emoji { get; set; }
    public string? Color { get; set; }
    public string? Description { get; set; }
}

public class Channel
{
    public string? Id { get; set; }
    public string? Name { get; set; }
}

public class Sector
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public bool? Default { get; set; }
}

public class LastMessage
{
    public string? Id { get; set; }
    public DateTime? CreatedAtUTC { get; set; }
    public string? Content { get; set; }
    public string? MessageType { get; set; }
    public string? Source { get; set; }
    public DateTime? EventAtUTC { get; set; }
}

public class BotInfo
{
    public string? Status { get; set; }
    public string? BotId { get; set; }
    public string? BotTitle { get; set; }
    public string? BotInstanceId { get; set; }
    public string? TriggerName { get; set; }
}
