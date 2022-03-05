using PuppyPlace.Blazor.Features.Person;
using PuppyPlace.Domain;

namespace PuppyPlace.Blazor.Features.Dog;

public class GetDogDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Age { get; set; }
    public string Breed { get; set; }
    public IEnumerable<string> Owners { get; set; } = new List<string>();

    public static GetDogDto FromDog(Domain.Dog dog)
    {
        var owners = dog.Owners.Select(x => x.Name.Value);
        return new GetDogDto
        {
            Id = dog.Id,
            Name = dog.Name.Value,
            Age = dog.Age.Value,
            Breed = dog.Breed.Value,
            Owners = owners
        };
    }
}
