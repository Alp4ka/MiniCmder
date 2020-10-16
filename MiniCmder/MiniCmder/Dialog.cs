using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;

namespace MiniCmder
{
    public static class Dialog
    {
        public static bool YesNo(string question, char keyYes='y', char keyNo='n')
        {
            Console.WriteLine($"{question} \nНажмите [{keyYes}], чтобы да - [{keyNo}], чтобы нет:D");
            char choice = '\0';
            while(true){
                choice = Console.ReadKey().KeyChar.ToString().ToLower()[0];
                Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                Console.Write("   ");
                Console.SetCursorPosition(0, Console.CursorTop);
                if (choice == keyYes)
                {
                    return true;
                }
                else if(choice == keyNo)
                {
                    return false;
                }
            }
            return true;
        }
        public static bool WhileNot(string question, char keyToPress='y')
        {
            Console.WriteLine(question);
            char choice = '\0';
            while (true)
            {
                choice = Console.ReadKey().KeyChar.ToString().ToLower()[0];
                Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                Console.Write("   ");
                Console.SetCursorPosition(0, Console.CursorTop);
                if (choice == keyToPress)
                {
                    return true;
                }
            }
            return true;
        }
    }
    
}
