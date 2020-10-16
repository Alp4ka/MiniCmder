using System;
using System.Globalization;

namespace MiniCmder
{
    class Program
    {
        static void Main(string[] args)
        {
            Manager manager = new Manager();
            Console.ForegroundColor = ConsoleColor.White;
            while (true)
            {
                Console.Write($"{manager.CurrentPath}> ");
                string line = Console.ReadLine();
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                manager.SetCommand(line);
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }
}
