using Microsoft.EntityFrameworkCore;
using PuppyPlace.Data;
using PuppyPlace.Domain;

namespace PuppyPlace.Repository;

public class DogRepository
{
    private PuppyPlaceContext _context;
    
    public DogRepository(PuppyPlaceContext context)
    {
        _context = context;
    }

    public void AddDog(Dog dog)
    {
        _context.Dogs.Add(dog);
        _context.SaveChanges();
    }

    public IReadOnlyList<Dog> Dogs()
    {
        return _context.Dogs.ToList();
    }

    public Dog? FindById(Guid id)
    {
        return _context.Dogs.Include(d => d.Owner).FirstOrDefault(d => d.Id == id);
    }

    public void DeleteDog(Dog dog)
    {
        _context.Dogs.Remove(dog);
        _context.SaveChanges();
    }

    public void UpdateName(Dog dog)
    {
        _context.Dogs.Update(dog);
        _context.SaveChanges();
    }
}