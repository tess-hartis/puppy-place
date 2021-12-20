using Microsoft.EntityFrameworkCore;
using PuppyPlace.Data;
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

    public void AddDog()
    {
        System.Console.Clear();
        System.Console.WriteLine("You have chosen to add a dog!");
        Thread.Sleep(1000);
        
        System.Console.WriteLine("Please insert the dog's name:");
        var newDogName = System.Console.ReadLine();
        System.Console.Clear();
        Thread.Sleep(1000);
        
        System.Console.WriteLine($"Please insert {newDogName}'s age:");
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
            AddDog();
        }

        System.Console.Clear();
        Thread.Sleep(1000); 
        
        System.Console.WriteLine($"Please insert {newDogName}'s breed:");
        var newDogBreed = System.Console.ReadLine();
        
        System.Console.Clear();
        Thread.Sleep(1000);

        var newDog = new Dog(newDogName, age, newDogBreed);
            
        System.Console.WriteLine("Success! We added the following information to the database:" +
                                 "\n==========================================================" +
                                 $"\nName: {newDog.Name}" +
                                 $"\nAge: {newDogAge}" +
                                 $"\nBreed: {newDogBreed}" +
                                 $"\n=========================================================");

       _dogRepository.AddDog(newDog);
        
        Thread.Sleep(1000);
        PromptToAddAnotherDog();
    }

    void PromptToAddAnotherDog()
    {
        System.Console.WriteLine("Add another dog? Choose (Y)es or (N)o");
        var yesNo = System.Console.ReadKey();

        switch (yesNo.Key)
        {
            case ConsoleKey.Y:
                System.Console.Clear();
                AddDog();
                break;
            case ConsoleKey.N:
                Prompt.ReturnToMainMenu();
                break;
            default:
                System.Console.Clear();
                System.Console.WriteLine("Invalid option. Please type 'yes' or 'no'");
                PromptToAddAnotherDog();
                break;

        }
    }

    public void ShowListOfDogs()
    {
        System.Console.Clear();
        System.Console.WriteLine("Here are the dogs in the database:" +
                                 "\n(Enter a number to view a dog or (M)ain Menu)" +
                                 "\n====================================");
        var dogCount = 1;
        var dogs = _dogRepository.Dogs();
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
                Prompt.ReturnToMainMenu();
                break;
            default:
                try
                {
                    if (isDigit)
                    {
                        var userInput = int.Parse(key.KeyChar.ToString());
                        var dogId = dogs[userInput - 1].Id;
                        ShowDog(dogId);
                    }

                    if (!isDigit && key.Key != ConsoleKey.M)
                    {
                        System.Console.Clear();
                        System.Console.WriteLine("Enter a number or M for Main Menu");
                        Thread.Sleep(1000);
                        ShowListOfDogs();
                    }
                }
                catch (ArgumentOutOfRangeException e)
                {
                    System.Console.Clear();
                    System.Console.WriteLine("Enter a number or M for Main Menu");
                    Thread.Sleep(1000);
                    ShowListOfDogs();
                }
                break;
        }
        // int integerChosenDog;
        //
        // if (char.IsDigit(keyChosenDog.KeyChar))
        // {
        //     integerChosenDog = int.Parse(keyChosenDog.KeyChar.ToString());
        //     var dogId = dogs[integerChosenDog - 1].Id;
        //     ShowDog(dogId);
        // }
        //
        // if (keyChosenDog.Key == ConsoleKey.M)
        // {
        //     Prompt.ReturnToMainMenu();
        // }
        // else
        // {
        //     System.Console.WriteLine("\nPlease enter a number");
        //     Thread.Sleep(2000);
        //     ShowListOfDogs();
        // }
    }
    

    public void ShowDog(Guid id)
    {
        System.Console.Clear();
        var dog = _dogRepository.FindById(id);
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
            // case ConsoleKey.A:
            //     System.Console.WriteLine($"Let's give {dog.Name} an owner!");
            //     SelectDogOwner(dog);
            //     break;
            case ConsoleKey.E:
                EditDogName(dog);
                break;
            case ConsoleKey.D:
                DeleteDog(dog);
                break;
            case ConsoleKey.L:
                ShowListOfDogs();
                break;
            case ConsoleKey.M:
                Prompt.ReturnToMainMenu();
                break;
            default:
                System.Console.WriteLine("Invalid selection. Please select option number.");
                Thread.Sleep(1000);
                System.Console.Clear();
                ShowListOfDogs();
                break;
        }
    }
    // public void SelectDogOwner(Dog specificcDogg)
    // {
    //     System.Console.Clear();
    //     System.Console.WriteLine("Here are the people in the database:");
    //     System.Console.WriteLine($"Enter a number to give {specificcDogg.Name} an owner!");
    //     System.Console.WriteLine("(M)ain Menu (L)ist of Dogs");
    //     System.Console.WriteLine("===================================");
    //     var personCount = 1;
    //     var persons = _personRepository.Persons();
    //     foreach (var person in persons)
    //     {
    //         System.Console.WriteLine($"{personCount} {person.Name}");
    //         personCount++;
    //     }
    //
    //     var key = System.Console.ReadKey();
    //     bool isDigit = char.IsDigit(key.KeyChar);
    //     switch (key.Key)
    //     {
    //         case ConsoleKey.L:
    //             ShowListOfDogs();
    //             break;
    //         case ConsoleKey.M:
    //             Prompt.ReturnToMainMenu();
    //             break;
    //         default:
    //             try
    //             {
    //                 if (isDigit)
    //                 {
    //                     var userInput = int.Parse(key.KeyChar.ToString());
    //                     var owner = persons[userInput - 1];
    //                     specificcDogg.Owner = owner;
    //                     _context.SaveChanges();
    //                     System.Console.Clear();
    //                     System.Console.WriteLine($"{owner.Name} is now {specificcDogg.Name}'s owner!");
    //                     Thread.Sleep(1000);
    //                     ShowListOfDogs();
    //                 }
    //
    //                 if (!isDigit)
    //                 {
    //                     System.Console.WriteLine("Please enter a number");
    //                     Thread.Sleep(1000);
    //                     SelectDogOwner(specificcDogg);
    //                 }
    //             }
    //             catch (ArgumentOutOfRangeException)
    //             {
    //                 System.Console.WriteLine("\n");
    //                 System.Console.WriteLine("Owner not found");
    //                 Thread.Sleep(1500);
    //                 SelectDogOwner(specificcDogg);
    //             }
    //             break;
    //     }
        
        // var selectedOwner = int.Parse(System.Console.ReadLine());
        // var ownerIndex = selectedOwner - 1;
        // var owner = persons[ownerIndex];
        // specificcDogg.Owner = owner;
        // // owner.Dogs.Add(specificcDogg);
        // _context.SaveChanges();
        // System.Console.Clear();
        // System.Console.WriteLine($"{owner.Name} is now {specificcDogg.Name}'s owner!");
        // Thread.Sleep(1500);
        // Prompt.ReturnToMainMenu();
    // }
    
    

    public void DeleteDog(Dog dog)
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
                _dogRepository.DeleteDog(dog);
                System.Console.Clear();
                System.Console.WriteLine($"{dog.Name} has been deleted.");
                Thread.Sleep(1000);
                ShowListOfDogs();
                break;
            case ConsoleKey.N:
                System.Console.Clear();
                ShowListOfDogs();
                break;
            default:
                System.Console.Clear();
                System.Console.WriteLine("Invalid Key Selection");
                System.Console.WriteLine("Returning to list of dogs...");
                Thread.Sleep(1000);
                ShowListOfDogs();
                break;
        }
    }
    public void EditDogName(Dog dog)
    {
        System.Console.Clear();
        System.Console.WriteLine($"Enter a new name for {dog.Name}");
        var userInput = System.Console.ReadLine();

        if (!string.IsNullOrEmpty(userInput))
        {
            dog.Name = userInput;
            _dogRepository.UpdateName(dog);
            System.Console.Clear();
            System.Console.WriteLine("Name has been updated!");
            Thread.Sleep(1000);
        }
        else
        {
            System.Console.WriteLine("No good!");
            EditDogName(dog);
        }
        Prompt.ReturnToMainMenu();
    }

    
}