using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PuppyPlace.Data;
using PuppyPlace.Domain;
using PuppyPlace.Repository;

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

    public void AddPerson()
    {
        System.Console.Clear();
        System.Console.WriteLine("Great! Let's add a new Person!");
        Thread.Sleep(1000);
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
            _personRepository.AddPerson(newPerson);
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

    public void ShowListOfPeople()
    {
        System.Console.Clear();
        System.Console.WriteLine("Here are the people in the database:" +
                                 "\n(Enter a number to view a person or (M)ain Menu)" +
                                 "\n====================================");
        var personCount = 1;
        var persons = _personRepository.Persons();
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
                Prompt.ReturnToMainMenu();
                break;

            default:
                try
                {
                    if (isDigit)
                    {
                        var userInput = int.Parse(key.KeyChar.ToString());
                        var personId = persons.ElementAtOrDefault(userInput - 1).Id;
                        ShowPerson(personId);
                    }

                    if (!isDigit && key.Key != ConsoleKey.M)
                    {
                        System.Console.Clear();
                        System.Console.WriteLine("Enter a number or M for Main Menu");
                        Thread.Sleep(1000);
                        ShowListOfPeople();
                    }
                }
                catch (ArgumentOutOfRangeException e)
                {
                    System.Console.Clear();
                    System.Console.WriteLine("Enter a number or M for Main Menu");
                    Thread.Sleep(1000);
                    ShowListOfPeople();
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

    public void ShowPerson(Guid id)
    {
        System.Console.Clear();
        try
        {
            var person = _personRepository.FindById(id);
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
                    AdoptDog(person);
                    break;
                case ConsoleKey.D:
                    DeletePerson(person);
                    break;
                case ConsoleKey.S:
                    ShowListOfPeople();
                    break;
                case ConsoleKey.E:
                    UpdateName(person);
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

    public void UpdateName(Person person)
    {
        System.Console.Clear();
        System.Console.WriteLine($"Enter a new name for {person.Name}");
        var input = System.Console.ReadLine();

        if (!string.IsNullOrEmpty(input))
        {
            person.Name = input;
            _personRepository.UpdateName(person);
            System.Console.Clear();
            System.Console.WriteLine("Name has been updated!");
            Thread.Sleep(1500);
        }
        else
        {
            System.Console.WriteLine("No good!");
            UpdateName(person);
        }

        Prompt.ReturnToMainMenu();
    }

    public void AdoptDog(Person person)
    {
        System.Console.Clear();
        System.Console.WriteLine("Here are the dogs in the database:" +
                                 "\nEnter a number to adopt a dog!" +
                                 "\n(M)ain Menu (L)ist of People" +
                                 "\n==============================");
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
            case ConsoleKey.L:
                ShowListOfPeople();
                break;
            default:
                try
                {
                    if (isDigit)
                    {
                        var input = int.Parse(key.KeyChar.ToString());
                        var dog = dogs[input - 1];
                        _adoptionService.Adopt(person.Id, dog.Id);
                        System.Console.Clear();
                        System.Console.WriteLine($"{person.Name} has adopted {dog.Name}!");
                        PromptToAdoptAnotherDog();
                    }

                    if (!isDigit)
                    {
                        System.Console.WriteLine("Please enter a number");
                        AdoptDog(person);
                    }

                }
                catch (ArgumentOutOfRangeException)
                {
                    System.Console.WriteLine("\n" +
                                             "\nDog not found");
                    Thread.Sleep(1000);
                    AdoptDog(person);
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

    public void PromptToAdoptAnotherDog()
    {
        System.Console.WriteLine("(A)dopt Another Dog (R)eturn to Main Menu");
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

    public void DeletePerson(Person person)
    {
        System.Console.Clear();
        System.Console.WriteLine(
            $"Are you sure you want to delete {person.Name} from the database? (Y)es (N)o");
        var yesNo = System.Console.ReadKey();
        switch (yesNo.Key)
        {
            case ConsoleKey.Y:
                _personRepository.DeletePerson(person);
                foreach (var dog in person.Dogs)
                {
                    dog.Owners = null;
                }

                System.Console.Clear();
                System.Console.WriteLine($"{person.Name} has been deleted.");
                Thread.Sleep(1000);
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