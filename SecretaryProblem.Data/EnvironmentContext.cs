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
        modelBuilder.Entity<DbContender>().ToTable("contenders");
        
        modelBuilder.Entity<DbContender>()
            .Property(c => c.TryId).HasColumnName("try_id");
        modelBuilder.Entity<DbContender>()
            .Property(c => c.SequenceNumber).HasColumnName("sequence_number");
        modelBuilder.Entity<DbContender>()
            .Property(c => c.Name).HasColumnName("name");
        modelBuilder.Entity<DbContender>()
            .Property(c => c.Rating).HasColumnName("rating");
        
        modelBuilder.Entity<DbContender>()
            .HasKey(c => new { c.TryId, c.SequenceNumber });
        
    }
}