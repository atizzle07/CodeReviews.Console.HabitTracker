using Spectre.Console;

namespace Habit_Tracker_App;
public class Program
{
    static void Main(string[] args)
    {
        DataConnection conn = new();
        string userChoice;

        ConsoleUI.WelcomeMessage();
        userChoice = ConsoleUI.GetMenuChoice();

        switch (userChoice)
        {
            case "1":
                // Add entry
                AnsiConsole.MarkupLine("[bold red]You reached the Add entry Method![/]");
                break;
            case "2":
                // View saved entries
                AnsiConsole.MarkupLine("[bold red]You reached the View saved entries Method![/]");
                break;
            case "3":
                // Update an entry
                AnsiConsole.MarkupLine("[bold red]You reached the Update entry Method![/]");
                break;
            case "4":
                // Delete an entry
                AnsiConsole.MarkupLine("[bold red]You reached the Delete entry Method![/]");
                break;
            case "x":
                // Exit application
                AnsiConsole.MarkupLine("[bold red]You reached the Exit application Method![/]");
                break;
            default:
                AnsiConsole.MarkupLine("[bold red]Error - Invalid entry[/]");
                break;
        }
    }
}