using LanguageExt;
using static LanguageExt.Prelude;
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
    private readonly IAdoptionUnitOfWork _unitOfWork;

    public AdoptDogCommandHandler(IAdoptionUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Option<Unit>> Handle
        (AdoptDogCommand command, CancellationToken cancellationToken)
    {
        var person = await _unitOfWork.PersonRepo.FindAsync(command.PersonId);
        var dog = await _unitOfWork.DogRepo.FindAsync(command.DogId);

        var result =
            from p in person
            from d in dog
            select p.AdoptDog(d);

        ignore(result.Map(async _ => await _unitOfWork.Save()));

        return result;
    }
}