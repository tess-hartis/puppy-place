using LanguageExt;
using MediatR;
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
    private readonly IAdoptionService _adoptionService;

    public AddOwnerCommandHandler(IAdoptionService adoptionService)
    {
        _adoptionService = adoptionService;
    }

    public async Task<Option<Unit>> Handle
        (AddOwnerCommand command, CancellationToken cancellationToken)
    {
        return await _adoptionService.AddOwner(command.PersonId, command.DogId);
    }
}