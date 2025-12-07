using Microsoft.Data.Sqlite;
using Spectre.Console;
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

        // Main Loop
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
                    AnsiConsole.MarkupLine("[bold orange3]Press Enter to continue...[/]");
                    Console.ReadKey();
                    break;
                case "3": // Update an entry
                    UpdateRecord();
                    AnsiConsole.MarkupLine("[bold orange3]Press Enter to continue...[/]");
                    Console.ReadKey();
                    break;
                case "4": // Delete an entry
                    DeleteRecord();
                    AnsiConsole.MarkupLine("[bold orange3]Press Enter to continue...[/]");
                    Console.ReadKey();
                    break;
                case "x": // Exit application
                    AnsiConsole.MarkupLine("[bold red]Press Enter to exit...[/]");
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
        int idSelection = GetValidRecordID();
        int rowCount;

        using (var conn = new SqliteConnection(connectionString))
        {
            conn.Open();
            var command = conn.CreateCommand();
            command.CommandText = @$"DELETE FROM {tableName}
                                     WHERE Id = '{idSelection}'";
            rowCount = command.ExecuteNonQuery();
            conn.Close();
        }
        AnsiConsole.Markup($"[bold orange3]You Deleted {rowCount} Row(s).[/]");
    }
    private static void UpdateRecord()
    {
        string inputDate;
        int inputQuantity;
        int rowsAffected;
        bool exitLoop = false;

        //Prompt and return valid record ID
        int idSelection = GetValidRecordID();
        GetSingleRecord(idSelection);

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

        using (var conn = new SqliteConnection(connectionString))
        {
            conn.Open();
            var command = conn.CreateCommand();
            command.CommandText = @$"UPDATE {tableName}
                                     SET Date = '{inputDate}', Quantity = '{inputQuantity}'
                                     WHERE Id = '{idSelection}'";
            rowsAffected = command.ExecuteNonQuery();
            conn.Close();
        }
        AnsiConsole.Markup($"[bold orange3]{rowsAffected} record updated.[/]");
    }
    private static void ViewAllRecords()
    {
        List<Habit> tableData = GetAllRecords();

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
    private static List<Habit> GetAllRecords()
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
    private static List<Habit> GetSingleRecord(int ID) //IMPROVE - Could create overload function with GetAllRecords if no parameters passed
    {
        List<Habit> tableData = new();
        try
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();
                var command = conn.CreateCommand();
                command.CommandText = @$"SELECT Id, Date, Quantity FROM {tableName}
                                         WHERE Id = {ID}";
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

        if (Regex.IsMatch(input, @"\d{2}-\d{2}-\d{4}"))
            return input;
        else
            AnsiConsole.MarkupLine("[bold red]Invalid Input. Please try again[/]");
        return "";
    }
    private static int GetQuantity() 
    {
        AnsiConsole.Markup("[green]Please enter whole number of glasses to log [/]");
        int quantity;

        while(!int.TryParse(Console.ReadLine(), out quantity))
        {
            AnsiConsole.MarkupLine("[bold red]Invalid Input. Please try again[/]");
        }
        return quantity;
    }
    private static int GetValidRecordID()
        {
            // Retrieve all record ID's from DB
            List<Habit> recordsTable = GetAllRecords();
            List<int> validRecord = new();
            foreach (Habit record in recordsTable)
            {
                validRecord.Add(record.HabitId);
            }
            ViewAllRecords();

            string input;
            int validInput;
            bool exitLoop = false;
            
            // Validate selection to current record ID's
            do
            {
                AnsiConsole.Markup("[bold orange3]Please select the ID of the record: [/]");
                input = Console.ReadLine();

                if (!int.TryParse(input, out validInput) || !validRecord.Contains(validInput))
                    AnsiConsole.MarkupLine("[bold red]Error - Invalid entry[/]");
                else
                    exitLoop = true;
            }
            while (exitLoop == false);
            return validInput;
        }
}