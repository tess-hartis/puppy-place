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
        // if (yesNo == "yes")
        // {
        //     AddDog();
        // }
        //
        // if (yesNo == "no")
        // {
        //     Prompt.MainMenu();
        // }
        //
        // if (yesNo != "yes" & yesNo != "no")
        // {
        //     Console.Clear();
        //     PromptToAddAnotherDog();
        // }

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
    
    public static void ShowDogs()
    {
        foreach (var dog in Dogs)
        {
            Console.WriteLine(dog.Name);
        }
    }

    public static void AddDogToList(Dog dog)
    {
        Dogs.Add(dog);
    }
}