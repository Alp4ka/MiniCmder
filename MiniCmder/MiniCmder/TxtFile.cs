using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Text;
using System.IO;
using System.Xml;

namespace MiniCmder
{
    /// <summary>
    /// По сути - просто класс обменник для Copy, Paste.
    /// </summary>
    public static class TxtFile
    {
        public static string Name = null;
        public static string Content = null;
        public static string GenerateName(string path)
        {
            string name = Name;
            Console.WriteLine(name);
            while (true)
            {
                if(File.Exists(Path.Combine(path, name)))
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
