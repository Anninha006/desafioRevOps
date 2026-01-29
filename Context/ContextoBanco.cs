/*
 * ============================================================
 * CONTEXTO DO BANCO DE DADOS (Entity Framework Core)
 * ============================================================
 * O DbContext é a "ponte" entre o código C# e o banco de dados.
 * Ele permite consultar e salvar dados usando objetos normais
 * em vez de escrever SQL manualmente.
 * 
 * O banco usado é SQLite (arquivo local ChallengeDatabase.db)
 * ============================================================
 */

using Microsoft.EntityFrameworkCore;
using REVOPS.DevChallenge.Context.Entities;

namespace REVOPS.DevChallenge.Context;

public class ChallengeContext : DbContext
{
    // ====================================================
    // TABELAS DO BANCO DE DADOS
    // Cada DbSet representa uma tabela no banco
    // ====================================================
    
    /// <summary>
    /// Tabela legada (mantida para compatibilidade)
    /// </summary>
    public DbSet<ChatInfo> ChatInfos { get; private set; }
    
    /// <summary>
    /// Tabela principal - armazena o histórico de pesquisas de chat
    /// </summary>
    public DbSet<ChatInfoRecord> ChatInfoRecords { get; private set; }

    // Construtor recebe as opções de conexão do framework
    public ChallengeContext(DbContextOptions<ChallengeContext> options) : base(options) { }
    
    /// <summary>
    /// Configurações extras das tabelas (índices, chaves, etc)
    /// Executado quando o banco é criado/migrado
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Configura a entidade ChatInfoRecord
        modelBuilder.Entity<ChatInfoRecord>(entity =>
        {
            entity.HasKey(e => e.Id);           // Define a chave primária
            entity.HasIndex(e => e.ChatId);     // Índice para buscar por ChatId
            entity.HasIndex(e => e.SearchedAtUTC);  // Índice para ordenar por data
        });
    }
}
