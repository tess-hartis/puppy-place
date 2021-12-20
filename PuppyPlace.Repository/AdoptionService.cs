using PuppyPlace.Data;
using PuppyPlace.Domain;

namespace PuppyPlace.Repository;

public class AdoptionService
{
    private readonly PuppyPlaceContext _context;
    private DogRepository _dogRepository;
    private PersonRepository _personRepository;

    public AdoptionService(PuppyPlaceContext context)
    {
        _context = context;
    }
    public DogRepository DogRepo
    {
        get
        {
            return _dogRepository = new DogRepository(_context);
        }
    }

    public PersonRepository PersonRepo
    {
        get
        {
            return _personRepository = new PersonRepository(_context);
        }
    }
    public void Adopt(Guid personId, Guid dogId )
    {
        var person = PersonRepo.FindById(personId);
        var dog = DogRepo.FindById(dogId);
        person.AdoptDog(dog);
        _context.SaveChanges();
    }
}
