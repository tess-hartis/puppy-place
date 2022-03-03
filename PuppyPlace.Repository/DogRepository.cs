using LanguageExt;
using LanguageExt.SomeHelp;
using Microsoft.EntityFrameworkCore;
using PuppyPlace.Data;
using PuppyPlace.Domain;

namespace PuppyPlace.Repository;

public interface IDogRepository : IGenericRepository<Dog>
{
    new Task<Option<Dog>> FindAsync(Guid id);
    Task<IEnumerable<Dog>> GetAll();
}

public class DogRepository : GenericRepository<Dog>, IDogRepository
{

    public DogRepository(PuppyPlaceContext context) : base(context)
    {

    }
    
    public async Task<IEnumerable<Dog>> GetAll()
    {
        return await Context.Dogs
            .Include(d => d.Owners)
            .ToListAsync();
    }
    
    public override async Task<Option<Dog>> FindAsync(Guid id)
    {
        var dog = await Context.Dogs
            .Include(d => d.Owners)
            .FirstOrDefaultAsync(d => d.Id == id);

        if (dog == null)
            return Option<Dog>.None;

        return dog.ToSome();
    }
}