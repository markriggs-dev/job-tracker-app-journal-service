using JournalService.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace JournalService.Infrastructure.Data;

public class JournalServiceDbContext : DbContext
{
    public JournalServiceDbContext(DbContextOptions<JournalServiceDbContext> options)
        : base(options)
    {
    }

    public DbSet<JournalEntry> JournalEntries => Set<JournalEntry>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<JournalEntry>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.UserId)
                .IsRequired()
                .HasMaxLength(256);

            entity.Property(e => e.JobRequisitionId)
                .IsRequired();

            entity.Property(e => e.InteractionType)
                .HasConversion<string>()
                .HasMaxLength(64)
                .IsRequired();

            entity.Property(e => e.Notes)
                .HasMaxLength(4000);

            entity.Property(e => e.EntryDate)
                .IsRequired();

            entity.Property(e => e.CreatedAt)
                .IsRequired();

            entity.Property(e => e.UpdatedAt)
                .IsRequired();

            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => new { e.JobRequisitionId, e.UserId });

            entity.ToTable("journal_entries");
        });
    }
}
