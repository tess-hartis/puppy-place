using LanguageExt;
using LanguageExt.Common;
using static LanguageExt.Prelude;
using MediatR;
using PuppyPlace.Domain;
using PuppyPlace.Domain.ValueObjects.PersonValueObjects;
using PuppyPlace.Repository;
using Unit = LanguageExt.Unit;

namespace PuppyPlace.CqrsService.PersonCQ.Commands;

public class UpdatePersonCommand : IRequest<Option<Validation<Error, Unit>>>
{
    public Guid Id { get; set; }
    public string Name { get; }

    public UpdatePersonCommand(Guid id, string name)
    {
        Id = id;
        Name = name;
    }
}
public class UpdatePersonCommandHandler : 
    IRequestHandler<UpdatePersonCommand, Option<Validation<Error, Unit>>>
{
    private readonly IPersonRepository _personRepository;

    public UpdatePersonCommandHandler(IPersonRepository personRepository)
    {
        _personRepository = personRepository;
    }

    public async Task<Option<Validation<Error, Unit>>> Handle
        (UpdatePersonCommand command, CancellationToken cancellationToken)
    {
        var person = await _personRepository.FindAsync(command.Id);
        var name = PersonName.Create(command.Name);

        var updatedPerson = person
            .Map(p => name.Map(p.Update));

        ignore(updatedPerson
            .Map(p =>
                p.Map(async x => await _personRepository.SaveAsync())));

        return updatedPerson;

    }
}