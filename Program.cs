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
                Console.ReadLine();
                break;
            case "2": // View saved entries
                AnsiConsole.MarkupLine("[bold red]You reached the View saved entries Method![/]");
                ViewAllRecords();
                Console.ReadLine();
                break;
            case "3": // Update an entry
                AnsiConsole.MarkupLine("[bold red]You reached the Update entry Method![/]");
                UpdateRecord();
                Console.ReadLine();
                break;
            case "4": // Delete an entry
                AnsiConsole.MarkupLine("[bold red]You reached the Delete entry Method![/]");
                DeleteRecord();
                Console.ReadLine();
                break;
            case "x": // Exit application
                AnsiConsole.MarkupLine("[bold red]You reached the Exit application Method![/]");
                Console.ReadLine();
                break;
            default:
                AnsiConsole.MarkupLine("[bold red]Error - Invalid entry[/]");
                Console.ReadLine();
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
                command.CommandText = @$"SELECT * FROM {tableName}";
                command.ExecuteNonQuery();

                SqliteDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    tableData.Add( //EXCEPTION - "No data found for the specified row/column"
                        new Habit
                        {
                            HabitId = reader.GetInt32(0),
                            Date = reader.GetString(1),
                            Quantity = reader.GetInt32(2)
                        });
                }
                else
                {
                    AnsiConsole.MarkupLine($"[bold red]No records found[/]");
                    Console.ReadLine();
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
            //string answer;
            //do
            //{
                AnsiConsole.MarkupLine($"[bold orange3]You entered the following:[/]");
                Console.WriteLine($"Date: {inputDate}");
                Console.WriteLine($"Quantity: {inputQuantity}\n");
                AnsiConsole.MarkupLine($"[bold orange3]Press enter to continue...[/]");
                Console.Read();
            //    AnsiConsole.MarkupLine($"[bold orange3]Do you want to continue (Y/N)? [/]");
            //    answer = Console.ReadLine().ToLower();
            //} while (answer != "y");
            

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