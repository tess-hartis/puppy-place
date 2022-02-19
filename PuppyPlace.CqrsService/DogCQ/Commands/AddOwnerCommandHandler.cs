using LanguageExt;
using static LanguageExt.Prelude;
using MediatR;
using PuppyPlace.Data;
using PuppyPlace.Repository;
using Unit = LanguageExt.Unit;

namespace PuppyPlace.CqrsService.DogCQ.Commands;

public class AddOwnerCommand : IRequest<Option<Unit>>
{
    public Guid DogId { get; }
    public Guid PersonId { get; }

    public AddOwnerCommand(Guid dogId, Guid personId)
    {
        DogId = dogId;
        PersonId = personId;
    }
}
public class AddOwnerCommandHandler : IRequestHandler<AddOwnerCommand, Option<Unit>>
{
    private readonly PuppyPlaceContext _context;
    private IDogRepository _dogRepository;
    private IPersonRepository _personRepository;

    public AddOwnerCommandHandler(PuppyPlaceContext context)
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
        (AddOwnerCommand command, CancellationToken cancellationToken)
    {
        var person = await PersonRepo.FindAsync(command.PersonId);
        var dog = await DogRepo.FindAsync(command.DogId);

        var result =
            from d in dog
            from p in person
            select d.AddOwner(p);

        ignore(result.Map(async _ => await _context.SaveChangesAsync()));

        return result;
    }
}