using MediatR;
using Microsoft.EntityFrameworkCore;
using PuppyPlace.Domain;
using PuppyPlace.Repository;

namespace PuppyPlace.CqrsService.PersonCQ.Queries;

public record GetPersonsQuery : IRequest<IEnumerable<Person>>;
public class GetPersonsQueryHandler : IRequestHandler<GetPersonsQuery, IEnumerable<Person>>
{
    private readonly IPersonRepository _personRepository;

    public GetPersonsQueryHandler(IPersonRepository personRepository)
    {
        _personRepository = personRepository;
    }

    public async Task<IEnumerable<Person>> Handle
        (GetPersonsQuery request, CancellationToken cancellationToken)
    {
        return await _personRepository.GetAll();
    }
}