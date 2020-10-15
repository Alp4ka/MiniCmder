using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace MiniCmder
{
    public class Manager
    {
        private string currentDrive;
        private string userProfilePath, currentPath;
        public Dictionary <string, Delegate> Functions;
        public Manager()
        {
            Functions = new Dictionary<string, Delegate>();
            string[] rootPathSplitted = this.StringToPath(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));
            CurrentDrive = rootPathSplitted[0];
            CurrentPath = String.Join("\\", PathToString(rootPathSplitted));
            this.userProfilePath = CurrentPath;
            this.Functions.Add("./", new Action(ToUp));
            this.Functions.Add("dir", new Func<string[], int>(str => GetContaining(str)));
        }
        public string PathToString(string[] pathSplitted)
        {
            try
            {
                return String.Join("\\", pathSplitted);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "";
            }
        }
        private void ToUp()
        {
            if (CurrentPath != CurrentDrive+"\\")
            {
                /*string pathWoLast = PathToString(StringToPath(CurrentPath).Where((i,j) => 
                j!= StringToPath(CurrentPath).Length-1).ToArray());
                CurrentPath = pathWoLast;*/
                CurrentPath = Directory.GetParent(CurrentPath).FullName;
            }
            else
            {
                Console.WriteLine("Нельзя подняться выше.");
            }
        }
        private int GetContaining(string[] parameters)
        {
            try
            {
                string path, mode;
                path = parameters[0];
                if (!CheckIfExists(path))
                {
                    throw new Exception("put' pizdec");
                }
                mode = parameters[1];
                if (parameters.Length != 2 && parameters.Length != 1)
                {
                    throw new Exception("dline pizdec");
                }
                string[] files = Directory.GetFiles(path).Select(i => i.Replace(path, "")).ToArray();
                string[] directories = Directory.GetDirectories(path).Select(i => i.Replace(path, "")).ToArray();

                switch (mode)
                {
                    case "-a":
                        Console.WriteLine(String.Join("\n", directories.Concat(files).ToArray()));
                        return 0;
                    case "-f":
                        Console.WriteLine(String.Join("\n", files));
                        return 0;
                    case "-d":
                        Console.WriteLine(String.Join("\n", directories));
                        return 0;
                    case "--help":
                        //Dopisat
                        Console.WriteLine("help dir");
                        return 0;
                    default:
                        Console.WriteLine("no such an option");
                        return 1;
                }
            }
            catch
            {
                return 1;
            }
            
        }
        private bool CheckIfExists(string path)
        {
            FileAttributes attr = File.GetAttributes(path);
            if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                return Directory.Exists(path);
            else
                return File.Exists(path);
        }
        public string[] StringToPath(string path)
        {
            try
            {
                return path.Split("\\");
            }
            catch
            {

                return (new string[] { });
            }
        }
        public string[] GetComputerDrives()
        {
            return Environment.GetLogicalDrives().Select(i => i.Replace("\\", "")).ToArray();
        }
        
        public string CurrentDrive
        {
            get
            {
                return this.currentDrive;
            }
            set
            {
                if (GetComputerDrives().Contains(value))
                {
                    this.currentDrive = value;
                }
                else
                {
                    throw new Exception("Нет такого диска");
                }
            }
        }
        public string CurrentPath
        {
            get
            {
                return this.currentPath;
            }
            set
            {
                if (CheckIfExists(value))
                {
                    if(StringToPath(value)[0] != CurrentDrive)
                    {
                        throw new Exception("Путь не найден");
                    }
                    else
                    {
                        this.currentPath = value;
                    }
                    
                }
                else
                {
                    throw new Exception("Путь не найден");
                }
            }
        }
        public string UserProfilePath
        {
            get
            {
                return this.userProfilePath;
            }
        }
        public string CurrentFullPath{
            get
            {
                return CurrentDrive + "\\" + CurrentPath;
            }
        }
    }
}
