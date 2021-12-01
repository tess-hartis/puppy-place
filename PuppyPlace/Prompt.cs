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
                          "\n1 - Add new Person" +
                          "\n2 - Add new Dog" +
                          "\n3 - Show list of People" +
                          "\n4 - Show list of Dogs" +
                          "\n" +
                          "\n(Press q to quit)");
        var answer = Console.ReadLine();
        switch (answer)
        {
            case "1":
                PersonTools.AddPerson();
                break;
            case "2":
                DogTools.AddDog();
                break;
            case "3":
                PersonTools.ShowPeople();
                break;
            case "4":
                DogTools.ShowDogs();
                break;
            case "q":
                Console.WriteLine("You have entered q");
                break;
            default:
                MainMenu();
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

}