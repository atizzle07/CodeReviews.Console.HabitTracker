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
        string userChoice = "";
        MenuUI.WelcomeMessage();

        while (userChoice != "x")
        {
            userChoice = MenuUI.GetMenuChoice();
            MenuUI.AddSpace(3);

            switch (userChoice)
            {
                case "1": // Add entry
                    InsertRecord();
                    Console.ReadLine();
                    break;
                case "2": // View saved entries
                    ViewAllRecords();
                    Console.ReadKey();
                    break;
                case "3": // Update an entry
                    UpdateRecord();
                    Console.ReadKey();
                    break;
                case "4": // Delete an entry
                    DeleteRecord();
                    Console.ReadKey();
                    break;
                case "x": // Exit application
                    AnsiConsole.MarkupLine("[bold red]You reached the Exit application Method! Press Enter to exit...[/]");
                    Console.ReadKey();
                    break;
                default:
                    AnsiConsole.MarkupLine("[bold red]Error - Invalid entry[/]");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private static void DeleteRecord()
    {
        AnsiConsole.MarkupLine("[bold red]**Under Construction**[/] This feature is not ready yet. Please make another selection");
        ViewAllRecords();

        string input;
        do
        {
            AnsiConsole.Markup("[bold orange3]Please select the ID of the record you would like to delete: [/]");
            input = Console.ReadLine();

            int.TryParse(input, out int validInput);
        }
        while (true);
        

    }

    private static void UpdateRecord()
    {
        AnsiConsole.MarkupLine("[bold red]**Under Construction**[/] This feature is not ready yet. Please make another selection");
    }

    static void ViewAllRecords()
    {
        List<Habit> tableData = GetAllRecords();

        //Write results to console.
        var recordsTable = new Spectre.Console.Table();
        recordsTable.AddColumn("[bold orange3]ID[/]")
                    .AddColumn("[bold orange3]Date[/]")
                    .AddColumn("[bold orange3]Glasses Drank[/]");

        foreach (Habit row in tableData)
        {
            recordsTable = recordsTable.AddRow(row.HabitId.ToString(), row.Date, row.Quantity.ToString());
        }
        AnsiConsole.Write(recordsTable);

        AnsiConsole.MarkupLine("[bold orange3]Press Enter to continue...[/]");
    }

    static List<Habit> GetAllRecords()
    {
        // This was moved out of ViewAllRecords method so it could also be used for record selection in other methods.
        List<Habit> tableData = new List<Habit>();
        try
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();
                var command = conn.CreateCommand();
                command.CommandText = @$"SELECT Id, Date, Quantity FROM {tableName}";

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            tableData.Add(new Habit
                            {
                                HabitId = reader.GetInt32(0),
                                Date = reader.GetString(1),
                                Quantity = reader.GetInt32(2)
                            });
                        }
                    }
                    else
                    {
                        AnsiConsole.MarkupLine($"[bold red]No records found[/]");
                        Console.ReadLine();
                    }
                }
                conn.Close();
                return tableData;
            }
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[bold red]An error occurred: {ex.Message}[/]");
            return new List<Habit>();
        }
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
            //IMPROVE - Need to add loop that asks user if they want to use their entries or enter new data
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
            AnsiConsole.MarkupLine("[bold orange3]Record Saved![/]");
            AnsiConsole.MarkupLine("[bold orange3]Details:[/]");
            AnsiConsole.MarkupLine($"[bold orange3]Date: [/]{inputDate}");
            AnsiConsole.MarkupLine($"[bold orange3]Quantity: [/]{inputQuantity}");
            AnsiConsole.MarkupLine($"[bold orange3]Date: [/]{inputDate}");

            AnsiConsole.MarkupLine("[bold orange3]Press Enter to continue...[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[bold red]An error occurred: {ex.Message}[/]");
        }
        
    }

    private static string GetDate()
    {
        // NOT WORKING - This does not cause an exception but allows 4 digits in the year column
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
        AnsiConsole.Markup("[green]Please enter whole number of glasses to log [/]");
        if (int.TryParse(Console.ReadLine(), out int quantity))
            return quantity;
        else
            AnsiConsole.MarkupLine("[bold red]Invalid Input. Please try again[/]");
            return 0;
    }
}