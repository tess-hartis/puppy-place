using LanguageExt;
using static LanguageExt.Prelude;
using MediatR;
using PuppyPlace.Data;
using PuppyPlace.Repository;
using Unit = LanguageExt.Unit;

namespace PuppyPlace.CqrsService.PersonCQ.Commands;

public class AdoptDogCommand : IRequest<Option<Unit>>
{
    public Guid PersonId { get; }
    public Guid DogId { get; }

    public AdoptDogCommand(Guid personId, Guid dogId)
    {
        PersonId = personId;
        DogId = dogId;
    }
}
public class AdoptDogCommandHandler : 
    IRequestHandler<AdoptDogCommand, Option<Unit>>
{
    private readonly PuppyPlaceContext _context;
    private IDogRepository _dogRepository;
    private IPersonRepository _personRepository;

    public AdoptDogCommandHandler(PuppyPlaceContext context)
    {
        _context = context;
    }
    
    private IDogRepository DogRepo
    {
        get
        {
            return _dogRepository = new DogRepository(_context);
        }
    }

    private IPersonRepository PersonRepo
    {
        get
        {
            return _personRepository = new PersonRepository(_context);
        }
    }

    public async Task<Option<Unit>> Handle
        (AdoptDogCommand command, CancellationToken cancellationToken)
    {
        var person = await PersonRepo.FindAsync(command.PersonId);
        var dog = await DogRepo.FindAsync(command.DogId);

        var result =
            from p in person
            from d in dog
            select p.AdoptDog(d);

        ignore(result.Map(async _ => await _context.SaveChangesAsync()));

        return result;
    }
}