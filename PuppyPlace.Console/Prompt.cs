using Spectre.Console;

namespace PuppyPlace.Console;

public class Prompt
{
    private static PersonTools _personTools = DependencyInjection.PersonTools;
    private static readonly DogTools _dogTools = DependencyInjection.DogTools;
    
    public static async Task MainMenu()
    {
        System.Console.Clear();
        var font = FigletFont.Default;
        AnsiConsole.Write(new FigletText(font, "Puppy Place")
            .LeftAligned()
            .Color(Color.Blue));
    
        var table = new Table().LeftAligned().Border(TableBorder.Rounded);
        await AnsiConsole.Live(table)
            .StartAsync(async ctx =>
            {
                table.AddColumn("[blue]MAIN MENU[/]");
                ctx.Refresh();
                await Task.Delay(250);

                table.AddRow("[green]1 - Add Person[/]");
                ctx.Refresh();
                await Task.Delay(250);
                
                table.AddRow("[blue]2 - Add Dog[/]");
                ctx.Refresh();
                await Task.Delay(250);
                
                table.AddRow("[green]3 - View People[/]");
                ctx.Refresh();
                await Task.Delay(250);
                
                table.AddRow("[blue]4 - View Dogs[/]");
                ctx.Refresh();
                await Task.Delay(250);
                
                table.AddRow("[green]===========[/]");
                ctx.Refresh();
                await Task.Delay(250);
                
                table.AddRow("[blue]Press Q to Quit[/]");
                ctx.Refresh();
                await Task.Delay(250);

            });
        var answer = System.Console.ReadKey();
        switch (answer.Key)
        {
            case ConsoleKey.D1:
                await _personTools.AddPerson();
                break;
            case ConsoleKey.D2:
                await _dogTools.AddDog();
                break;
            case ConsoleKey.D3:
                await _personTools.ShowListOfPeopleAsync();
                break;
            case ConsoleKey.D4:
                await _dogTools.ShowListOfDogsAsync();
                break;
            case ConsoleKey.Q:
                await PromptToQuitAsync();
                break;
            default:
                await MainMenu();
                break;
            
        }

    }

    public static async Task PromptToQuitAsync()
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
                await ReturnToMainMenu();
                break;
            default:
                System.Console.WriteLine("Invalid selection");
                await PromptToQuitAsync();
                break;
        }
    }

    public static async Task ReturnToMainMenu()
    {
        System.Console.Clear();
        System.Console.WriteLine("Returning to Main Menu...");
        Thread.Sleep(1000);
        await MainMenu();
    }

    public async Task PromptToReturnToMainMenu()
    {
        System.Console.WriteLine("====================================");
  
        System.Console.WriteLine("Press 'M' to return to the Main Menu");
        var userInput = System.Console.ReadKey();
        switch (userInput.Key)
        {
            case ConsoleKey.M:
                await ReturnToMainMenu();
                break;
            default:
                System.Console.WriteLine("Input invalid");
                Thread.Sleep(1000);
                await PromptToReturnToMainMenu();
                break;
        }
    }
}