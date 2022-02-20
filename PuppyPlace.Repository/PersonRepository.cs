using LanguageExt;
using LanguageExt.SomeHelp;
using Microsoft.EntityFrameworkCore;
using PuppyPlace.Data;
using PuppyPlace.Domain;

namespace PuppyPlace.Repository;

public interface IPersonRepository : IGenericRepository<Person>
{
    new Task<Option<Person>> FindAsync(Guid id);
    Task<IEnumerable<Person>> GetAll();
}
public class PersonRepository : GenericRepository<Person>, IPersonRepository
{
    public PersonRepository(PuppyPlaceContext context) : base(context)
    {
        
    }

    public async Task<IEnumerable<Person>> GetAll()
    {
        return await Context.Persons
            .Include(p => p.Dogs)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Person>> PersonsAsync()
    {
       return await Context.Persons.ToListAsync();
    }

    public override async Task<Option<Person>> FindAsync(Guid id)
    {
        var person = await Context.Persons
            .Include(p => p.Dogs)
            .FirstOrDefaultAsync(p => p.Id == id);
        
        if (person == null)
            return Option<Person>.None;

        return person.ToSome();

    }

    // public async Task UpdateNameAsync(Person person)
    // {
    //     Context.Persons.Update(person);
    //     await Context.SaveChangesAsync();
    // }
    

    // public async Task RemovePersonAsync(Guid id)
    // {
    //     try
    //     {
    //         var person = await _context.Persons.FirstOrDefaultAsync(p => p.Id == id);
    //         // _context.Persons.Attach(person);
    //         _context.Persons.Remove(person);
    //         await _context.SaveChangesAsync();
    //     }
    //     catch (ArgumentNullException)
    //     {
    //         await RemovePersonAsync(id);
    //     }
    //     
    // }
}