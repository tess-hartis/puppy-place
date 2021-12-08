namespace PuppyPlace;

public static class PersonTools
{
    public static void AddPerson()
    {
        Console.Clear();
        Console.WriteLine("Great! Let's add a new Person!");
        Thread.Sleep(2000);
        Console.Clear();
        Console.WriteLine("Please enter the person's name:");
        var userinput = Console.ReadLine();
        var newPerson = new Person(userinput);
        Console.Clear();
        Console.WriteLine($"The following person has been created:");
        Console.WriteLine("==============================================");
        Console.WriteLine($"Name: {newPerson.Name}");
        Console.WriteLine("==============================================");
        
        AddPersonsToList(newPerson);

        Thread.Sleep(1000);
        PromptToAddAnotherPerson();
        
    }
    static void PromptToAddAnotherPerson()
    {
        Console.WriteLine("Add another person? (Y)es (N)o");
        var yesNo = Console.ReadKey();
        
        switch (yesNo.Key)
        {
            case ConsoleKey.Y:
                AddPerson();
                break;
            case ConsoleKey.N:
                Prompt.ReturnToMainMenu();
                break;
            default:
                Console.Clear();
                Console.WriteLine("Invalid option. Please type 'yes' or 'no'");
                PromptToAddAnotherPerson();
                break;
        }
    }
    
   public static readonly List<Person> Persons = new List<Person>()
    {
        new Person("tess"),
        new Person("anthony")
    };

   public static void ShowListOfPeople()
   {
       Console.Clear();
       Console.WriteLine("Here are the people in the database:" +
                         "\n(Enter a number to view a person or (M)ain Menu)" +
                         "\n====================================");
       var personCount = 1;
       foreach (var person in Persons)
       {
           Console.WriteLine($"{personCount} {person.Name}");
           personCount++;
       }

       var keyChosenPerson = Console.ReadKey();
       int integerChosenPerson;

        // This will become a switch statement [Anthony]
       if (char.IsDigit(keyChosenPerson.KeyChar))
       {
           integerChosenPerson = int.Parse(keyChosenPerson.KeyChar.ToString());
           ShowPerson(integerChosenPerson);
       }

       if (keyChosenPerson.Key == ConsoleKey.M)
       {
           Prompt.ReturnToMainMenu();
       }
       
       if (!char.IsDigit(keyChosenPerson.KeyChar) && keyChosenPerson.Key != ConsoleKey.M)
       {
           Console.WriteLine("\nPlease enter a number");
           Thread.Sleep(2000);
           ShowListOfPeople();
       }
   }

   public static void ShowPerson( int intChosenPerson)
    {
        Console.Clear();
        var realIndex = intChosenPerson - 1;
        try
        {
            var personRealIndex= Persons[realIndex];

            Console.WriteLine($"Getting {personRealIndex.Name}'s information...");
            Thread.Sleep(1000);
            Console.Clear();
            Console.WriteLine($"Name: {personRealIndex.Name}");
            Console.WriteLine($"ID: {personRealIndex.Id}");
            Console.WriteLine("Dogs:");
            if (personRealIndex.Dogs.Count > 0)
            {
                foreach (var dog in personRealIndex.Dogs)
                {
                    Console.WriteLine(dog.Name);
                }
            }
            else
            {
                Console.WriteLine($"{personRealIndex.Name} has no dogs to show");
            }
            
            Console.WriteLine("\nWhat would you like to do?" +
                              "\n" + 
                              "\n(A)dopt a dog (E)dit Name (D)elete Person (M)ain Menu");
    
            var userInput = Console.ReadKey();
            switch (userInput.Key)
            {
                case ConsoleKey.A:
                    Console.WriteLine($"Let's give {personRealIndex.Name} an owner!");
                    AdoptDog(personRealIndex);
                    break;
                case ConsoleKey.D:
                    DeletePerson(personRealIndex);
                    break;
                case ConsoleKey.E:
                    EditPersonName(personRealIndex);
                    break;
                case ConsoleKey.M:
                    Prompt.ReturnToMainMenu();
                    break;
                default:
                    Console.WriteLine("Invalid selection. Please select option number.");
                    Thread.Sleep(1000);
                    Console.Clear();
                    ShowListOfPeople();
                    break;
            }
        }
        catch (Exception e)
        {
            Prompt.MainMenu();
        }
    }

    public static void EditPersonName(Person person)
    {
        Console.Clear();
        Console.WriteLine($"Enter a new name for {person.Name}");
        var userInput = Console.ReadLine();

        if (!string.IsNullOrEmpty(userInput))
        {
            person.Name = userInput;
            Console.Clear();
            Console.WriteLine("Name has been updated!");
            Thread.Sleep(1500);
        }
        else
        {
            Console.WriteLine("No good!");
            EditPersonName(person);
        }
        Prompt.ReturnToMainMenu();
    }

    public static void AdoptDog(Person specificcPerson)
    {
        Console.Clear();
        Console.WriteLine("Select a dog to adopt them!" +
                          "\n");
        var dogCount = 1;
        foreach (var dog in DogTools.Dogs)
        {
            Console.WriteLine($"{dogCount} {dog.Name}");
            dogCount++;
        }

        var selectedDog = int.Parse(Console.ReadLine());
        var dogIndex = selectedDog - 1;
        var adoptedDog = DogTools.Dogs[dogIndex];
        adoptedDog.Owner = specificcPerson;
        specificcPerson.Dogs.Add(adoptedDog);
        Console.Clear();
        Console.WriteLine($"{specificcPerson.Name} has adopted {adoptedDog.Name}! ");
        Console.WriteLine("====================================");
        PromptToAdoptAnotherDog();
      
        Prompt.ReturnToMainMenu();
    }

    public static void PromptToAdoptAnotherDog()
    {
        Console.WriteLine("(A)dopt Another Dog (R)eturn to Main Menu" );
        var userInput = Console.ReadKey();
        switch (userInput.Key)
        {
            case ConsoleKey.A:
               ShowListOfPeople();
                break;
            case ConsoleKey.R:
                Prompt.ReturnToMainMenu();
                break;
            default:
                PromptToAdoptAnotherDog();
                break;
        }
    }
    public static void AddPersonsToList(Person person)
    {
        Persons.Add(person);
    }


    }
}