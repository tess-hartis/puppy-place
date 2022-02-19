using LanguageExt;
using MediatR;
using PuppyPlace.Domain;
using PuppyPlace.Repository;

namespace PuppyPlace.CqrsService.PersonCQ.Queries;

public class GetPersonByIdQuery : IRequest<Option<Person>>
{
    public Guid Id { get; }

    public GetPersonByIdQuery(Guid id)
    {
        Id = id;
    }
}
public class GetPersonByIdQueryHandler : IRequestHandler<GetPersonByIdQuery, Option<Person>>
{
    private readonly IPersonRepository _personRepository;

    public GetPersonByIdQueryHandler(IPersonRepository personRepository)
    {
        _personRepository = personRepository;
    }

    public async Task<Option<Person>> Handle
        (GetPersonByIdQuery request, CancellationToken cancellationToken)
    {
        return await _personRepository.FindAsync(request.Id);
    }

}