namespace PuppyPlace.Console;

public class Prompt
{
    private static PersonTools _personTools = DependencyInjection.PersonTools;
    private static readonly DogTools _dogTools = DependencyInjection.DogTools;

    // public Prompt(PersonTools personTools, DogTools dogTools)
    // {
    //     _personTools = personTools;
    //     _dogTools = dogTools;
    //
    // }
    public static void MainMenu()
    {
        System.Console.Clear();
        // Console.WriteLine(Figgle.FiggleFonts.Banner.Render("Puppy Place"));
        System.Console.WriteLine("MAIN MENU");
        System.Console.WriteLine("=========" +
                                 "\n" +
                                 "\nWhat would you like to do?" +
                                 "\n" +
                                 "\n1 - Add New Person" +
                                 "\n2 - Add New Dog" +
                                 "\n3 - Show List of People" +
                                 "\n4 - Show List of Dogs" +
                                 "\n" +
                                 "\n(Press Q to Quit)");
        var answer = System.Console.ReadKey();
        switch (answer.Key)
        {
            case ConsoleKey.D1:
                _personTools.AddPerson();
                break;
            case ConsoleKey.D2:
                _dogTools.ShowListOfDogs();
                break;
            case ConsoleKey.D3:
                _personTools.ShowListOfPeople();
                break;
            case ConsoleKey.D4:
                _dogTools.ShowListOfDogs();
                break;
            case ConsoleKey.Q:
                PromptToQuit();
                break;
            default:
                MainMenu();
                break;
            
        }

    }

    public static void PromptToQuit()
    {
        System.Console.Clear();
        System.Console.WriteLine("Are you sure you want to Quit? Press Q to continue. Press M to return to the Main Menu");
        var userInput = System.Console.ReadKey();
        switch (userInput.Key)
        {
            case ConsoleKey.Q:
                System.Console.Clear();
                System.Environment.ExitCode = 0;
                break;
            case ConsoleKey.M:
                ReturnToMainMenu();
                break;
            default:
                System.Console.WriteLine("Invalid selection");
                PromptToQuit();
                break;
        }
    }

    public static void ReturnToMainMenu()
    {
        System.Console.Clear();
        System.Console.WriteLine("Returning to Main Menu...");
        Thread.Sleep(1000);
        MainMenu();
    }

    public void PromptToReturnToMainMenu()
    {
        System.Console.WriteLine("====================================");
  
        System.Console.WriteLine("Press 'M' to return to the Main Menu");
        var userInput = System.Console.ReadKey();
        switch (userInput.Key)
        {
            case ConsoleKey.M:
                ReturnToMainMenu();
                break;
            default:
                System.Console.WriteLine("Input invalid");
                Thread.Sleep(1000);
                PromptToReturnToMainMenu();
                break;
        }
    }
}