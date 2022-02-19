using MediatR;
using Microsoft.EntityFrameworkCore;
using PuppyPlace.Domain;
using PuppyPlace.Repository;

namespace PuppyPlace.CqrsService.DogCQ.Queries;

public record GetDogsQuery : IRequest<IEnumerable<Dog>>;

public class GetDogsQueryHandler : IRequestHandler<GetDogsQuery, IEnumerable<Dog>>
{
    private readonly IDogRepository _dogRepository;

    public GetDogsQueryHandler(IDogRepository dogRepository)
    {
        _dogRepository = dogRepository;
    }

    public async Task<IEnumerable<Dog>> Handle
        (GetDogsQuery request, CancellationToken cancellationToken)
    {
        var dogs = await _dogRepository.GetEntities().ToListAsync();
        return dogs;
    }
}