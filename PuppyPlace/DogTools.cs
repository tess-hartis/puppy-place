namespace PuppyPlace;

public static class DogTools
{
    public static void AddDog()
    {
        Console.Clear();
        Console.WriteLine("You have chosen to add a dog!");
        Thread.Sleep(1000);
        
        Console.WriteLine("Please insert the dog's name:");
        var newDogName = Console.ReadLine();
        Console.Clear();
        Thread.Sleep(1000);
        
        Console.WriteLine($"Please insert {newDogName}'s age:");
        var newDogAge = Console.ReadLine();
        Console.Clear();
        Thread.Sleep(1000); 
        
        Console.WriteLine($"Please insert {newDogName}'s breed:");
        var newDogBreed = Console.ReadLine();
        
        Console.Clear();
        Thread.Sleep(1000);

        var newDog = new Dog(newDogName, Int32.Parse(newDogAge), newDogBreed);
            
        Console.WriteLine("Success! We added the following information to the database:" +
                          "\n==========================================================" +
                          $"\nName: {newDog.Name}" +
                          $"\nAge: {newDogAge}" +
                          $"\nBreed: {newDogBreed}" +
                          $"\n=========================================================");
        
        AddDogToList(newDog);
        
        Thread.Sleep(1000);
        PromptToAddAnotherDog();
    }

    static void PromptToAddAnotherDog()
    {
        Console.WriteLine("Add another dog? Choose (Y)es or (N)o");
        var yesNo = Console.ReadKey();

        switch (yesNo.Key)
        {
            case ConsoleKey.Y:
                Console.Clear();
                AddDog();
                break;
            case ConsoleKey.N:
                Prompt.ReturnToMainMenu();
                break;
            default:
                Console.Clear();
                Console.WriteLine("Invalid option. Please type 'yes' or 'no'");
                PromptToAddAnotherDog();
                break;

        }
    }

    public static List<Dog> Dogs = new List<Dog>()
    {
         // Dogs[0]
         new Dog("Bucky", 15, "Fox Terrier"),
         //Dogs[1]
         new Dog("Speckles", 16, "Red Heeler")
    };
    
    public static void ShowListOfDogs()
    {
        Console.Clear();
        Console.WriteLine("Enter a number to select a dog and view options"+
                          "\n");
        var dogCount = 1;
        foreach (var dog in Dogs)
        {
            Console.WriteLine($"{dogCount} {dog.Name}");
            dogCount++;
        }
        Console.WriteLine("\n");
        Console.WriteLine("Press m for Main Menu");

        var chosenDog = Console.ReadLine();
        switch (chosenDog)
        {
            case "m":
                Prompt.ReturnToMainMenu();
                break;
            default:
                try
                {
                    var inputToInt = int.Parse(chosenDog);
                    ShowDog(inputToInt);
                }
                catch (Exception e)
                {
                    ShowListOfDogs();
                }
                break;
                
        }
       
    }

    public static void ShowDog(int chosenDog)
    {
        Console.Clear();
        var realIndex = chosenDog - 1;
        var dog = Dogs[realIndex];

        Console.WriteLine($"Name: {dog.Name}");
        Console.WriteLine($"Age: {dog.Age}");
        Console.WriteLine($"Breed: {dog.Breed}");
        
        // try
        // {
        //     Console.WriteLine($"Owners: {dog.Owner.Name}");
        // }
        // catch (Exception e)
        // {
        //     Console.WriteLine("Owner: doesn't have an owner yet!");
        // }

        if (dog.Owner is not null)
        {
            Console.WriteLine($"Owners: {dog.Owner.Name}");
        }
        else
        {
            Console.WriteLine("Owner: doesn't have an owner yet!");
        }

        Console.WriteLine("\nWhat would you like to do?" +
                          "\n" + 
                          "\n(A)dd Owner (D)elete Dog (M)ain Menu");
    
        var userInput = Console.ReadKey();
        switch (userInput.Key)
        {
            case ConsoleKey.A:
                Console.WriteLine($"Let's give {dog.Name} an owner!");
                SelectDogOwner(dog);
                break;
            case ConsoleKey.D:
                DeleteDog(dog);
                break;
            case ConsoleKey.M:
                Prompt.ReturnToMainMenu();
                break;
            default:
                Console.WriteLine("Invalid selection. Please select option number.");
                Thread.Sleep(1000);
                Console.Clear();
                ShowListOfDogs();
                break;
        }
    }
    public static void SelectDogOwner(Dog specificcDogg)
    {
        Console.Clear();
        var personCount = 1;
        foreach (var person in PersonTools.Persons)
        {
            Console.WriteLine($"{personCount} {person.Name}");
            personCount++;
        }

        var selectedOwner = int.Parse(Console.ReadLine());
        var ownerIndex = selectedOwner - 1;
        var owner = PersonTools.Persons[ownerIndex];
        specificcDogg.Owner = owner;
        owner.Dogs.Add(specificcDogg);
        Prompt.MainMenu();
    }
    
    public static void AddDogToList(Dog dog)
    {
        Dogs.Add(dog);
    }

    public static void DeleteDog(Dog dogToDelete)
    {
        Console.Clear();
        Console.WriteLine($"Are you sure you want to delete {dogToDelete.Name} from the database? (Y)es (N)o");
        var yesNo = Console.ReadKey();
        switch (yesNo.Key)
        {
            case ConsoleKey.Y:
                Dogs.Remove(dogToDelete);
                Console.Clear();
                Console.WriteLine($"{dogToDelete.Name} has been deleted.");
                Thread.Sleep(1500);
                ShowListOfDogs();
                break;
            case ConsoleKey.N:
                Console.Clear();
                ShowListOfDogs();
                break;
            default:
                Console.Clear();
                Console.WriteLine("Invalid Key Selection");
                Console.WriteLine("Returning to list of dogs...");
                Thread.Sleep(1000);
                ShowListOfDogs();
                break;
        }
    }

    
}