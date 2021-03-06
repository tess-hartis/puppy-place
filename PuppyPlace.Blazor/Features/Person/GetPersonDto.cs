using PuppyPlace.Domain;

namespace PuppyPlace.Blazor.Features.Person;


public class GetPersonDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public IEnumerable<string> Dogs { get; set; } = new List<string>();

    public static GetPersonDto FromPerson(Domain.Person person)
    {
        var dogs = person.Dogs.Select(x => x.Name.Value);

        return new GetPersonDto
        {
            Id = person.Id,
            Name = person.Name.Value,
            Dogs = dogs
        };
    }
}
