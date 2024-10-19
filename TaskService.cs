using System;
using System.Drawing;
using System.Text.RegularExpressions;
using Pastel;

namespace TaskTracker.Service
{
    class TaskService
    {
        public static void Start()
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
                        }
                        break;
                    case "help":
                        Console.WriteLine("Here are all the available commands:");
                        Console.WriteLine($"\t{"help".Pastel(Color.DarkViolet)}\t\tShows available commands");
                        Console.WriteLine($"\t{"exit".Pastel(Color.DarkViolet)}\t\tExits the cli");
                        break;
                    default:
                        Console.WriteLine($"Unknown command: {command}. Type 'help' to see all available commands.");
                        break;
                }
            }
        }
    }
}