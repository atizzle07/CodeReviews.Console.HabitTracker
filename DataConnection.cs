using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;

namespace Habit_Tracker_App;

public class DataConnection
{
    static string databaseName = "Habit-Tracker";
    string tableName = "drinking_water";
    static string connectionString = $"Data Source={databaseName}.db";
    public DataConnection()
    {
        CreateTable();
    }

    public void CreateTable()
    {
        


        using (var conn = new SqliteConnection(connectionString))
        {
            conn.Open();
            var tableCmd = conn.CreateCommand();

            tableCmd.CommandText = @$"CREATE TABLE IF NOT EXISTS {tableName}(
                                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                Date TEXT,
                                Quantity INTEGER
                                )";

            tableCmd.ExecuteNonQuery();

            conn.Close();
        }
    }
}
