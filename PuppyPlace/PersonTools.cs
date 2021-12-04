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
        Console.WriteLine("Add another person? Choose: yes/no");
        var yesNo = Console.ReadLine();
        
        switch (yesNo)
        {
            case "yes":
                AddPerson();
                break;
            case "no":
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
        var personCount = 1;
        foreach (var person in Persons)
        {
            Console.WriteLine($"{personCount} {person.Name}");
            personCount++;
        }
        var chosenPerson = Console.ReadLine();
        try
        {
            var inputToInt = int.Parse(chosenPerson);
            ShowPerson(inputToInt);
        }
        catch (Exception e)
        {
            Prompt.MainMenu();
        }
        
    }
    
    public static void ShowPerson( int chosenPerson)
    {
        Console.Clear();
        var realIndex = chosenPerson - 1;
        try
        {
            var personRealIndex= Persons[realIndex];

            Console.WriteLine($"Getting {personRealIndex.Name}'s information...");
            Thread.Sleep(1000);
            Console.Clear();
            Console.WriteLine($"Name: {personRealIndex.Name}");
            Console.WriteLine($"ID: {personRealIndex.Id}");
            Console.WriteLine($"Dogs: {personRealIndex.Dogs}");
            Prompt.PromptToReturnToMainMenu();
        }
        catch (Exception e)
        {
            Prompt.MainMenu();
        }
    }
    
    public static void AddPersonsToList(Person person)
    {
        Persons.Add(person);
    }


}