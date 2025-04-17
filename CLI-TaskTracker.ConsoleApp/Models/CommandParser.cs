using System;
using System.Text.RegularExpressions;

namespace CLI_TaskTracker.ConsoleApp.Models;

public class CommandParser
{
    // El comando list puede tener uno de los siguientes argumentos : [done, todo, in-progres]
    private static string[] ValidCommands = ["add", "update", "delete", "mark-in-progress", "mark-done", "list", "salir"];

    internal void DisplayCommands()
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
        string command = arguments[0];
        if (!IsValidCommand(command))
        {
            throw new InvalidOperationException("El comando introducido no es valido");
        }

        string[] parameters = GetParameters(command, arguments.Length > 1 ? arguments[1] : "");

        return (command, parameters);
    }

    private bool IsValidCommand(string command)
    {
        foreach (var commandItem in ValidCommands)
        {
            if (commandItem == command)
            {
                return true;
            }
        }

        return false;
    }

    private string[] GetParameters(string command, string arguments)
    {
        string[] parameters;
        switch (command)
        {
            case "add":
                if (!IsValidArgument(arguments))
                {
                    throw new ArgumentException("el comand 'add', solo acepta un parametro 'descripción'.");
                }
                return [arguments];
            case "update":
                if (!IsValidArgumentsForUpdate(arguments))
                {
                    throw new ArgumentException("El comando 'update' debe tener doss parametros.");
                }
                return arguments.Split(' ', 2);
            case "delete":
                parameters = arguments.Split(' ');
                if (arguments.Length != 1 || !int.TryParse(parameters[0], out int _))
                {
                    throw new ArgumentException("El comando 'eliminar' solo acepta un parametro 'id'");
                }
                return [arguments];
            case "mark-in-progress":
                parameters = arguments.Split(' ');
                if (arguments.Length != 1 || !int.TryParse(parameters[0], out int _))
                {
                    throw new ArgumentException("El comando 'mark-in-progress' solo acepta un parametro 'id'");
                }
                return [arguments];
            case "mark-done":
                parameters = arguments.Split(' ');
                if (arguments.Length != 1 || !int.TryParse(parameters[0], out int _))
                {
                    throw new ArgumentException("El comando 'mark-in-progress' solo acepta un parametro 'id'");
                }
                return [arguments];
            case "list":
                return [arguments];
            default:
                return [];
        }
        ;
    }

    private static bool IsValidArgument(string arguments)
    {
        return Regex.IsMatch(arguments, @"^""[^""]+""\s*$");
    }

    private bool IsValidArgumentsForUpdate(string arguments)
    {
        var parameters = arguments.Split(' ', 2);
        if ((parameters.Length != 2) || !int.TryParse(parameters[0], out _) || !IsValidArgument(parameters[1]))
        {
            return false;
        }

        return true;
    }
}
