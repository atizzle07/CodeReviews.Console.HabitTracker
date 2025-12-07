using Habit_Tracker_App;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Text;

namespace Habit_Tracker_App;

public static class MenuUI
{
    public static void WelcomeMessage()
    {
        AnsiConsole.MarkupLine("[bold orange3]Welcome to the Habit Tracker Application![/]\n");
        AnsiConsole.MarkupLine("[bold orange3]In this application you will manage the drinking water habit. To Continue, please press Enter... [/]");
        Console.ReadKey();
    }

    public static string GetMenuChoice()
    {
        Console.Clear();
        string userInput;
 
        while (true)
        {
            var menuTitle = new Rule("[bold orange3]Main Menu[/]");
            menuTitle.Justification = Justify.Left;
            AnsiConsole.Write(menuTitle);
            Console.WriteLine();
            AnsiConsole.MarkupLine($"[green]1[/] - Add an entry");
            AnsiConsole.MarkupLine($"[green]2[/] - View saved entries");
            AnsiConsole.MarkupLine($"[green]3[/] - Update an entry");
            AnsiConsole.MarkupLine($"[green]4[/] - Delete an entry");
            AnsiConsole.MarkupLine($"[red]X[/] - Exit the application");
            AnsiConsole.Markup("[green]Your Selection: [/]");
            userInput = Console.ReadLine().ToLower();

            switch (userInput)
            {
                case "1":
                case "2":
                case "3":
                case "4":
                case "x":
                    return userInput;
                default:
                    AnsiConsole.MarkupLine("[red]Invalid entry. Please type a [/][green]menu choice[/]");
                    break;
            }
        }
    }
    public static void AddSpace(int number)
    {
        for (int i = 0; i < number; i++)
        {
            Console.WriteLine();
        }
    }
}
