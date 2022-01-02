using PuppyPlace.Domain;

namespace PuppyPlace.Repository;

public interface IDogRepository
{
    Task AddDogAsync(Dog dog);
    
    Task<IReadOnlyList<Dog>> DogsAsync();
    
    Task<Dog?> FindByIdAsync(Guid id);
    
    Task RemoveDogAsync(Guid id);
    
    Task UpdateNameAsync(Dog dog);
}