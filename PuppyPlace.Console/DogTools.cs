using FluentValidation.Results;
using PuppyPlace.Domain;
using PuppyPlace.Repository;

namespace PuppyPlace.Console;

public class DogTools
{
    private readonly Prompt _prompt;
    private readonly DogRepository _dogRepository;
    private readonly PersonRepository _personRepository;
    

    public DogTools(Prompt prompt, DogRepository dogRepository, PersonRepository personRepository)
    {
        _prompt = prompt;
        _dogRepository = dogRepository;
        _personRepository = personRepository;
        
    }

    public DogValidator DogValidator = new DogValidator();
    private List<string> errors = new List<string>();

    public async Task AddDog()
    {
        errors.Clear();
        System.Console.Clear();

        System.Console.WriteLine("Please insert the dog's name:");
        var dogName = System.Console.ReadLine();
        System.Console.Clear();
        Thread.Sleep(1000);
        
        System.Console.WriteLine($"Please insert the dog's age:");
        var newDogAge = System.Console.ReadLine();
        var age = 0;
        try
        {
            age = int.Parse(newDogAge);
        }
        catch (FormatException e)
        {
            System.Console.WriteLine("Dog's age must be a number!");
            Thread.Sleep(1000);
            await AddDog();
        }

        System.Console.Clear();
        Thread.Sleep(1000); 
        
        System.Console.WriteLine($"Please insert {dogName}'s breed:");
        var newDogBreed = System.Console.ReadLine();
        
        System.Console.Clear();
        Thread.Sleep(1000);

        var newDog = new Dog(dogName, age, newDogBreed);

        ValidationResult results = DogValidator.Validate(newDog);
        
        if (results.IsValid == false)
        {
            foreach (ValidationFailure failure in results.Errors)
            {
                errors.Add($"{failure.ErrorMessage}");
            }

            foreach (var error in errors)
            {
                System.Console.WriteLine($"{error}");
            }
            
            Thread.Sleep(1500);
            await AddDog();
        }
            
        System.Console.WriteLine("Success! We added the following information to the database:" +
                                 "\n==========================================================" +
                                 $"\nName: {newDog.Name}" +
                                 $"\nAge: {newDogAge}" +
                                 $"\nBreed: {newDogBreed}" +
                                 $"\n=========================================================");

       await _dogRepository.AddDogAsync(newDog);
        
        Thread.Sleep(1000);
        await PromptToAddAnotherDog();
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
            default:
                try
                {
                    if (isDigit)
                    {
                        var userInput = int.Parse(key.KeyChar.ToString());
                        var dogId = dogs[userInput - 1].Id;
                        await ShowDogAsync(dogId);
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

    public async Task ShowDogAsync(Guid id)
    {
        System.Console.Clear();
        var dog = await _dogRepository.FindByIdAsync(id);
        System.Console.WriteLine($"Name: {dog.Name}");
        System.Console.WriteLine($"Age: {dog.Age}");
        System.Console.WriteLine($"Breed: {dog.Breed}");
        System.Console.WriteLine("Owners:");

        if (dog.Owners.Count > 0)
        {
            foreach (var person in dog.Owners)
            {
                System.Console.WriteLine(person.Name);
            }
        }
        else
        {
            System.Console.WriteLine("doesn't have an owner yet!");
        }

        System.Console.WriteLine("\nWhat would you like to do?" +
                                 "\n" + 
                                 "\n(A)dd Owner (E)dit Name (D)elete Dog (L)ist of Dogs (M)ain Menu");
    
        var userInput = System.Console.ReadKey();
        switch (userInput.Key)
        {
            case ConsoleKey.A:
                System.Console.WriteLine($"Let's give {dog.Name} an owner!");
                await SelectDogOwner(dog);
                break;
            case ConsoleKey.E:
                await EditDogName(dog);
                break;
            case ConsoleKey.D:
                await DeleteDogAsync(dog);
                break;
            case ConsoleKey.L:
                await ShowListOfDogsAsync();
                break;
            case ConsoleKey.M:
                await Prompt.ReturnToMainMenu();
                break;
            default:
                System.Console.WriteLine("Invalid selection. Please select option number.");
                Thread.Sleep(1000);
                System.Console.Clear();
                await ShowListOfDogsAsync();
                break;
        }
    }
    public async Task SelectDogOwner(Dog dog)
    {
        System.Console.Clear();
        System.Console.WriteLine("Here are the people in the database:");
        System.Console.WriteLine($"Enter a number to give {dog.Name} an owner!");
        System.Console.WriteLine("(M)ain Menu (L)ist of Dogs");
        System.Console.WriteLine("===================================");
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
                        var owner = persons[userInput - 1];
                        await _adoptionService.AddOwner(owner.Id, dog.Id);
                        System.Console.Clear();
                        System.Console.WriteLine($"{owner.Name} is now {dog.Name}'s owner!");
                        Thread.Sleep(1000);
                        await ShowListOfDogsAsync();
                    }
    
                    if (!isDigit)
                    {
                        System.Console.WriteLine("Please enter a number");
                        Thread.Sleep(1000);
                        await SelectDogOwner(dog);
                    }
                }
                catch (ArgumentOutOfRangeException)
                {
                    System.Console.WriteLine("\n");
                    System.Console.WriteLine("Owner not found");
                    Thread.Sleep(1500);
                    await SelectDogOwner(dog);
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
                // if (dogToDelete.Owner != null)
                // {
                //     dogToDelete.Owner.Dogs.Remove(dogToDelete);
                //     _context.SaveChanges();
                // }
                await _dogRepository.RemoveDogAsync(dog.Id);
                System.Console.Clear();
                System.Console.WriteLine($"{dog.Name} has been deleted.");
                Thread.Sleep(1000);
                await ShowListOfDogsAsync();
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
    public async Task EditDogName(Dog dog)
    {
        System.Console.Clear();
        System.Console.WriteLine($"Enter a new name for {dog.Name}");
        var userInput = System.Console.ReadLine();

        if (!string.IsNullOrEmpty(userInput))
        {
            dog.Name = userInput;
            await _dogRepository.UpdateNameAsync(dog);
            System.Console.Clear();
            System.Console.WriteLine("Name has been updated!");
            Thread.Sleep(1000);
        }
        else
        {
            System.Console.WriteLine("No good!");
            await EditDogName(dog);
        }
        await Prompt.ReturnToMainMenu();
    }

    
}