using LanguageExt;
using static LanguageExt.Prelude;
using MediatR;
using PuppyPlace.Repository;
using Unit = LanguageExt.Unit;

namespace PuppyPlace.CqrsService.PersonCQ.Commands;

public class DeletePersonCommand : IRequest<Option<Unit>>
{
    public Guid Id;

    public DeletePersonCommand(Guid id)
    {
        Id = id;
    }
}
public class DeletePersonCommandHandler : IRequestHandler<DeletePersonCommand, Option<Unit>>
{
    private readonly IPersonRepository _personRepository;

    public DeletePersonCommandHandler(IPersonRepository personRepository)
    {
        _personRepository = personRepository;
    }

    public async Task<Option<Unit>> Handle
        (DeletePersonCommand command, CancellationToken cancellationToken)
    {
        var person = await _personRepository.FindAsync(command.Id);
        ignore(person.Map(async p => await _personRepository.DeleteAsync(p)));
        return person.Map(p => unit);
    }
}