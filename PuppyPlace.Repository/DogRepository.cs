using PuppyPlace.Data;
using PuppyPlace.Domain;

namespace PuppyPlace.Repository;

public class DogRepository
{
    private PuppyPlaceContext _context = new PuppyPlaceContext();

    public void AddDog(Dog dog)
    {
        _context.Dogs.Add(dog);
        _context.SaveChanges();
    }
}