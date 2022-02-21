using MediatR;
using Microsoft.Extensions.DependencyInjection;
using PuppyPlace.CqrsService;
using PuppyPlace.Data;
using PuppyPlace.Repository;

namespace PuppyPlace.Console;

public static class DependencyInjection
{
    public static PersonTools PersonTools = new PersonTools(new Prompt(), 
        new Mediator(new ServiceFactory(ServiceFactory)));

    private static IMediator ServiceFactory(Type serviceType)
    {
        var services = new ServiceCollection();
        services.AddMediatR(typeof(MediatorEntry).Assembly);
        var provider = services.BuildServiceProvider();
        return provider.GetRequiredService<IMediator>();
    }

    public static DogTools DogTools = new DogTools(new Prompt(), 
        new Mediator(new ServiceFactory(ServiceFactory)));
    
    public static PuppyPlaceContext _PuppyPlaceContext = new PuppyPlaceContext();
    
    public static Prompt Prompt = new Prompt();

}