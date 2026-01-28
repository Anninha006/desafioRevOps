using Microsoft.EntityFrameworkCore;
using REVOPS.DevChallenge.Context.Entities;

namespace REVOPS.DevChallenge.Context;

public class ChallengeContext : DbContext
{
    public DbSet<ChatInfo> ChatInfos { get; private set; }
    public DbSet<ChatInfoRecord> ChatInfoRecords { get; private set; }

    public ChallengeContext(DbContextOptions<ChallengeContext> options) : base(options) { }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<ChatInfoRecord>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.ChatId);
            entity.HasIndex(e => e.SearchedAtUTC);
        });
    }
}
