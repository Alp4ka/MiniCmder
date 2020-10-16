using System;
using System.IO;
using System.Linq;

namespace MiniCmder
{
    public class Manager
    {
        private string currentDrive;
        private string userProfilePath, currentPath;
        public Manager()
        {
            string[] rootPathSplitted = this.StringToPath(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));
            CurrentDrive = rootPathSplitted[0];
            CurrentPath = String.Join("\\", PathToString(rootPathSplitted));
            this.userProfilePath = CurrentPath;
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
            try
            {
                if (input.Length == 0)
                {
                    throw new Exception("Че с длиной");
                }
                string[] inputSplitted = input.Split();
                string command = inputSplitted[0];
                string[] parameters = inputSplitted.Where((i, j) => j != 0).ToArray();
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
                    case "cd":
                        ChangeDirectory(parameters);
                        break;
                    case "mkdir":
                        CreateDirectory(parameters);
                        break;
                    case "rfile":
                        DeleteFile(parameters);
                        break;
                    default:
                        throw new Exception("Не существует такой команды. \n--help для справки");
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
            }
        }
        public void CreateDirectory(string[] parameters) 
        {
            if (parameters.Length == 1)
            {
                try
                {
                    switch (parameters[0])
                    {
                        case "--help":
                            Console.WriteLine("\t'mkdir: \n\t'mkdir <ИМЯ_НОВОГО_КАТАЛОГА>' - Создать каталог <ИМЯ_НОВОГО_КАТАЛОГА>.");
                            break;
                        default:
                            string path = Path.GetFullPath(Path.Combine(CurrentPath, parameters[0]));
                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }
                            else
                            {
                                throw new Exception("Директория с таким именем уже существует.");
                            }
                            break;
                    }
                    
                    
                }
                catch(Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ex.Message);
                }
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
                }
                return 0;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
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
            Console.WriteLine("\t'--help'\n\tСправка по командам.\n");
            Console.WriteLine("\t'dir'\n\tПоказать файлы в текущем каталоге. Смотреть dir --help для справки.\n");
            Console.WriteLine("\t'cls'\n\tОчистить экран.\n");
            Console.WriteLine("\t'cdrive'\n\tСмена диска.Смотреть cdrive --help для просмтра справки.\n");
            Console.WriteLine("\t'../' или '..\\'\n\tПерейти на каталог выше.\n");
            Console.WriteLine("\t'cd <ПУТЬ>'\n\tПерейти в <ПУТЬ>. Смотреть cd --help для просмтра справки.\n");
            Console.WriteLine("\t'mdkir <ИМЯ_НОВОГО_КАТАЛОГА>'\n\tСоздать каталог <ИМЯ_НОВОГО_КАТАЛОГА>. Смотреть mkdir --help для просмтра справки.\n");
            Console.WriteLine("\t'rfile <ПУТЬ_К_ФАЙЛУ>'\n\tУдалить файл в <ПУТЬ_К_ФАЙЛУ>. Смотреть rfile --help для просмтра справки.\n");
        }
        public void ChangeDirectory(string[] parameters)
        {
            try
            {
                if (parameters.Length == 1)
                {
                    parameters[0] = parameters[0].Replace("/", "\\");
                }
                switch (parameters[0])
                {
                    case "--help":
                        Console.WriteLine("\t'cd: \n\t'cd <ПУТЬ>' - Перейти в <ПУТЬ>.");
                        break;
                    default:
                        string pathCheck = Path.GetFullPath(Path.Combine(CurrentPath, parameters[0]));
                        if (Directory.Exists(pathCheck))
                        {
                            CurrentPath = Path.GetFullPath(Path.Combine(CurrentPath, parameters[0]));
                        }
                        else
                        {
                            throw new Exception($"{pathCheck} не является директорией.");
                        }
                        break;
                }
            }
            catch (FileNotFoundException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Не найден файл." + "\ncd --help для справки.");
            }
            catch(Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message + "\ncd --help для справки.");
            }
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
                        Console.WriteLine("\t'cdrive': \n\t'cdrive <ИМЯ_ДИСКА>' - Изменяет текущий диск и путь на <ИМЯ_ДИСКА>:.\n\t'cdrive -list' - Выводит список всех дисков на компьютере.");
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
                Console.ForegroundColor = ConsoleColor.Red;
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
        //nuzhna zashita ot duraka
        public void DeleteFile(string[] parameters)
        {
            try
            {
                if(parameters.Length == 1)
                {
                    switch (parameters[0])
                    {
                        case "--help":
                            Console.WriteLine("\t'rfile: \n\t'rfile <ПУТЬ_К_ФАЙЛУ>' - Удалить файл в <ПУТЬ_К_ФАЙЛУ>.");
                            break;
                        default:
                            string path = Path.GetFullPath(Path.Combine(CurrentPath, parameters[0]));
                            if (File.Exists(path))
                            {
                                if (Dialog.YesNo("Вы точно хотите удалить этот файл?"))
                                {
                                    System.IO.File.Delete(path);
                                    if (!File.Exists(path))
                                    {
                                        Console.WriteLine($"Файл {path} успешно удален!");
                                    }
                                    else
                                    {
                                        throw new Exception("Не удалось удалить файл.");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine($"Вы отменили удаление файла {path}");
                                }

                            }
                            else
                            {
                                throw new FileNotFoundException();
                            }
                            break;
                    }
                    
                }
                else
                {
                    throw new Exception("Неверное количество аргументов.");
                }
            }
            catch (FileNotFoundException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Файл не найден.");
            }
            catch (UnauthorizedAccessException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Нет доступа к удалению файла.");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
            }
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
    }
}
