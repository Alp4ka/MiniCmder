using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MiniCmder
{
    public static class Editor
    {
        public static void ShowContent(string path, bool meta = true)
        {
            if (meta)
            {
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                FileInfo fileInfo = new FileInfo(path);
                string metaInf = $"{fileInfo.Name} \t {fileInfo.Extension} \t {fileInfo.CreationTimeUtc} \t {fileInfo.Length} bytes";
                Console.WriteLine(metaInf);
            }
            Console.ForegroundColor = ConsoleColor.White;
            string[] lines = File.ReadAllLines(path, Encoding.UTF8);
            Console.WriteLine(String.Join("\n", lines));
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(new String(new char[Console.WindowWidth]).Replace("\0", "_"));
            Dialog.WhileNot("Нажмите [y] чтобы выйти из режима просмотра.", 'y');
            
        }
        public static void EditContent()
        {

        }
    }
}
