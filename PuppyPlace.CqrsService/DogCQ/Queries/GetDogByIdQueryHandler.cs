using LanguageExt;
using MediatR;
using PuppyPlace.Domain;
using PuppyPlace.Repository;

namespace PuppyPlace.CqrsService.DogCQ.Queries;

public class GetDogByIdQuery : IRequest<Option<Dog>>
{
    public Guid Id { get; }

    public GetDogByIdQuery(Guid id)
    {
        Id = id;
    }
}
public class GetDogByIdQueryHandler : 
    IRequestHandler<GetDogByIdQuery, Option<Dog>>
{
    private readonly IDogRepository _dogRepository;

    public GetDogByIdQueryHandler(IDogRepository dogRepository)
    {
        _dogRepository = dogRepository;
    }

    public async Task<Option<Dog>> Handle
        (GetDogByIdQuery request, CancellationToken cancellationToken)
    {
        return await _dogRepository.FindAsync(request.Id);
    }
}