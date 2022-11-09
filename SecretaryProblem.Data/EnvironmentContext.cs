using Microsoft.EntityFrameworkCore;

namespace SecretaryProblem.Data;

public class EnvironmentContext: DbContext
{
    public DbSet<DbContender> DbContenders { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString =
            @"Server=localhost;Database=SecretaryProblem;
            User Id=postgres;Password=password";
        
        optionsBuilder.UseNpgsql(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DbContender>()
            .HasKey(c => new { c.TryId, c.SequenceNumber });
        
    }
}