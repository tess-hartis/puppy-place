using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
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

    public void AddPerson(Person newPerson)
    {
        _context.Persons.Add(newPerson);
        _context.SaveChanges();
    }

    public IReadOnlyList<Person> Persons()
    {
       return _context.Persons.ToList();
    }

    public Person? FindById(Guid idPerson)
    {
        return _context.Persons
            .Include(p => p.Dogs)
            .FirstOrDefault(p => p.Id == idPerson);

    }

    public void UpdateName(Person person)
    {
        _context.Persons.Update(person);
        _context.SaveChanges();
    }

    public void AdoptDog()
    {
        
    }

    public void DeletePerson(Person person)
    {
        _context.Persons.Remove(person);
        _context.SaveChanges();
    }
}