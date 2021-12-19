using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using PuppyPlace.Data;
using PuppyPlace.Domain;

namespace PuppyPlace.Repository;

public class PersonRepository
{
    private PuppyPlaceContext _context = new PuppyPlaceContext();

    public void AddPerson(Person newPerson)
    {
        _context.Persons.Add(newPerson);
        _context.SaveChanges();
    }

    public IEnumerable<Person> Persons()
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
}