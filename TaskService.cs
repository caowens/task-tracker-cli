using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Text.Json;
using Pastel;
using TaskTracker.Models;

namespace TaskTracker.Service
{
    class TaskService
    {
        private List<Todo> todos = new List<Todo>();
        private string filePath = @"tasks.json";

        public TaskService()
        {
            LoadTodosFromFile();
        }

        private void LoadTodosFromFile()
        {
            if (File.Exists(filePath))
            {
                string jsonString = File.ReadAllText(filePath);
                if (!string.IsNullOrEmpty(jsonString))
                {
                    todos = JsonSerializer.Deserialize<List<Todo>>(jsonString) ?? new List<Todo>();
                }
            }
        }

        // Helper method to standardize command output
        public static void DisplayCommand(string command, string args, string description)
        {
            // Set a fixed width for command and arguments combined to ensure descriptions align.
            int totalWidth = 70; 
            int argWidth = 30;

            // Print the command and arguments separately with their own colors and fixed width alignment
            Console.WriteLine($"\t{(command.Pastel(Color.DarkViolet).PadRight(argWidth) + " " + args.Pastel(Color.Green)).PadRight(totalWidth)} {description}");
        }

        public void DisplayTodos()
        {
            Console.WriteLine(" ID\t\tDescription\t\tStatus\t\tCreated\t\tLast Updated");
            Console.WriteLine("----\t\t----\t\t----\t\t----\t\t----\t\t");

            foreach (Todo todo in todos)
            {
                Console.WriteLine(todo.ToString());
            }
        }

        public void AddTodo(Todo todo)
        {
            todos.Add(todo);

            // Add task to json file
            string jsonString = JsonSerializer.Serialize(todos);
            File.WriteAllText(filePath, jsonString);

            Console.WriteLine("Task added successfully.");
        }

        public void Start()
        {
            bool running = true;

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n-----------------------\n");
            Console.WriteLine("Welcome to the Task Tracker cli!");
            Console.WriteLine("To see commonly used commands, type \"help\"");
            Console.WriteLine("To exit the cli, type \"exit\"");
            Console.WriteLine("\n-----------------------\n");
            Console.ForegroundColor = ConsoleColor.Yellow;

            while (running) 
            {
                Console.Write("task-cli ".Pastel(Color.Aqua));
                string? input = Console.ReadLine();

                // Use a regular expression to split input into command and arguments, supporting quotes for strings with spaces
                string pattern = @"(?<match>[\""].+?[\""]|\S+)";
                var matches = Regex.Matches(input, pattern);

                // Extract the command and arguments
                string[] inputArgs = new string[matches.Count];
                for (int i = 0; i < matches.Count; i++)
                {
                    inputArgs[i] = matches[i].Groups["match"].Value.Trim('"'); // Remove quotes from quoted strings
                }

                // If the user wants to exit, break the loop
                if (inputArgs.Length > 0 && inputArgs[0].ToLower() == "exit")
                {
                    running = false;
                    Console.WriteLine("Goodbye!");
                    continue;
                }

                // Process the input as commands and arguments
                if (inputArgs.Length == 0)
                {
                    Console.WriteLine("No command provided. Use 'help' to see available commands or 'exit' to exit the cli.");
                    continue;
                }

                string command = inputArgs[0].ToLower();

                switch (command) 
                {
                    case "add":
                        if (inputArgs.Length < 2) 
                        {
                            Console.WriteLine("Wrong number of arguments. Please provide at least one argument to the add command.");
                            break;
                        }

                        Guid todoId = Guid.NewGuid();
                        string description = inputArgs[1];
                        Todo todo = new Todo(todoId, description);

                        AddTodo(todo);

                        Console.WriteLine("\nHere is your updated list of tasks:\n");
                        DisplayTodos();

                        break;
                    case "help":
                        Console.WriteLine("Here are all the available commands:");

                        DisplayCommand("add", "[task]", "Adds a new task to the task list.");
                        DisplayCommand("help", "", "Shows available commands.");
                        DisplayCommand("exit", "", "Exits the CLI application.");

                        break;
                    default:
                        Console.WriteLine($"Unknown command: {command}. Type 'help' to see all available commands.");
                        break;
                }
            }
        }
    }
}