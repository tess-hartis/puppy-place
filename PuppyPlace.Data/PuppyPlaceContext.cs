using Microsoft.EntityFrameworkCore;
using PuppyPlace.Domain;

namespace PuppyPlace.Data;

public class PuppyPlaceContext : DbContext
{
    public DbSet<Person> Persons { get; set; }
    public DbSet<Dog> Dogs { get; set; }
    //
    // public PuppyPlaceContext(DbContextOptions<PuppyPlaceContext> options) : base(options)
    // {
    //     
    // }
    //
    // public PuppyPlaceContext()
    // {
    //     
    // }
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseNpgsql("Host=localhost;Username=itb;Password=itb;Database=PuppyPlace");
    
}