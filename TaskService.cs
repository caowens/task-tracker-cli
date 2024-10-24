using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Text.Json;
using Pastel;
using TaskTracker.Models;
using TaskTracker.Utilities;

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

        /// <summary>
        /// If file exists from <see cref="TaskService.filePath"/> instance, then updates <see cref="TaskService.todos"/> to contain the corresponding <see cref="Todo"/>s.
        /// </summary>
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

        /// <summary>
        /// Helper method to standardize command output.
        /// </summary>
        /// <param name="command">The actionable command the user can use.</param>
        /// <param name="args">The arguments that should be used with the command.</param>
        /// <param name="description">Describes the use of the command.</param>
        public static void DisplayCommand(string command, string args, string description)
        {
            // Set a fixed width for command and arguments combined to ensure descriptions align.
            int totalWidth = 70; 
            int argWidth = 30;

            // Print the command and arguments separately with their own colors and fixed width alignment
            Console.WriteLine($"\t{(command.Pastel(Color.DarkViolet).PadRight(argWidth) + " " + args.Pastel(Color.Green)).PadRight(totalWidth)} {description}");
        }

        /// <summary>
        /// Displays all of the user's tasks.
        /// </summary>
        public void DisplayTodos()
        {
            Console.WriteLine(" ID\t\t\t\t\t\tDescription\t\t\tStatus\t\tCreated\t\t\t\tLast Updated");
            Console.WriteLine("----\t\t\t\t\t\t----\t\t\t\t----\t\t----\t\t\t\t----\t\t");

            foreach (Todo todo in todos)
            {
                Console.WriteLine(todo.ToString());
            }
        }

        /// <summary>
        /// Displays all of the user's tasks that match the given status.
        /// </summary>
        /// <param name="command">Status-specific command used to find which status to filter by.</param>
        public void DisplayTodos(string command)
        {
            List<Todo> filteredTodosByCommand = new List<Todo>();

            if (command == "todo" || command == "done") 
            {
                filteredTodosByCommand = todos.Where(todo => todo.Status == command).ToList();
                
                // orderedTodosByCommand = (List<Todo>)todos.OrderBy(x => x.Status == "todo");
            }
            else if (command == "in-progress")
            {
                filteredTodosByCommand = todos.Where(todo => todo.Status == "in progress").ToList();
            }
            else 
            {
                Console.WriteLine("Wrong command. Please use 'help' for more information.");
                return;
            }


            if (filteredTodosByCommand.Any())
            {
                Console.WriteLine(" ID\t\t\t\t\t\tDescription\t\t\tStatus\t\tCreated\t\t\t\tLast Updated");
                Console.WriteLine("----\t\t\t\t\t\t----\t\t\t\t----\t\t----\t\t\t\t----\t\t");

                foreach (Todo todo in filteredTodosByCommand)
                {
                    Console.WriteLine(todo.ToString());
                }
            }
            else 
            {
                Console.WriteLine("There are no tasks marked with that status.");
            }
        }

        /// <summary>
        /// Adds a task to the <see cref="TaskService.todos"/> and to the json file specified at the <see cref="TaskService.filePath"/> path.
        /// </summary>
        /// <param name="todo"></param>
        public void AddTodo(Todo todo)
        {
            todos.Add(todo);

            // Add task to json file
            string jsonString = JsonSerializer.Serialize(todos);
            File.WriteAllText(filePath, jsonString);

            Console.WriteLine("Task added successfully.");
        }

        /// <summary>
        /// Updates specified task with a new description.
        /// </summary>
        /// <param name="id"><see cref="Guid"/> for the task the user wants to update.</param>
        /// <param name="description">New description the user wants to change for the specified task.</param>
        public void UpdateTodo(Guid id, string description)
        {
            if (todos.Exists(x => x.Id == id)) 
            {
                Todo? todo = todos.Find(task => task.Id == id);
                if (todo != null)
                {
                    todo.Description = description;
                    todo.UpdatedAt = DateTime.Now;

                    // Add task to json file
                    string jsonString = JsonSerializer.Serialize(todos);
                    File.WriteAllText(filePath, jsonString);

                    Console.WriteLine("Task updated successfully.");
                }
            }
            else
            {
                Console.WriteLine("There are no tasks that match that Id.");
            }
        }

        /// <summary>
        /// Deletes a task from both the <see cref="TaskService.todos"/> and the json file specified at <see cref="TaskService.filePath"/>.
        /// </summary>
        /// <param name="id"><see cref="Guid"/> for the specific task the user wants to delete.</param>
        public void DeleteTodo(Guid id)
        {
            if (todos.Exists(x => x.Id == id)) 
            {
                todos.RemoveAll(x => x.Id == id);

                // Overwrite json file with new list
                string jsonString = JsonSerializer.Serialize(todos);
                File.WriteAllText(filePath, jsonString);

                Console.WriteLine("Task deleted successfully.");
            }
            else
            {
                Console.WriteLine("There are no tasks that match that Id.");
            }
        }

        /// <summary>
        /// Updates specified task's status to be whatever is the corresponding command.
        /// </summary>
        /// <param name="id"><see cref="Guid"/> for the specific task the user wants to update.</param>
        /// <param name="status">New status the user wants to add for the specified task.</param>
        public void ChangeStatus(Guid id, string status)
        {
            if (todos.Exists(x => x.Id == id)) 
            {
                Todo? todo = todos.Find(task => task.Id == id);
                if (todo != null)
                {
                    todo.Status = status;
                    todo.UpdatedAt = DateTime.Now;

                    // Add task to json file
                    string jsonString = JsonSerializer.Serialize(todos);
                    File.WriteAllText(filePath, jsonString);

                    Console.WriteLine("Task updated successfully.");
                }
            }
            else
            {
                Console.WriteLine("There are no tasks that match that Id.");
            }
        }

        /// <summary>
        /// Starts <see cref="TaskService"/> job to start the session for interaction with the CLI.
        /// </summary>
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
                            Console.WriteLine("Wrong number of arguments. Please provide at least one argument to the add command. You can use 'help' for more information.");
                            break;
                        }

                        Guid todoId = Guid.NewGuid();
                        string description = inputArgs[1];
                        Todo todo = new Todo(todoId, description);

                        AddTodo(todo);

                        Console.WriteLine("\nHere is your updated list of tasks:\n");
                        DisplayTodos();

                        break;
                    case "update":
                        if (inputArgs.Length != 3)
                        {
                            Console.WriteLine("Incorrect amount of arguments. Please provide both an Id and a description. You can use 'help' for more information.");
                        }
                        else 
                        {
                            bool isId = TaskTrackerExtensions.TryParseGuid(inputArgs[1], out Guid id);
                            if (isId)
                            {
                                UpdateTodo(id, inputArgs[2]);
                            }
                            else
                            {
                                Console.WriteLine("That is not a valid id. You can use the 'list' command to see the ID for the task you want to update.");
                            }
                        }
                        break;
                    case "delete":
                        if (inputArgs.Length != 2)
                        {
                            Console.WriteLine("Invalid number of arguments. Please provide only one ID. You can use 'help' for more information.");
                        }
                        else 
                        {
                            bool isId = TaskTrackerExtensions.TryParseGuid(inputArgs[1], out Guid id);
                            if (isId)
                            {
                                DeleteTodo(id);
                            }
                            else
                            {
                                Console.WriteLine("That is not a valid id. You can use the 'list' command to see the ID for the task you want to delete.");
                            }
                        }
                        break;
                    case "mark-in-progress":
                    case "mark-done":
                        if (inputArgs.Length != 2)
                        {
                            Console.WriteLine("Invalid number of arguments. Please provide only one ID. You can use 'help' for more information.");
                        }
                        else 
                        {
                            bool isId = TaskTrackerExtensions.TryParseGuid(inputArgs[1], out Guid id);
                            if (isId && inputArgs[0] == "mark-in-progress")
                            {
                                ChangeStatus(id, "in progress");
                            }
                            else if (isId && inputArgs[0] == "mark-done")
                            {
                                ChangeStatus(id, "done");
                            }
                            else
                            {
                                Console.WriteLine("That is not a valid id and/or command. You can use the 'list' command to see the ID for the task you want to delete.");
                            }
                        }
                        break;
                    case "list": // List all tasks
                        if (inputArgs.Length == 1)
                        {
                            DisplayTodos();
                        }
                        else if (inputArgs.Length > 2)
                        {
                            Console.WriteLine("Too many arguments given. You can use 'help' for more information.");
                        }
                        else 
                        {
                            if (inputArgs[1] == "todo" 
                                || inputArgs[1] == "in-progress"
                                || inputArgs[1] == "done")
                            {
                                DisplayTodos(inputArgs[1]);
                            }
                            else
                            {
                                Console.WriteLine("Wrong argument given. You can use 'help' for more information.");
                            }
                        }
                        break;
                    case "help":
                        Console.WriteLine("Here are all the available commands:");

                        DisplayCommand("add", "[task]", "Adds a new task to the task list.");
                        DisplayCommand("update", "[id] [task]", "Updates a task with the specified id and description.");
                        DisplayCommand("delete", "[id]", "Deletes a task with the specified id.");
                        DisplayCommand("mark-in-progress", "[id]", "Updates status of specified id to 'In progress'.");
                        DisplayCommand("mark-done", "[id]", "Updates status of specified id to 'Done'.");
                        DisplayCommand("list", "", "Displays all of the current tasks");
                        DisplayCommand("list", "todo", "Displays all of the current tasks with 'todo' status.");
                        DisplayCommand("list", "in-progress", "Displays all of the current tasks with 'in progress' status.");
                        DisplayCommand("list", "done", "Displays all of the current tasks with 'done' status.");
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