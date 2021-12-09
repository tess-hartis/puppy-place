using Microsoft.EntityFrameworkCore;

namespace PuppyPlace.Data;

public class PuppyPlaceContext : DbContext
{
    public DbSet<Person> Persons { get; set; }
    public DbSet<Dog> Dogs { get; set; }

    public PuppyPlaceContext(DbContextOptions<PuppyPlaceContext> options) : base(options)
    {
        
    }
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseNpgsql("Host=localhost;Username=itb;Database=PuppyPlace");
    
}