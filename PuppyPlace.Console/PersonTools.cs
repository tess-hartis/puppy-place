using FluentValidation.Results;
using PuppyPlace.Domain;
using PuppyPlace.Repository;
using Spectre.Console;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace PuppyPlace.Console;

public class PersonTools
{
    private readonly Prompt _prompt;
    private PersonRepository _personRepository;
    private DogRepository _dogRepository;
    private AdoptionService _adoptionService; 
    public PersonTools(Prompt prompt, PersonRepository personRepository, DogRepository dogRepository, AdoptionService adoptionService)
    {
        _prompt = prompt;
        _personRepository = personRepository;
        _dogRepository = dogRepository;
        _adoptionService = adoptionService;
    }

    public PersonValidator PersonValidator = new PersonValidator();
    private List<string> errors = new List<string>();

    public async Task AddPerson()
    {
        System.Console.Clear();
        errors.Clear();
        var name = AnsiConsole.Ask<string>("What is the person's [green]name[/]?");
        var person = new Person(name);
        ValidationResult results = PersonValidator.Validate(person);
        
        if (results.IsValid == false)
        {
            foreach (ValidationFailure failure in results.Errors)
            {
                errors.Add($"{failure.ErrorMessage}");
            }

            foreach (var error in errors)
            {
                System.Console.WriteLine($"Name {error}");
            }
            
            Thread.Sleep(1500);
            await AddPerson();
        }
        
        System.Console.Clear();
        System.Console.WriteLine($"The following person has been successfully created: {person.Name}" );
       
        await _personRepository.AddPersonAsync(person);
        await PromptToAddAnotherPerson();
    }

    async Task PromptToAddAnotherPerson()
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
        var persons = await _personRepository.PersonsAsync();
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
                        await ShowPerson(personId);
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
        // int chosenPerson = 0;
        // if (char.IsDigit(key.KeyChar))
        // {
        //     chosenPerson = int.Parse(key.KeyChar.ToString());
        // }
        //
        // else
        // {
        //     System.Console.WriteLine("Enter a number");
        //     Thread.Sleep(1000);
        //     ShowListOfPeople();
        // }
        // switch (chosenPerson)
        // {
        //     case int when (chosenPerson >= 1 && chosenPerson <= 9):
        //         var personId = persons[chosenPerson - 1].Id;
        //         ShowPerson(personId);
        //         break;
        //     default:
        //         ShowListOfPeople();
        //         break;
        // }
        // This will become a switch statement [Anthony]
        // if (char.IsDigit(key.KeyChar))
        // {
        //     integerChosenPerson = int.Parse(key.KeyChar.ToString());
        //     var personId = persons[integerChosenPerson - 1].Id;
        //     ShowPerson(personId);
        // }
        //
        // if (key.Key == ConsoleKey.M)
        // {
        //     Prompt.ReturnToMainMenu();
        // }
        //
        // if (!char.IsDigit(key.KeyChar) && key.Key != ConsoleKey.M)
        // {
        //     System.Console.WriteLine("\nPlease enter a number");
        //     Thread.Sleep(2000);
        //     ShowListOfPeople();
        // }

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
            var person = await _personRepository.FindByIdAsync(id);
            System.Console.WriteLine($"Getting {person.Name}'s information...");
            Thread.Sleep(1000);
            System.Console.Clear();
            System.Console.WriteLine($"Name: {person.Name}");
            System.Console.WriteLine($"ID: {person.Id}");
            System.Console.WriteLine("Dogs:");
            if (person.Dogs.Count > 0)
            {
                foreach (var dog in person.Dogs)
                {
                    System.Console.WriteLine(dog.Name);
                }
            }
            else
            {
                System.Console.WriteLine($"{person.Name} has no dogs to show");
            }

            System.Console.WriteLine("\nWhat would you like to do?" +
                                     "\n" +
                                     "\n(A)dopt a dog (E)dit Name (D)elete Person (S)how People (M)ain Menu");

            var userInput = System.Console.ReadKey();
            switch (userInput.Key)
            {
                case ConsoleKey.A:
                    System.Console.WriteLine($"Let's give {person.Name} an owner!");
                    await AdoptDog(person);
                    break;
                case ConsoleKey.D:
                    await DeletePerson(person);
                    break;
                case ConsoleKey.S:
                    await ShowListOfPeopleAsync();
                    break;
                case ConsoleKey.E:
                    await UpdateName(person);
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
        var input = System.Console.ReadLine();

        if (!string.IsNullOrEmpty(input))
        {
            person.Name = input;
            await _personRepository.UpdateNameAsync(person);
            System.Console.Clear();
            System.Console.WriteLine("Name has been updated!");
            Thread.Sleep(1500);
        }
        else
        {
            System.Console.WriteLine("No good!");
            await UpdateName(person);
        }

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
        var dogs = await _dogRepository.DogsAsync();
        foreach (var dog in dogs)
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
            case ConsoleKey.L:
                await ShowListOfPeopleAsync();
                break;
            default:
                try
                {
                    if (isDigit)
                    {
                        var input = int.Parse(key.KeyChar.ToString());
                        var dog = dogs[input - 1];
                        await _adoptionService.AdoptDog(person.Id, dog.Id);
                        System.Console.Clear();
                        System.Console.WriteLine($"{person.Name} has adopted {dog.Name}!");
                        await PromptToAdoptAnotherDog();
                    }

                    if (!isDigit)
                    {
                        System.Console.WriteLine("Please enter a number");
                        await AdoptDog(person);
                    }

                }
                catch (ArgumentOutOfRangeException)
                {
                    System.Console.WriteLine("\n" +
                                             "\nDog not found");
                    Thread.Sleep(1000);
                    await AdoptDog(person);
                }

                break;
        }
    }

    // public void AdoptDog(Person specificcPerson)
    // {
    //     System.Console.Clear();
    //     System.Console.WriteLine("Select a dog to adopt them!" +
    //                              "\n");
    //     var dogCount = 1;
    //     foreach (var dog in DogTools.Dogs)
    //     {
    //         System.Console.WriteLine($"{dogCount} {dog.Name}");
    //         dogCount++;
    //     }
    //
    //     var selectedDog = int.Parse(System.Console.ReadLine());
    //     var dogIndex = selectedDog - 1;
    //     var adoptedDog = DogTools.Dogs[dogIndex];
    //     adoptedDog.Owner = specificcPerson;
    //     specificcPerson.Dogs.Add(adoptedDog);
    //     System.Console.Clear();
    //     System.Console.WriteLine($"{specificcPerson.Name} has adopted {adoptedDog.Name}! ");
    //     System.Console.WriteLine("====================================");
    //     PromptToAdoptAnotherDog();
    //   
    //     Prompt.ReturnToMainMenu();
    // }

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
                await _personRepository.RemovePersonAsync(person.Id);
                foreach (var dog in person.Dogs)
                {
                    dog.Owners = null;
                }

                System.Console.Clear();
                System.Console.WriteLine($"{person.Name} has been deleted.");
                Thread.Sleep(1000);
                await ShowListOfPeopleAsync();
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