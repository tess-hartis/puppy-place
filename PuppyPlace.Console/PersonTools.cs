using FluentValidation.Results;
using MediatR;
using PuppyPlace.Console;
using PuppyPlace.CqrsService.DogCQ.Queries;
using PuppyPlace.CqrsService.PersonCQ.Commands;
using PuppyPlace.CqrsService.PersonCQ.Queries;
using PuppyPlace.Domain;
using PuppyPlace.Domain.ValueObjects.PersonValueObjects;
using PuppyPlace.Repository;
using Spectre.Console;
using Unit = LanguageExt.Unit;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace PuppyPlace.Console;

public class PersonTools
{
    private readonly Prompt _prompt;
    private readonly IMediator _mediator;

    public PersonTools(Prompt prompt, IMediator mediator)
    {
        _prompt = prompt;
        _mediator = mediator;
    }

    public async Task AddPerson()
    {
        System.Console.Clear();
        var nameInput = AnsiConsole.Ask<string>("What is the person's [green]name[/]?");
        var command = new AddPersonCommand(nameInput);
        var person = await _mediator.Send(command);
        if (person.IsSuccess)
        {
            System.Console.Clear();
            System.Console.WriteLine($"The following person has been successfully created: {nameInput}");
            await PromptToAddAnotherPerson();
        }

        if (person.IsFail)
        {
            var errors = person.FailAsEnumerable().Select(e => e.Message).ToList();
            System.Console.WriteLine($"{errors}");
            Thread.Sleep(1500);
            await AddPerson();
        }
    }
    
    public async Task PromptToAddAnotherPerson()
    {
        System.Console.WriteLine("Add another person? (Y)es (N)o");
        var yesNo = System.Console.ReadKey();

        switch (yesNo.Key)
        {
            case ConsoleKey.Y:
                await AddPerson();
                break;
            case ConsoleKey.N:
                await Prompt.ReturnToMainMenu();
                break;
            default:
                System.Console.Clear();
                System.Console.WriteLine("Invalid option. Please type 'yes' or 'no'");
                await PromptToAddAnotherPerson();
                break;
        }
    }

    public async Task ShowListOfPeopleAsync()
    {
        System.Console.Clear();
        System.Console.WriteLine("Here are the people in the database:" +
                                 "\n(Enter a number to view a person or (M)ain Menu)" +
                                 "\n====================================");
        var personCount = 1;
        var query = new GetPersonsQuery();
        var persons = await _mediator.Send(query);
        foreach (var person in persons)
        {
            System.Console.WriteLine($"{personCount} {person.Name}");
            personCount++;
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
                        var personId = persons.ElementAtOrDefault(userInput - 1).Id;
                        // await ShowPerson(personId);
                    }

                    if (!isDigit && key.Key != ConsoleKey.M)
                    {
                        System.Console.Clear();
                        System.Console.WriteLine("Enter a number or M for Main Menu");
                        Thread.Sleep(1000);
                        await ShowListOfPeopleAsync();
                    }
                }
                catch (ArgumentOutOfRangeException e)
                {
                    System.Console.Clear();
                    System.Console.WriteLine("Enter a number or M for Main Menu");
                    Thread.Sleep(1000);
                    await ShowListOfPeopleAsync();
                }

                break;
        }
    }

    // public async Task ShowList()
    // {
    //     // LINQ.Select takes a "lambda"
    //     // x -> x.Name
    //     // [1,2,3]
    //     // [2,3,4]
    //     // [P1, P2, P3]
    //     // [Name1, Name2, Name3]
    //     // Select is "Map"
    //     // Select.(lambda)
    //     var people = await _personRepository.PersonsAsync();
    //     var peepsList = people.ToList();
    //     var daNames = peepsList.Select(p => $"{peepsList.IndexOf(p) + 1} - {p.Name}");
    //     var person = AnsiConsole.Prompt(new SelectionPrompt<string>()
    //         .Title("Select a person to view options")
    //         .PageSize(9)
    //         .AddChoices(daNames));
    //    
    // }
    
    public async Task ShowPerson(Guid id)
    {
        System.Console.Clear();
        try
        {
            var query = new GetPersonByIdQuery(id);
            var person = await _mediator.Send(query);
            
            person.Some(async p =>
                {
                    System.Console.WriteLine($"Getting {p.Name}'s information...");
                    Thread.Sleep(1000);
                    System.Console.Clear();
                    System.Console.WriteLine($"Name: {p.Name}");
                    System.Console.WriteLine($"ID: {p.Id}");
                    System.Console.WriteLine("Dogs:");
                    if (p.Dogs.Count() > 0)
                    {
                        foreach (var dog in p.Dogs)
                        {
                            System.Console.WriteLine(dog.Name.Value);
                        }
                    }
                    else
                    {
                        System.Console.WriteLine($"{p.Name} has no dogs to show");
                    }
    
                    System.Console.WriteLine("\nWhat would you like to do?" +
                                             "\n" +
                                             "\n(A)dopt a dog (E)dit Name (D)elete Person (S)how People (M)ain Menu");
    
                    var userInput = System.Console.ReadKey();
                    switch (userInput.Key)
                    {
                        case ConsoleKey.A:
                            System.Console.WriteLine($"Let's give {p.Name} an owner!");
                            await AdoptDog(p);
                            break;
                        case ConsoleKey.D:
                            await DeletePerson(p);
                            break;
                        case ConsoleKey.S:
                            await ShowListOfPeopleAsync();
                            break;
                        case ConsoleKey.E:
                            await UpdateName(p);
                            break;
                        case ConsoleKey.M:
                            await Prompt.ReturnToMainMenu();
                            break;
                        default:
                            System.Console.WriteLine("Invalid selection. Please select option number.");
                            Thread.Sleep(1000);
                            System.Console.Clear();
                            await ShowListOfPeopleAsync();
                            break;
                    }
                })
                .None(() =>
                {
                    System.Console.WriteLine("No person was found");
                    return await Prompt.MainMenu();
                });
        }
        catch (Exception e)
        {
            await Prompt.MainMenu();
        }
    }

    public async Task UpdateName(Person person)
    {
        System.Console.Clear();
        System.Console.WriteLine($"Enter a new name for {person.Name}");
        var nameInput = System.Console.ReadLine();
        var command = new UpdatePersonCommand(person.Id, nameInput);
        var updatedPerson = await _mediator.Send(command);
        
        updatedPerson
            .Some(async x =>
            {
                if (x.IsSuccess)
                {
                    System.Console.Clear();
                    System.Console.WriteLine("Name has been updated!");
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
                await UpdateName(person);
            });
        

        await Prompt.ReturnToMainMenu();
    }

    public async Task AdoptDog(Person person)
    {
        System.Console.Clear();
        System.Console.WriteLine("Here are the dogs in the database:" +
                                 "\nEnter a number to adopt a dog!" +
                                 "\n(M)ain Menu (L)ist of People" +
                                 "\n==============================");
        var dogCount = 1;
        var dogsQuery = new GetDogsQuery();
        var dogsResult = await _mediator.Send(dogsQuery);
        var dogsList = dogsResult.ToList();
        foreach (var dog in dogsList)
        {
            System.Console.WriteLine($"{dogCount} {dog.Name.Value}");
            dogCount++;
        }

        var key = System.Console.ReadKey();
        bool isDigit = char.IsDigit(key.KeyChar);
        switch (key.Key)
        {
            case ConsoleKey.M:
                await Prompt.ReturnToMainMenu();
                break;
            case ConsoleKey.L:
                await ShowListOfPeopleAsync();
                break;
            default:
                try
                {
                    if (isDigit)
                    {
                        var input = int.Parse(key.KeyChar.ToString());
                        var dog = dogsList[input - 1];
                        var adoptCommand = new AdoptDogCommand(person.Id, dog.Id);
                        var result = await _mediator.Send(adoptCommand);
                        if (result.IsSome)
                        {
                            System.Console.Clear();
                            System.Console.WriteLine($"{person.Name} has adopted {dog.Name}!");
                            await PromptToAdoptAnotherDog();
                        }

                        if (result.IsNone)
                        {
                            System.Console.WriteLine("\n");
                            System.Console.WriteLine("Dog not found");
                            Thread.Sleep(1500);
                            await AdoptDog(person);
                        }
                    }

                    if (!isDigit)
                    {
                        System.Console.WriteLine("Please enter a number");
                        Thread.Sleep(1000);
                        await AdoptDog(person);
                    }
                }
                catch (Exception)
                {
                    await Prompt.MainMenu();
                }

                break;
        }
    }

    public async Task PromptToAdoptAnotherDog()
    {
        System.Console.WriteLine("(A)dopt Another Dog (R)eturn to Main Menu");
        var userInput = System.Console.ReadKey();
        switch (userInput.Key)
        {
            case ConsoleKey.A:
                await ShowListOfPeopleAsync();
                break;
            case ConsoleKey.R:
                await Prompt.ReturnToMainMenu();
                break;
            default:
                await PromptToAdoptAnotherDog();
                break;
        }
    }

    public async Task DeletePerson(Person person)
    {
        System.Console.Clear();
        System.Console.WriteLine(
            $"Are you sure you want to delete {person.Name} from the database? (Y)es (N)o");
        var yesNo = System.Console.ReadKey();
        switch (yesNo.Key)
        {
            case ConsoleKey.Y:
                var command = new DeletePersonCommand(person.Id);
                var result = await _mediator.Send(command);
                if (result.IsSome)
                {
                    System.Console.Clear();
                    System.Console.WriteLine($"{person.Name} has been deleted.");
                    Thread.Sleep(1000);
                    await ShowListOfPeopleAsync();
                }

                if (result.IsNone)
                {
                    System.Console.WriteLine($"The person was not found");
                    await ShowListOfPeopleAsync();
                }
                break;
            
            case ConsoleKey.N:
                System.Console.Clear();
                await ShowListOfPeopleAsync();
                break;
            default:
                System.Console.Clear();
                System.Console.WriteLine("Invalid Key Selection");
                System.Console.WriteLine("Returning to list of people...");
                Thread.Sleep(1000);
                await ShowListOfPeopleAsync();
                break;
        }
    }
}