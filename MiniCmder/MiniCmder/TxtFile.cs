using System;
using System.IO;

namespace MiniCmder
{
    /// <summary>
    /// По сути - просто класс обменник для Copy, Paste.
    /// </summary>
    public static class TxtFile
    {
        public static string Name = null;
        public static string Content = null;
        /// <summary>
        /// Генерирует имя для файла, чтобы избежать повторений имен.
        /// </summary>
        /// <param name="path"> Путь, где должен быть файл. </param>
        /// <returns> string - сгенерированное имя файла. </returns>
        public static string GenerateName(string path)
        {
            string name = Name;
            Console.WriteLine(name);
            while (true)
            {
                if (File.Exists(Path.Combine(path, name)))
                {
                    name = "копия_" + name;
                    Console.WriteLine(name);
                }
                else
                {
                    break;
                }
            }
            return name;
        }
    }
}
