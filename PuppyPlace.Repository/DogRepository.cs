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

    // public async Task AddDogAsync(Dog dog)
    // {
    //     await _context.Dogs.AddAsync(dog);
    //     await _context.SaveChangesAsync();
    // }
    //
    // public async Task<IReadOnlyList<Dog>> DogsAsync()
    // {
    //     return await _context.Dogs.ToListAsync();
    // }

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

//     public async Task RemoveDogAsync(Guid id)
//     {
//         var dog = await _context.Dogs.FirstOrDefaultAsync(d => d.Id == id);
//         _context.Dogs.Remove(dog);
//         await _context.SaveChangesAsync();
//     }
//
//     public async Task UpdateNameAsync(Dog dog)
//     {
//         _context.Dogs.Update(dog);
//         await _context.SaveChangesAsync();
//     }
// }