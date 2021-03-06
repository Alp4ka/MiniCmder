﻿using System;
using System.IO;
using System.Text;

namespace MiniCmder
{
    /// <summary>
    /// Класс редактора файлов.
    /// </summary>
    public static class Editor
    {
        /// <summary>
        /// Метод, осуществляющий просмотр текстового файла.
        /// </summary>
        /// <param name="path"> Путь к файлу. </param>
        /// <param name="encoding"> Кодировка. </param>
        /// <param name="meta"> true - Мета инфа о файле выводится, false - нет. </param>
        public static void ShowContent(string path, Encoding encoding, bool meta = true)
        {
            if (meta)
            {
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                FileInfo fileInfo = new FileInfo(path);
                string metaInf = $"{fileInfo.Name} \t {fileInfo.Extension} \t {fileInfo.CreationTimeUtc} \t {fileInfo.Length} bytes";
                Console.WriteLine(metaInf);
            }
            Console.ForegroundColor = ConsoleColor.White;
            string[] lines = File.ReadAllLines(path, encoding);
            Console.WriteLine(String.Join("\n", lines));
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(new String(new char[Console.WindowWidth]).Replace("\0", "_"));
            Dialog.WhileNot("Нажмите [y] чтобы выйти из режима просмотра.", 'y');

        }
        /// <summary>
        /// Метод, осуществляющий создание(запись) текстового файла.
        /// </summary>
        /// <param name="path"> Путь к файлу. </param>
        /// <param name="encoding"> Кодировка файла. </param>
        public static void EditContent(string path, Encoding encoding)
        {
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            FileInfo fileInfo = new FileInfo(path);
            ConsoleKeyInfo symbol;
            string metaInf = $"{fileInfo.Name} \t {fileInfo.Extension} \t {fileInfo.CreationTimeUtc} \t {fileInfo.Length} bytes";
            Console.WriteLine(metaInf);
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("[ESCAPE], чтобы завершить написание файла. ");
            string input = "";
            Console.ForegroundColor = ConsoleColor.White;
            while (true)
            {
                symbol = Console.ReadKey(true);
                if (symbol.Key == ConsoleKey.Escape)
                {
                    break;
                }
                else if (symbol.KeyChar == '\b')
                {
                    if (Console.CursorLeft > 0)
                    {
                        Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                        Console.Write(" ");
                        Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                        input = input[0..(input.Length - 1)];
                    }
                }
                else if (symbol.KeyChar == (char)13)
                {
                    Console.Write("\n");
                    input += "\n";
                }
                else
                {
                    Console.Write(symbol.KeyChar);
                    input += symbol.KeyChar;
                }
            }
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            if (Dialog.YesNo("Сохранить изменения в документе?"))
            {
                using (var writer = new StreamWriter(path, true, encoding))
                {
                    writer.Write(input);
                }
            }
            else
            {
                return;
            }


        }
    }
}
