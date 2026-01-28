/*
 * ============================================================
 * CONTEXTO DO BANCO DE DADOS (Entity Framework Core)
 * ============================================================
 */

using Microsoft.EntityFrameworkCore;
using REVOPS.DevChallenge.Context.Entities;

namespace REVOPS.DevChallenge.Context;

public class ContextoBanco : DbContext
{
    public DbSet<ChatInfo> ChatInfos { get; private set; }
    public DbSet<RegistroDeChat> RegistroDeChats { get; private set; }

    public ContextoBanco(DbContextOptions<ContextoBanco> options) : base(options) { }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<RegistroDeChat>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.ChatId);
            entity.HasIndex(e => e.SearchedAtUTC);
        });
    }
}
