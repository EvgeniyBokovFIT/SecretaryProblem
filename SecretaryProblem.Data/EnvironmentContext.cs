using HostedServiceAndDI.Entity;
using Microsoft.EntityFrameworkCore;

namespace SecretaryProblem.Data;

public class EnvironmentContext: DbContext
{
    public DbSet<Contender> Contenders { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString =
            @"Server=localhost;Database=SecretaryProblem;
            User Id=postgres;Password=password";
        optionsBuilder.UseNpgsql(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Contender>()
            .HasKey(c => new { c.TryId, c.SequenceNumber });
    }
}