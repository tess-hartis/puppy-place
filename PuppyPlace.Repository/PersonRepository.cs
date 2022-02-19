using LanguageExt;
using LanguageExt.SomeHelp;
using Microsoft.EntityFrameworkCore;
using PuppyPlace.Data;
using PuppyPlace.Domain;

namespace PuppyPlace.Repository;

public interface IPersonRepository : IGenericRepository<Person>
{
    new Task<Option<Person>> FindAsync(Guid id);
}
public class PersonRepository : GenericRepository<Person>, IPersonRepository
{
    private PuppyPlaceContext _context;
    
    public PersonRepository(PuppyPlaceContext context) : base(context)
    {
        _context = context;
    }

    public async Task AddPersonAsync(Person newPerson)
    {
         await _context.Persons.AddAsync(newPerson);
         await _context.SaveChangesAsync();
    }

    public async Task<IReadOnlyList<Person>> PersonsAsync()
    {
       return await _context.Persons.ToListAsync();
    }

    public override async Task<Option<Person>> FindAsync(Guid id)
    {
        var person = await _context.Persons
            .Include(p => p.Dogs)
            .FirstOrDefaultAsync(p => p.Id == id);
        
        if (person == null)
            return Option<Person>.None;

        return person.ToSome();

    }

    public async Task UpdateNameAsync(Person person)
    {
        _context.Persons.Update(person);
        await _context.SaveChangesAsync();
    }
    

    public async Task RemovePersonAsync(Guid id)
    {
        try
        {
            var person = await _context.Persons.FirstOrDefaultAsync(p => p.Id == id);
            // _context.Persons.Attach(person);
            _context.Persons.Remove(person);
            await _context.SaveChangesAsync();
        }
        catch (ArgumentNullException)
        {
            await RemovePersonAsync(id);
        }
        
    }
}