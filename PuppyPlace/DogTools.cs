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
        Console.WriteLine("Add another dog? Choose: yes/no");
        var yesNo = Console.ReadLine();

        switch (yesNo)
        {
            case "yes":
                Console.Clear();
                AddDog();
                break;
            case "no":
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

        var chosenDog = Console.ReadLine();
        try
        {
            var inputToInt = int.Parse(chosenDog);
            ShowDog(inputToInt);
        }
        catch (Exception e)
        {
            Prompt.MainMenu();
        }
        

    }

    public static void ShowDog( int chosenDog)
    {
        Console.Clear();
        var realIndex = chosenDog - 1;
        try
        {
            var dogRealIndex= Dogs[realIndex];

            Console.WriteLine($"Name: {dogRealIndex.Name}");
            Console.WriteLine($"Age: {dogRealIndex.Age}");
            Console.WriteLine($"Breed: {dogRealIndex.Breed}");
            Console.WriteLine($"Owners: {dogRealIndex.Owner}");
            Console.WriteLine("\nWhat would you like to do?" +
                              "\n" + 
                              "\n(1)Add Owner (2)Delete Dog (3)Main Menu");
            var userInput = int.Parse(Console.ReadLine());
            switch (userInput)
            {
                case 1:
                    Console.WriteLine($"Let's give {dogRealIndex.Name} an owner");
                    
                    break;
                case 2:
                    Console.Clear();
                    Console.WriteLine($"Are you sure you want to delete {dogRealIndex.Name} from the database? Type 'yes' or 'no'");
                    var yesNo = Console.ReadLine();
                    switch (yesNo)
                    {
                        case "yes":
                            Dogs.Remove(dogRealIndex);
                            Console.Clear();
                            Console.WriteLine($"{dogRealIndex.Name} has been deleted.");
                            Thread.Sleep(1500);
                            ShowListOfDogs();
                            break;
                        case "no":
                            Console.Clear();
                            ShowListOfDogs();
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Choose 'yes' or 'no'");
                            break;
                    }
                    break;
                case 3:
                    Prompt.ReturnToMainMenu();
                    break;

                default:
                    ShowListOfDogs();
                    break;
            }

        }
        catch (Exception e)
        {
            Prompt.MainMenu();
        }
        
    }
    
    
    public static void AddDogToList(Dog dog)
    {
        Dogs.Add(dog);
    }
    
    
}