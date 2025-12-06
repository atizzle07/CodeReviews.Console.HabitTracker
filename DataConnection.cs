using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Habit_Tracker_App;

public class DataConnection
{
    string connectionString = "Data Source=Habit-Tracker.db";
    public DataConnection()
    {
        CreateTable();
    }

    public void CreateTable()
    {
        
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = @"CREATE TABLE IF NOT EXISTS drinking_water (
                                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                Date TEXT,
                                Quantity INTEGER
                                )";

            tableCmd.ExecuteNonQuery();

            connection.Close();
        }
    }
}
