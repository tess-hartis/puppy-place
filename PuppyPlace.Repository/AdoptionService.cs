using PuppyPlace.Data;

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
    public async Task Adopt(Guid personId, Guid dogId )
    {
        var person = await PersonRepo.FindByIdAsync(personId);
        var dog = await DogRepo.FindByIdAsync(dogId);
        person.AdoptDog(dog);
        await _context.SaveChangesAsync();
    }
}
