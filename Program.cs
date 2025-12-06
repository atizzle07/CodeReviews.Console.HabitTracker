using Microsoft.Data.Sqlite;
using Spectre.Console;
using Spectre.Console.Rendering;
using System.Globalization;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

namespace Habit_Tracker_App;
public class Program
{
    static string databaseName = "Habit-Tracker";
    static string tableName = "drinking_water";
    static string connectionString = $"Data Source={databaseName}.db";
    DataConnection conn = new();
    static void Main(string[] args)
    {
        
        string userChoice;

        MenuUI.WelcomeMessage();
        userChoice = MenuUI.GetMenuChoice();

        switch (userChoice)
        {
            case "1": // Add entry
                AnsiConsole.MarkupLine("[bold red]You reached the Add entry Method![/]");
                InsertRecord();
                break;
            case "2": // View saved entries
                AnsiConsole.MarkupLine("[bold red]You reached the View saved entries Method![/]");
                ViewAllRecords();
                break;
            case "3": // Update an entry
                AnsiConsole.MarkupLine("[bold red]You reached the Update entry Method![/]");
                UpdateRecord();
                break;
            case "4": // Delete an entry
                AnsiConsole.MarkupLine("[bold red]You reached the Delete entry Method![/]");
                DeleteRecord();
                break;
            case "x": // Exit application
                AnsiConsole.MarkupLine("[bold red]You reached the Exit application Method![/]");
                break;
            default:
                AnsiConsole.MarkupLine("[bold red]Error - Invalid entry[/]");
                break;
        } 
    }

    private static void DeleteRecord()
    {
        throw new NotImplementedException();
    }

    private static void UpdateRecord()
    {
        throw new NotImplementedException();
    }

    private static void ViewAllRecords()
    {
        throw new NotImplementedException();
    }

    private static void InsertRecord()
    {
        string inputDate;
        int inputQuantity;
        bool exitLoop = false;
        //Get valid date entry
        do
        {
            inputDate = GetDate();
            if (inputDate == "")
                continue;
            else
                exitLoop = true;
        }
        while (exitLoop == false);
        //Get valid quantity entry
        do
        {
            inputQuantity = GetQuantity();
            if (inputQuantity == 0)
                continue;
            else
                exitLoop = true;
        }
        while (exitLoop == false);
        //Save to database
            //IMPROVE - Need to add a check that views the users inputs and asks if they want to proceed or edit their inputs.
        try
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();
                var command = conn.CreateCommand();
                command.CommandText = @$"INSERT INTO {tableName}(Date, Quantity) 
                                        VALUES('{inputDate}','{inputQuantity}')";
                command.ExecuteNonQuery();
                conn.Close();
            }

        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[bold red]An error occurred: {ex.Message}[/]");
        }
        
    }

    private static string GetDate()
    {
        AnsiConsole.Markup("[green]Please enter the date of your entry (MM-DD-YYYY): [/]");
        string? input = Console.ReadLine();

        if (Regex.IsMatch(input, @"\d{1,2}-\d{1,2}-\d{2}"))
            return input;
        else
            AnsiConsole.MarkupLine("[bold red]Invalid Input. Please try again[/]");
        return "";
    }

    private static int GetQuantity()
    {
        AnsiConsole.Write("[green]Please enter whole number of glasses to log [/]");
        if (int.TryParse(Console.ReadLine(), out int quantity))
            return quantity;
        else
            AnsiConsole.MarkupLine("[bold red]Invalid Input. Please try again[/]");
            return 0;
    }
}