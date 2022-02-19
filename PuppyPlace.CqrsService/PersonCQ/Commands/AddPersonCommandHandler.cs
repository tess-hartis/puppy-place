using LanguageExt;
using LanguageExt.Common;
using MediatR;
using PuppyPlace.Domain;
using PuppyPlace.Domain.ValueObjects.PersonValueObjects;
using PuppyPlace.Repository;

namespace PuppyPlace.CqrsService.PersonCQ.Commands;

public class AddPersonCommand : IRequest<Validation<Error, Person>>
{
    public string Name { get; }

    public AddPersonCommand(string name)
    {
        Name = name;
    }
}
public class AddPersonCommandHandler : 
    IRequestHandler<AddPersonCommand, Validation<Error, Person>>
{
    private readonly IPersonRepository _personRepository;
    
    public AddPersonCommandHandler(IPersonRepository personRepository)
    {
        _personRepository = personRepository;
    }

    public async Task<Validation<Error, Person>> Handle
        (AddPersonCommand command, CancellationToken cancellationToken)
    {
        var name = PersonName.Create(command.Name);

        var newPerson = name.Map(n => Person.Create(n));

        await newPerson
            .Succ(async p =>
            {
                await _personRepository.AddAsync(p);
            })
            
            .Fail(e => e.AsTask());

        return newPerson;
    }
}