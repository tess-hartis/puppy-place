using PuppyPlace.Data;
using PuppyPlace.Domain;

namespace PuppyPlace.Console;

public class PersonTools
{
    private readonly Prompt _prompt;
    private readonly PuppyPlaceContext _context;
    public PersonTools(Prompt prompt, PuppyPlaceContext puppyPlaceContext)
    {
        _prompt = prompt;
        _context = puppyPlaceContext;
    }
    
    public void AddPerson()
    {
        System.Console.Clear();
        System.Console.WriteLine("Great! Let's add a new Person!");
        Thread.Sleep(2000);
        System.Console.Clear();
        System.Console.WriteLine("Please enter the person's name:");
        var userinput = System.Console.ReadLine();

        if (userinput is not null)
        {
            var newPerson = new Person(userinput);
            System.Console.Clear();
            System.Console.WriteLine($"The following person has been created:");
            System.Console.WriteLine("==============================================");
            System.Console.WriteLine($"Name: {newPerson.Name}");
            System.Console.WriteLine("==============================================");
        
            _context.Persons.Add(newPerson);
            _context.SaveChanges();
        }
        
        PromptToAddAnotherPerson();
        
    }
    void PromptToAddAnotherPerson()
    {
        System.Console.WriteLine("Add another person? (Y)es (N)o");
        var yesNo = System.Console.ReadKey();
        
        switch (yesNo.Key)
        {
            case ConsoleKey.Y:
                AddPerson();
                break;
            case ConsoleKey.N:
                Prompt.ReturnToMainMenu();
                break;
            default:
                System.Console.Clear();
                System.Console.WriteLine("Invalid option. Please type 'yes' or 'no'");
                PromptToAddAnotherPerson();
                break;
        }
    }
    
   public static readonly List<Person> Persons = new List<Person>()
    {
        new Person("tess"),
        new Person("anthony")
    };

   public void ShowListOfPeople()
   {
       System.Console.Clear();
       System.Console.WriteLine("Here are the people in the database:" +
                                "\n(Enter a number to view a person or (M)ain Menu)" +
                                "\n====================================");
       var personCount = 1;
       var persons = _context.Persons.ToList();
       foreach (var person in persons)
       {
           System.Console.WriteLine($"{personCount} {person.Name}");
           personCount++;
       }

       var keyChosenPerson = System.Console.ReadKey();
       int integerChosenPerson;

        // This will become a switch statement [Anthony]
       if (char.IsDigit(keyChosenPerson.KeyChar))
       {
           integerChosenPerson = int.Parse(keyChosenPerson.KeyChar.ToString());
           var personId = persons[integerChosenPerson - 1].Id;
           ShowPerson(personId);
       }

       if (keyChosenPerson.Key == ConsoleKey.M)
       {
           Prompt.ReturnToMainMenu();
       }
       
       if (!char.IsDigit(keyChosenPerson.KeyChar) && keyChosenPerson.Key != ConsoleKey.M)
       {
           System.Console.WriteLine("\nPlease enter a number");
           Thread.Sleep(2000);
           ShowListOfPeople();
       }
   }

   public void ShowPerson(Guid id)
   {
       System.Console.Clear();
        try
        {
            var person = _context.Persons
                .Include(p => p.Dogs)
                .FirstOrDefault(p => p.Id == id);

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
                                     "\n(A)dopt a dog (E)dit Name (D)elete Person (M)ain Menu");
    
            var userInput = System.Console.ReadKey();
            switch (userInput.Key)
            {
                // case ConsoleKey.A:
                //     System.Console.WriteLine($"Let's give {personRealIndex.Name} an owner!");
                //     AdoptDog(personRealIndex);
                //     break;
                case ConsoleKey.D:
                    DeletePerson(person);
                    break;
                case ConsoleKey.E:
                    EditPersonName(person);
                    break;
                case ConsoleKey.M:
                    Prompt.ReturnToMainMenu();
                    break;
                default:
                    System.Console.WriteLine("Invalid selection. Please select option number.");
                    Thread.Sleep(1000);
                    System.Console.Clear();
                    ShowListOfPeople();
                    break;
            }
        }
        catch (Exception e)
        {
            Prompt.MainMenu();
        }
    }

    public void EditPersonName(Person person)
    {
        System.Console.Clear();
        System.Console.WriteLine($"Enter a new name for {person.Name}");
        var userInput = System.Console.ReadLine();

        if (!string.IsNullOrEmpty(userInput))
        {
            person.Name = userInput;
            System.Console.Clear();
            System.Console.WriteLine("Name has been updated!");
            Thread.Sleep(1500);
        }
        else
        {
            System.Console.WriteLine("No good!");
            EditPersonName(person);
        }
        Prompt.ReturnToMainMenu();
    }

    public void AdoptDog(Person specificcPerson)
    {
        System.Console.Clear();
        System.Console.WriteLine("Select a dog to adopt them!" +
                                 "\n");
        var dogCount = 1;
        foreach (var dog in DogTools.Dogs)
        {
            System.Console.WriteLine($"{dogCount} {dog.Name}");
            dogCount++;
        }

        var selectedDog = int.Parse(System.Console.ReadLine());
        var dogIndex = selectedDog - 1;
        var adoptedDog = DogTools.Dogs[dogIndex];
        adoptedDog.Owner = specificcPerson;
        specificcPerson.Dogs.Add(adoptedDog);
        System.Console.Clear();
        System.Console.WriteLine($"{specificcPerson.Name} has adopted {adoptedDog.Name}! ");
        System.Console.WriteLine("====================================");
        PromptToAdoptAnotherDog();
      
        Prompt.ReturnToMainMenu();
    }

    public void PromptToAdoptAnotherDog()
    {
        System.Console.WriteLine("(A)dopt Another Dog (R)eturn to Main Menu" );
        var userInput = System.Console.ReadKey();
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

    public void DeletePerson(Person personToDelete)
    {
        System.Console.Clear();
        System.Console.WriteLine($"Are you sure you want to delete {personToDelete.Name} from the database? (Y)es (N)o");
        var yesNo = System.Console.ReadKey();
        switch (yesNo.Key)
        {
            case ConsoleKey.Y:
                _context.Persons.Remove(personToDelete);
                _context.SaveChanges();
                foreach (var dog in personToDelete.Dogs)
                {
                    dog.Owner = null;
                }
                System.Console.Clear();
                System.Console.WriteLine($"{personToDelete.Name} has been deleted.");
                Thread.Sleep(1500);
                ShowListOfPeople();
                break;
            case ConsoleKey.N:
                System.Console.Clear();
                ShowListOfPeople();
                break;
            default:
                System.Console.Clear();
                System.Console.WriteLine("Invalid Key Selection");
                System.Console.WriteLine("Returning to list of people...");
                Thread.Sleep(1000);
                ShowListOfPeople();
                break;
        }

    }
}