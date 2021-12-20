using Microsoft.EntityFrameworkCore;
using PuppyPlace.Data;
using PuppyPlace.Domain;

namespace PuppyPlace.Repository;

public class PersonRepository
{
    private PuppyPlaceContext _context;
    
    public PersonRepository(PuppyPlaceContext context)
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

    public async Task<Person?> FindByIdAsync(Guid idPerson)
    {
        return await _context.Persons
            .Include(p => p.Dogs)
            .FirstOrDefaultAsync(p => p.Id == idPerson);

    }

    public async Task UpdateNameAsync(Person person)
    {
        _context.Persons.Update(person);
        await _context.SaveChangesAsync();
    }

    public void AdoptDog()
    {
        
    }

    public async Task RemovePersonAsync(Person person)
    {
        _context.Persons.Remove(person);
        await _context.SaveChangesAsync();
    }
}