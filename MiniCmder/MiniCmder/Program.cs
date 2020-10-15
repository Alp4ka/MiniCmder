using System;

namespace MiniCmder
{
    class Program
    {
        static void Main(string[] args)
        {
            Manager manager = new Manager();
            Console.WriteLine(manager.CurrentDrive);
            Console.WriteLine(manager.CurrentPath);
            manager.Functions["./"].DynamicInvoke();
            Console.WriteLine(manager.CurrentPath);
        }
    }
}
