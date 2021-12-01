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
        // if (yesNo == "yes")
        // {
        //     AddPerson();
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
        //     PromptToAddAnotherPerson();
        // }
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
    
    private static readonly List<Person> Persons = new List<Person>()
    {
        new Person("tess"),
        new Person("anthony")
    };

    public static void ShowPeople()
    {
        foreach (var person in Persons)
        {
            Console.WriteLine(person.Name);
        }
    }

    public static void AddPersonsToList(Person person)
    {
        Persons.Add(person);
    }


}