using Microsoft.EntityFrameworkCore;
using PuppyPlace.Domain;

namespace PuppyPlace.Data;

public class PuppyPlaceContext : DbContext
{
    public DbSet<Person> Persons { get; set; }
    public DbSet<Dog> Dogs { get; set; }
   
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseNpgsql("Host=localhost;Username=itb;Password=itb;Database=PuppyPlace");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>()
            .OwnsOne(p => p.Name)
            .Property(x => x.Value)
            .HasColumnName("Name");

        modelBuilder.Entity<Dog>()
            .OwnsOne(d => d.Name)
            .Property(x => x.Value)
            .HasColumnName("Name");

        modelBuilder.Entity<Dog>()
            .OwnsOne(d => d.Age)
            .Property(x => x.Value)
            .HasColumnName("Age");

        modelBuilder.Entity<Dog>()
            .OwnsOne(d => d.Breed)
            .Property(x => x.Value)
            .HasColumnName("Breed");
    }
    
    
}