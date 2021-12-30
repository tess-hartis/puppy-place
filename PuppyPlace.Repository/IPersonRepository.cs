using PuppyPlace.Domain;

namespace PuppyPlace.Repository;

public interface IPersonRepository
{
    Task AddPersonAsync(Person newPerson);
    Task<IReadOnlyList<Person>> PersonsAsync();
    Task<Person?> FindByIdAsync(Guid idPerson);
    Task UpdateNameAsync(Person person);
    Task RemovePersonAsync(Guid id);
    

}