using PuppyPlace.Data;

namespace PuppyPlace.Console;

public static class DependencyInjection
{
    public static PersonTools PersonTools = new PersonTools(new Prompt(), new PuppyPlaceContext());
    
    public static DogTools DogTools = new DogTools(new Prompt(), new PuppyPlaceContext());
    
    public static PuppyPlaceContext _PuppyPlaceContext = new PuppyPlaceContext();
    
    public static Prompt Prompt = new Prompt();
}