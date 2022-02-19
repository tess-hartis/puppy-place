using LanguageExt;
using MediatR;
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
    private readonly IAdoptionService _adoptionService;

    public AdoptDogCommandHandler(IAdoptionService adoptionService)
    {
        _adoptionService = adoptionService;
    }

    public async Task<Option<Unit>> Handle
        (AdoptDogCommand command, CancellationToken cancellationToken)
    {
        return await _adoptionService.AdoptDog(command.PersonId, command.DogId);
    }
}