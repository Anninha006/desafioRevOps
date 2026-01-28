namespace REVOPS.DevChallenge.Clients.Models;

public class TalkApiResponse<T>
{
    public PageInfo? Page { get; set; }
    public List<T>? Items { get; set; }
}

public class PageInfo
{
    public int TotalItems { get; set; }
    public int Skipped { get; set; }
    public int Took { get; set; }
}
