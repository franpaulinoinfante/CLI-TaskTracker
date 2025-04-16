using System;

namespace CLI_TaskTracker.ConsoleApp.Models;

public class CommandParser
{
    // El comando list puede tener uno de los siguientes argumentos : [done, todo, in-progres]
    private static string[] ValidCommands = ["add", "update", "delete", "mark-in-progress", "mark-done", "list", "salir"];

    internal static void DisplayCommands()
    {
        Console.WriteLine("Los comandos que puede ejecutar son:");
        foreach (var command in ValidCommands)
        {
            Console.Write($"{command} - ");
        }

        Console.WriteLine();
    }

    internal (string command, string[] parameters) Parser(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            throw new ArgumentException("Debe introducir un comando");
        }

        string[] arguments = input.Split(' ', 2);
        if (!IsValidCommand(arguments))
        {
            throw new InvalidOperationException("El comando introducido no es valido");
        }

        string command = arguments[0];
        string[] parameters = GetParameters(command, arguments.Length > 1 ? arguments[1] : "");

        return (arguments[0], parameters);
    }

    private bool IsValidCommand(string[] arguments)
    {
        foreach (var command in ValidCommands)
        {
            if ((command == arguments[0]) && (arguments.Length > 0))
            {
                return true;
            }
        }

        return false;
    }

    private string[] GetParameters(string command, string arguments) 
    {
        switch (command)
        {
            case "add":
                return arguments.Length > 0 ? [arguments] : throw new ArgumentException("El comando 'add' debe tener una descripción.");
            case "update":
                string[] parameters = arguments.Split(' ', 2);
                if (parameters.Length != 2)
                {
                    throw new ArgumentException("El comando 'update' debe tener dos parametros.");
                }
                return arguments.Split(' ', 2);
            case "delete":
                return [arguments];
            case "mark-in-prograss":
                return [arguments];
            case "mark-done":
                return [arguments];
            case "list":
                return [arguments];
            default:
                return [];
        };
    }
}
