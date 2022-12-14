using Microsoft.EntityFrameworkCore;

namespace SecretaryProblem.Data;

public class EnvironmentContext: DbContext
{
    public DbSet<Contender> Contenders { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        const string connectionString = @"Server=localhost;Database=SecretaryProblem;
            User Id=postgres;Password=password";

        optionsBuilder.UseNpgsql(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Contender>().ToTable("contenders");
        
        modelBuilder.Entity<Contender>()
            .Property(c => c.TryId).HasColumnName("try_id");
        modelBuilder.Entity<Contender>()
            .Property(c => c.SequenceNumber).HasColumnName("sequence_number");
        modelBuilder.Entity<Contender>()
            .Property(c => c.Name).HasColumnName("name");
        modelBuilder.Entity<Contender>()
            .Property(c => c.Rating).HasColumnName("rating");
        
        modelBuilder.Entity<Contender>()
            .HasKey(c => new { c.TryId, c.SequenceNumber });
        
    }
}