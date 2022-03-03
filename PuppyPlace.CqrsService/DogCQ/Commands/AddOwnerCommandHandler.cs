using LanguageExt;
using static LanguageExt.Prelude;
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
    private readonly IAdoptionUnitOfWork _unitOfWork;

    public AddOwnerCommandHandler(IAdoptionUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Option<Unit>> Handle
        (AddOwnerCommand command, CancellationToken cancellationToken)
    {
        var person = await _unitOfWork.PersonRepo.FindAsync(command.PersonId);
        var dog = await _unitOfWork.DogRepo.FindAsync(command.DogId);

        var result =
            from d in dog
            from p in person
            select d.AddOwner(p);

        ignore(result.Map(async _ => await _unitOfWork.Save()));

        return result;
    }
}