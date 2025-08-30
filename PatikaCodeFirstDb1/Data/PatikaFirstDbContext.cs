using Microsoft.EntityFrameworkCore;
using PatikaCodeFirstDb1.Models;

namespace PatikaCodeFirstDb1.Data;

public class PatikaFirstDbContext : DbContext
{
    public DbSet<Movie> Movies => Set<Movie>();
    public DbSet<Game> Games => Set<Game>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            // VS ile gelen LocalDB
            optionsBuilder.UseSqlServer(
                @"Server=(localdb)\MSSQLLocalDB;Database=PatikaCodeFirstDb1;Trusted_Connection=True;TrustServerCertificate=True;");
            // Eğer SQL Express kullanıyorsan şunu kullan:
            // optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=PatikaCodeFirstDb1;Trusted_Connection=True;TrustServerCertificate=True;");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Movie>(e =>
        {
            e.ToTable("Movies");
            e.HasKey(x => x.Id);
            e.Property(x => x.Title).IsRequired().HasMaxLength(200);
            e.Property(x => x.Genre).IsRequired().HasMaxLength(50);
            e.Property(x => x.ReleaseYear).IsRequired();
        });

        modelBuilder.Entity<Game>(e =>
        {
            e.ToTable("Games");
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).IsRequired().HasMaxLength(150);
            e.Property(x => x.Platform).IsRequired().HasMaxLength(50);
            e.Property(x => x.Rating).IsRequired().HasPrecision(3, 1);
            e.HasCheckConstraint("CK_Games_Rating_0_10", "[Rating] >= 0 AND [Rating] <= 10");
        });
    }
}
