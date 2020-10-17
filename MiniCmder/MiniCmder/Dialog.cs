using System;

namespace MiniCmder
{
    /// <summary>
    /// Класс для диалогов с пользователем.
    /// </summary>
    public static class Dialog
    {
        /// <summary>
        /// Диалог типа Да-Нет
        /// </summary>
        /// <param name="question"> Вопрос в диалоге. </param>
        /// <param name="keyYes"> Клавиша для 'Да'. </param>
        /// <param name="keyNo"> Клавиша для 'Нет'. </param>
        /// <returns></returns>
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
        }
        /// <summary>
        /// Диалог, в котором, пока не нажмешь на нужную кнопку, от тебя не отстанут.
        /// </summary>
        /// <param name="question"> Вопрос в диалоге. </param>
        /// <param name="keyToPress"> Клавиша, которую нужно нажать. </param>
        /// <returns></returns>
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
