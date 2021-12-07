namespace PuppyPlace;

public class Prompt
{
    public static void MainMenu()
    {
        Console.Clear();
        // Console.WriteLine(Figgle.FiggleFonts.Banner.Render("Puppy Place"));
        Console.WriteLine("MAIN MENU");
        Console.WriteLine("=========" +
                          "\n" +
                          "\nWhat would you like to do?" +
                          "\n" +
                          "\n1 - Add New Person" +
                          "\n2 - Add New Dog" +
                          "\n3 - Show List of People" +
                          "\n4 - Show List of Dogs" +
                          "\n" +
                          "\n(Press Q to Quit)");
        var answer = Console.ReadKey();
        switch (answer.Key)
        {
            case ConsoleKey.D1:
                PersonTools.AddPerson();
                break;
            case ConsoleKey.D2:
                DogTools.AddDog();
                break;
            case ConsoleKey.D3:
                PersonTools.ShowListOfPeople();
                break;
            case ConsoleKey.D4:
                DogTools.ShowListOfDogs();
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
        Console.Clear();
        Console.WriteLine("Are you sure you want to Quit? Press Q to continue. Press M to return to the Main Menu");
        var userInput = Console.ReadKey();
        switch (userInput.Key)
        {
            case ConsoleKey.Q:
                Console.Clear();
                System.Environment.ExitCode = 0;
                break;
            case ConsoleKey.M:
                ReturnToMainMenu();
                break;
            default:
                Console.WriteLine("Invalid selection");
                PromptToQuit();
                break;
        }
    }

    public static void ReturnToMainMenu()
    {
        Console.Clear();
        Console.WriteLine("Returning to Main Menu...");
        Thread.Sleep(1000);
        MainMenu();
    }

    public static void PromptToReturnToMainMenu()
    {
        Console.WriteLine("====================================");
  
        Console.WriteLine("Press 'M' to return to the Main Menu");
        var userInput = Console.ReadKey();
        switch (userInput.Key)
        {
            case ConsoleKey.M:
                ReturnToMainMenu();
                break;
            default:
                Console.WriteLine("Input invalid");
                Thread.Sleep(1000);
                PromptToReturnToMainMenu();
                break;
        }
    }
}