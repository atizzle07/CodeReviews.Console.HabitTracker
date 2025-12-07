# Habit Tracker Created by Atizzle07

A simple console-based habit tracker written in C#. 
It uses **SQLite** for data persistence and Spectre Console for basic UI enhancement

This project is based on the Habit Tracker exercise from **CSharp Academy**, with a few personal tweaks and improvements.

---
##  Features
- Add new records (date + quantity)
- View all  records in a formatted table
- Update and Delete existing entries
- Data stored locally in a SQLite database file
- Basic error handling and input validation
---
##  Tech Stack

- Application Type: .NET (Console App)
- Language: C#
- Database: SQLite
- Console UI: [Spectre.Console](https://spectreconsole.net/)
---
##  Project Files and Structure

- `Program.cs` – Application entry point and main menu loop.
- `Habit.cs` – Model class representing a habit record. Contains these properties:
	- `HabitId`, `Date`, `Quantity`).
- `DataConnection.cs` – Handles SQLite connection. Creates DB if it doesn't already exist.
- `MenuUI.cs` - Handles Main Menu display

---


