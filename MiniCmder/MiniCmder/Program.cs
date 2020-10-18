using System;

namespace MiniCmder
{
    class Program
    {
        /// <summary>
        /// Основной цикл.
        /// </summary>
        /// <param name="args"></param>
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
