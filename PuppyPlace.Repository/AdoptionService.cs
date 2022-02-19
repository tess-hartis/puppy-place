using PuppyPlace.Data;
using LanguageExt;
using static LanguageExt.Prelude;

namespace PuppyPlace.Repository;

public interface IAdoptionService
{
    Task<Option<Unit>> AdoptDog(Guid personId, Guid dogId);
    Task<Option<Unit>> AddOwner(Guid personId, Guid dogId);
}
public class AdoptionService : IAdoptionService
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
    public async Task<Option<Unit>> AdoptDog(Guid personId, Guid dogId)
    {
        var person = await PersonRepo.FindAsync(personId);
        var dog = await DogRepo.FindAsync(dogId);

        var result =
            from p in person
            from d in dog
            select p.AdoptDog(d);

        ignore(result.Map(async _ => await _context.SaveChangesAsync()));

        return result;
    }

    public async Task<Option<Unit>> AddOwner(Guid personId, Guid dogId)
    {
        var person = await PersonRepo.FindAsync(personId);
        var dog = await DogRepo.FindAsync(dogId);

        var result =
            from d in dog
            from p in person
            select d.AddOwner(p);

        ignore(result.Map(async _ => await _context.SaveChangesAsync()));

        return result;
    }
}
