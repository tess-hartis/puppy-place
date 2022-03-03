using LanguageExt;
using static LanguageExt.Prelude;
using MediatR;
using PuppyPlace.Repository;
using Unit = LanguageExt.Unit;

namespace PuppyPlace.CqrsService.DogCQ.Commands;

public class DeleteDogCommand : IRequest<Option<Unit>>
{
    public Guid Id;

    public DeleteDogCommand(Guid id)
    {
        Id = id;
    }
}

public class DeleteDogCommandHandler : IRequestHandler<DeleteDogCommand, Option<Unit>>
{
    private readonly IDogRepository _dogRepository;

    public DeleteDogCommandHandler(IDogRepository dogRepository)
    {
        _dogRepository = dogRepository;
    }

    public async Task<Option<Unit>> Handle
        (DeleteDogCommand command, CancellationToken cancellationToken)
    {
        var dog = await _dogRepository.FindAsync(command.Id);
        ignore(dog.Map(async d => await _dogRepository.DeleteAsync(d)));
        return dog.Map(d => unit);
    }
}