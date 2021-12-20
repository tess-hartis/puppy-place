using PuppyPlace.Data;
using PuppyPlace.Repository;

namespace PuppyPlace.Console;

public static class DependencyInjection
{
    public static PersonTools PersonTools = new PersonTools(new Prompt(), 
        new PersonRepository(new PuppyPlaceContext()), 
        new DogRepository(new PuppyPlaceContext()), 
        new AdoptionService(new PuppyPlaceContext()));
    
    public static DogTools DogTools = new DogTools(new Prompt(), new DogRepository(new PuppyPlaceContext()), new PersonRepository(new PuppyPlaceContext()));
    
    public static PuppyPlaceContext _PuppyPlaceContext = new PuppyPlaceContext();
    
    public static Prompt Prompt = new Prompt();
}