using Microsoft.EntityFrameworkCore;
using WinTrayMemory.Data.Entities;

namespace WinTrayMemory.Data.Persistence;

public class WinTrayMemoryDbContext : DbContext
{
    public DbSet<AppSettings> AppSettings { get; set; } = null!;
    public DbSet<ProcessRule> ProcessRules { get; set; } = null!;

    public WinTrayMemoryDbContext(DbContextOptions<WinTrayMemoryDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AppSettings>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Threshold).HasDefaultValue(60);
            entity.Property(e => e.MinProcessSize).HasDefaultValue(500);
            entity.Property(e => e.MaxProcessesShown).HasDefaultValue(15);
            entity.Property(e => e.RefreshInterval).HasDefaultValue(3);
        });

        modelBuilder.Entity<ProcessRule>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Name).IsUnique();
            entity.Property(e => e.Category).HasConversion<string>();
        });

        base.OnModelCreating(modelBuilder);
    }
}
