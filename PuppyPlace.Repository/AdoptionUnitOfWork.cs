using PuppyPlace.Data;

namespace PuppyPlace.Repository;

public interface IAdoptionUnitOfWork
{
    Task Save();
    IPersonRepository PersonRepo { get; }
    IDogRepository DogRepo { get; }
}

public class AdoptionUnitOfWork : IAdoptionUnitOfWork
{
    private readonly PuppyPlaceContext _context;
    private IPersonRepository _personRepository;
    private IDogRepository _dogRepository;

    public AdoptionUnitOfWork(PuppyPlaceContext context)
    {
        _context = context;
    }
    
    public IDogRepository DogRepo
    {
        get
        {
            return _dogRepository = new DogRepository(_context);
        }
    }

    public IPersonRepository PersonRepo
    {
        get
        {
            return _personRepository = new PersonRepository(_context);
        }
    }

    public async Task Save()
    {
        await _context.SaveChangesAsync();
    }
}