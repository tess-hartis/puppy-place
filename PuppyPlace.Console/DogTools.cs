using System.Reflection.Metadata;
using FluentValidation.Results;
using LanguageExt;
using LanguageExt.Common;
using MediatR;
using PuppyPlace.CqrsService.DogCQ.Commands;
using PuppyPlace.CqrsService.DogCQ.Queries;
using PuppyPlace.CqrsService.PersonCQ.Queries;
using PuppyPlace.Domain;
using PuppyPlace.Domain.ValueObjects.DogValueObjects;
using PuppyPlace.Repository;

namespace PuppyPlace.Console;

public class DogTools
{
    private readonly Prompt _prompt;
    private readonly IMediator _mediator;
    
    public DogTools(Prompt prompt, IMediator mediator)
    {
        _prompt = prompt;
        _mediator = mediator;
    }

    public async Task AddDog()
    {
        System.Console.Clear();

        System.Console.WriteLine("Please insert the dog's name:");
        
        var dogNameInput = System.Console.ReadLine();

        System.Console.Clear();
        Thread.Sleep(1000);
        
        System.Console.WriteLine($"Please insert the dog's age:");
        var dogAgeInput = System.Console.ReadLine();
        int number;
        var success = int.TryParse(dogAgeInput, out number);
        if (!success)
        {
            System.Console.WriteLine("Dog's age must be a number");
        }
        
        System.Console.Clear();
        Thread.Sleep(1000); 
        
        System.Console.WriteLine($"Please insert the dog's breed:");
        var dogBreedInput = System.Console.ReadLine();

        System.Console.Clear();
        Thread.Sleep(1000);

        var command = new AddDogCommand(dogNameInput, number, dogBreedInput);
        var result = await _mediator.Send(command);

        if (result.IsSuccess)
        {
            System.Console.WriteLine("Success! We added the following information to the database:" +
                                     "\n==========================================================" +
                                     $"\nName: {dogNameInput}" +
                                     $"\nAge: {number}" +
                                     $"\nBreed: {dogBreedInput}" +
                                     $"\n=========================================================");
            
            Thread.Sleep(1000);
            await PromptToAddAnotherDog();
        }
        
        if(result.IsFail)
        {
            var errors = result.FailAsEnumerable();
            var errorsToList = errors.Select(e => e.Message).ToList();
            System.Console.WriteLine($"{errorsToList}");
            Thread.Sleep(1500);
            await AddDog();
        }
        
    }

    async Task PromptToAddAnotherDog()
    {
        System.Console.WriteLine("Add another dog? Choose (Y)es or (N)o");
        var yesNo = System.Console.ReadKey();

        switch (yesNo.Key)
        {
            case ConsoleKey.Y:
                System.Console.Clear();
                await AddDog();
                break;
            case ConsoleKey.N:
                await Prompt.ReturnToMainMenu();
                break;
            default:
                System.Console.Clear();
                System.Console.WriteLine("Invalid option. Please type 'yes' or 'no'");
                await PromptToAddAnotherDog();
                break;

        }
    }

    public async Task ShowListOfDogsAsync()
    {
        System.Console.Clear();
        System.Console.WriteLine("Here are the dogs in the database:" +
                                 "\n(Enter a number to view a dog or (M)ain Menu)" +
                                 "\n====================================");
        var dogCount = 1;
        var query = new GetDogsQuery();
        var dogs = await _mediator.Send(query);
        var dogsList = dogs.ToList();
        foreach (var dog in dogsList)
        {
            System.Console.WriteLine($"{dogCount} {dog.Name}");
            dogCount++;
        }

        var key = System.Console.ReadKey();
        bool isDigit = char.IsDigit(key.KeyChar);
        switch (key.Key)
        {
            case ConsoleKey.M:
                await Prompt.ReturnToMainMenu();
                break;
            default:
                try
                {
                    if (isDigit)
                    {
                        var userInput = int.Parse(key.KeyChar.ToString());
                        var dogId = dogsList[userInput - 1].Id;
                        // await ShowDogAsync(dogId);
                    }

                    if (!isDigit && key.Key != ConsoleKey.M)
                    {
                        System.Console.Clear();
                        System.Console.WriteLine("Enter a number or M for Main Menu");
                        Thread.Sleep(1000);
                        await ShowListOfDogsAsync();
                    }
                }
                catch (ArgumentOutOfRangeException e)
                {
                    System.Console.Clear();
                    System.Console.WriteLine("Enter a number or M for Main Menu");
                    Thread.Sleep(1000);
                    await ShowListOfDogsAsync();
                }
                break;
        }
    }

    // public async Task ShowDogAsync(Guid id)
    // {
    //     System.Console.Clear();
    //     var query = new GetDogByIdQuery(id);
    //     var dog = await _mediator.Send(query);
    //     dog.Some(async d =>
    //         {
    //             System.Console.WriteLine($"Name: {d.Name.Value}");
    //             System.Console.WriteLine($"Age: {d.Age.Value}");
    //             System.Console.WriteLine($"Breed: {d.Breed.Value}");
    //             System.Console.WriteLine("Owners:");
    //
    //             if (d.Owners.Count() > 0)
    //             {
    //                 foreach (var person in d.Owners)
    //                 {
    //                     System.Console.WriteLine(person.Name);
    //                 }
    //             }
    //             else
    //             {
    //                 System.Console.WriteLine("doesn't have an owner yet!");
    //             }
    //
    //             System.Console.WriteLine("\nWhat would you like to do?" +
    //                                      "\n" +
    //                                      "\n(A)dd Owner (E)dit Name (D)elete Dog (L)ist of Dogs (M)ain Menu");
    //
    //             var userInput = System.Console.ReadKey();
    //             switch (userInput.Key)
    //             {
    //                 case ConsoleKey.A:
    //                     System.Console.WriteLine($"Let's give {d.Name} an owner!");
    //                     await SelectDogOwner(d);
    //                     break;
    //                 case ConsoleKey.E:
    //                     await UpdateDog(d);
    //                     break;
    //                 case ConsoleKey.D:
    //                     await DeleteDogAsync(d);
    //                     break;
    //                 case ConsoleKey.L:
    //                     await ShowListOfDogsAsync();
    //                     break;
    //                 case ConsoleKey.M:
    //                     await Prompt.ReturnToMainMenu();
    //                     break;
    //                 default:
    //                     System.Console.WriteLine("Invalid selection. Please select option number.");
    //                     Thread.Sleep(1000);
    //                     System.Console.Clear();
    //                     await ShowListOfDogsAsync();
    //                     break;
    //             }
    //         })
    //         .None(_ => await Prompt.MainMenu());
    //
    // }
    public async Task SelectDogOwner(Dog dog)
    {
        System.Console.Clear();
        System.Console.WriteLine("Here are the people in the database:");
        System.Console.WriteLine($"Enter a number to give {dog.Name} an owner!");
        System.Console.WriteLine("(M)ain Menu (L)ist of Dogs");
        System.Console.WriteLine("===================================");
        var personCount = 1;
        var personsQuery = new GetPersonsQuery();
        var persons = await _mediator.Send(personsQuery);
        var personsList = persons.ToList();
        foreach (var person in personsList)
        {
            System.Console.WriteLine($"{personCount} {person.Name}");
            personCount++;
        }
    
        var key = System.Console.ReadKey();
        bool isDigit = char.IsDigit(key.KeyChar);
        switch (key.Key)
        {
            case ConsoleKey.L:
                await ShowListOfDogsAsync();
                break;
            case ConsoleKey.M:
                await Prompt.ReturnToMainMenu();
                break;
            default:
                try
                {
                    if (isDigit)
                    {
                        var userInput = int.Parse(key.KeyChar.ToString());
                        var owner = personsList[userInput - 1];
                        var addOwnerCommand = new AddOwnerCommand(dog.Id, owner.Id);
                        var result = await _mediator.Send(addOwnerCommand);
                        if (result.IsSome)
                        {
                            System.Console.Clear();
                            System.Console.WriteLine($"{owner.Name} is now {dog.Name}'s owner!");
                            Thread.Sleep(1000);
                            await ShowListOfDogsAsync();
                        }

                        if (result.IsNone)
                        {
                            System.Console.WriteLine("\n");
                            System.Console.WriteLine("Owner not found");
                            Thread.Sleep(1500);
                            await SelectDogOwner(dog);
                        }
                    }
    
                    if (!isDigit)
                    {
                        System.Console.WriteLine("Please enter a number");
                        Thread.Sleep(1000);
                        await SelectDogOwner(dog);
                    }
                }
                catch (Exception)
                {
                    await Prompt.MainMenu();
                }
                break;
        }
    }
    
    

    public async Task DeleteDogAsync(Dog dog)
    {
        System.Console.Clear();
        System.Console.WriteLine($"Are you sure you want to delete {dog.Name} from the database? (Y)es (N)o");
        
        var yesNo = System.Console.ReadKey();
        switch (yesNo.Key)
        {
            case ConsoleKey.Y:
                var command = new DeleteDogCommand(dog.Id);
                var result = await _mediator.Send(command);
                if (result.IsSome)
                {
                    System.Console.Clear();
                    System.Console.WriteLine($"{dog.Name} has been deleted.");
                    Thread.Sleep(1000);
                    await ShowListOfDogsAsync();
                }

                if (result.IsNone)
                {
                    System.Console.WriteLine("The dog was not found");
                    Thread.Sleep(1000);
                    await ShowListOfDogsAsync();
                }
                break;
            
            case ConsoleKey.N:
                System.Console.Clear();
                await ShowListOfDogsAsync();
                break;
            default:
                System.Console.Clear();
                System.Console.WriteLine("Invalid Key Selection");
                System.Console.WriteLine("Returning to list of dogs...");
                Thread.Sleep(1000);
                await ShowListOfDogsAsync();
                break;
        }
    }
    public async Task UpdateDog(Dog dog)
    {
        System.Console.Clear();
        System.Console.WriteLine($"Enter name for {dog.Name.Value}");
        var dogNameInput = System.Console.ReadLine();
        Thread.Sleep(1000);
        
        System.Console.Clear();
        System.Console.WriteLine($"Enter age for {dog.Name.Value}");
        var dogAgeInput = System.Console.ReadLine();
        Thread.Sleep(1000);

        int number;
        var success = int.TryParse(dogAgeInput, out number);
        if (!success)
        {
            await UpdateDog(dog);
        }
        
        System.Console.Clear();
        System.Console.WriteLine($"Enter breed for {dog.Name.Value}");
        var dogBreedInput = System.Console.ReadLine();
        Thread.Sleep(1000);

        var command = new UpdateDogCommand(dog.Id, dogNameInput, number, dogBreedInput);
        var result = await _mediator.Send(command);
        result.Some(async x =>
        {
            if (x.IsSuccess)
            {
                System.Console.Clear();
                System.Console.WriteLine("Dog has been updated!");
                Thread.Sleep(1500);
                await Prompt.ReturnToMainMenu();
            }

            if (x.IsFail)
            {
                var errors = x.FailAsEnumerable().Select(x => x.Message).ToList();
                System.Console.WriteLine($"{errors}");
            }
        })
            .None(async () =>
            {
                System.Console.WriteLine("No good!");
                await UpdateDog(dog);
            });
        await Prompt.ReturnToMainMenu();
    }

    
}