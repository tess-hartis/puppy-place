using PuppyPlace.Domain;

namespace PuppyPlaceApi.Dtos;

public class GetPersonDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public IEnumerable<string> Dogs { get; set; } = new List<string>();

    public static GetPersonDto FromPerson(Person person)
    {
        var dogNames = person.Dogs.Select(x => x.Name.Value);

        return new GetPersonDto
        {
            Id = person.Id,
            Name = person.Name.Value,
            Dogs = dogNames
        };
    }
}