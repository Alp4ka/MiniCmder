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
        //public Dictionary <string, Delegate> Functions;
        public Manager()
        {
            //Functions = new Dictionary<string, Delegate>();
            string[] rootPathSplitted = this.StringToPath(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));
            CurrentDrive = rootPathSplitted[0];
            CurrentPath = String.Join("\\", PathToString(rootPathSplitted));
            this.userProfilePath = CurrentPath;
            //this.Functions.Add("../", new Action(ToUp));
            //this.Functions.Add("dir", new Func<string[], int>(parameters => GetContaining(parameters)));
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
        public void ToUp()
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
        public void SetCommand(string input) {
            if(input.Length == 0)
            {
                throw new Exception("Че с длиной");
            }
            string[] inputSplitted = input.Split();
            string command = inputSplitted[0];
            string[] parameters = inputSplitted.Where((i, j) => j!=0).ToArray();
            switch (command)
            {
                case "../":
                    ToUp();
                    break;
                case "dir":
                    GetContaining(parameters);
                    break;
                case "..\\":
                    ToUp();
                    break;
                case "cls":
                    ClearScreen();
                    break;
                case "cdrive":
                    ChangeDrive(parameters);
                    break;
                case "--help":
                    Help();
                    break;
                default:
                    Console.WriteLine("Не существует такой команды. \n--help для справки");
                    break;
            }
        }
        public int GetContaining(string[] parameters)
        {
            try
            {
                string path, mode;
                switch (parameters.Length)
                {
                    case 0:
                        path = CurrentPath;
                        mode = "-a";
                        break;
                    case 1:
                        path = CurrentPath;
                        mode = parameters[0];
                        break;
                    default:
                        throw new Exception("Нет таких аргументов. \ndir --help для справки.");

                }
                if (!CheckIfExists(path))
                {
                    throw new Exception("Указанный путь не существует.");
                }
                string[] files = Directory.GetFiles(path).Select(i => i.Replace(path, "")).ToArray();
                string[] directories = Directory.GetDirectories(path).Select(i => i.Replace(path, "")).ToArray();
                /*Console.WriteLine(path);
                path = path.Replace("/", "\\");
                if (path.StartsWith("..\\") || path.StartsWith(".\\"))
                {
                    path = CurrentPath + "\\" + path;
                }
                Console.WriteLine(path);
                Console.WriteLine(path, mode);*/
                for (int fileIndex = 0; fileIndex < files.Length; ++fileIndex)
                {
                    FileInfo fileInfo = new FileInfo(path + "\\" + files[fileIndex]);
                    files[fileIndex] = $"{fileInfo.CreationTimeUtc} \t {fileInfo.Extension} \t {fileInfo.Name}";
                }
                for (int dirIndex = 0; dirIndex < directories.Length; ++dirIndex)
                {
                    DirectoryInfo fileInfo = new DirectoryInfo(path + "\\" + directories[dirIndex]);
                    directories[dirIndex] = $"{fileInfo.CreationTimeUtc} \t <DIR> \t {fileInfo.Name}";
                }
                switch (mode)
                {
                    case "-a":
                        Console.WriteLine(String.Join("\n", directories.Concat(files).ToArray()));
                        break;
                    case "-f":
                        Console.WriteLine(String.Join("\n", files));
                        break;
                    case "-d":
                        Console.WriteLine(String.Join("\n", directories));
                        break;
                    case "--help":
                        Console.WriteLine("\t'dir': \n\t'dir' - Показывает ВСЕ файлы и подкаталоги в текущем каталоге. \n\t'dir -a' - Показывает ВСЕ файлы и подкаталоги в текущем каталоге. \n\t'dir -f' - Показывает ВСЕ файлы в текущем каталоге. \n\t'dir -d' - Показывает ВСЕ подкаталоги в текущем каталоге.");
                        break;
                    default:
                        throw new Exception("Нет таких аргументов. \ndir --help для справки.");
                        Console.ForegroundColor = ConsoleColor.White;
                        return 1;
                }
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 1;
            }
            
        }
        public void ClearScreen()
        {
            Console.Clear();
        }
        public void Help()
        {
            Console.WriteLine("--help\tСправка по командам.");
            Console.WriteLine("dir\tПоказать файлы в текущем каталоге. Смотреть dir --help для справки.");
            Console.WriteLine("cls\tОчистить экран.");
            Console.WriteLine("cdrive\tСмотерть cdrive --help для просмтра справки");
            Console.WriteLine("../ или ..\\\tПерейти на каталог выше.");
        }
        public void ChangeDirectory() {
        }
        public void ChangeDrive(string[] parameters)
        {
            try
            {
                string parameter;
                if (parameters.Length == 1)
                {
                    parameter = parameters[0];
                }
                else
                {
                    throw new Exception("Нет таких аргументов. \ncdrive --help для справки.");
                }
                switch (parameter)
                {
                    case "--help":
                        Console.WriteLine("\t'cdrive': \n\t'cdrive <ИМЯ_ДИСКА>:' - Изменяет текущий диск и путь на <ИМЯ_ДИСКА>:.");
                        break;
                    case "-list":
                        Console.WriteLine(String.Join("\n", GetComputerDrives()));
                        break;
                    default:
                        if (GetComputerDrives().Contains(parameter))
                        {
                            CurrentPath = CurrentDrive = parameter;
                        }
                        else
                        {
                            throw new Exception("Нет таких аргументов. \ncdrive --help для справки.");
                        }
                        break;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
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
        /*public string CurrentFullPath{
            get
            {
                return CurrentDrive + "\\" + CurrentPath;
            }
        }*/
    }
}
