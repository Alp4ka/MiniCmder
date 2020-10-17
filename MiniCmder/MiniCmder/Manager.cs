using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
//if find regex "/a"
namespace MiniCmder
{
    public class Manager
    {
        private string currentDrive;
        private string userProfilePath, currentPath;
        public Concatenator Concat;
        public Manager()
        {
            Concat = new Concatenator();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Macrohard Шиндовс [Версия 228.1337]");
            Console.WriteLine("(c) Корпорация шизов(ШУЕ ППШ), 2020. Все права под надежной охраной санитаров.\n");
            Console.ForegroundColor = ConsoleColor.White;
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
            if (CurrentPath != CurrentDrive + "\\")
            {
                /*string pathWoLast = PathToString(StringToPath(CurrentPath).Where((i,j) => 
                j!= StringToPath(CurrentPath).Length-1).ToArray());
                CurrentPath = pathWoLast;*/
                CurrentPath = Directory.GetParent(Path.GetFullPath(Path.Combine(CurrentPath, "..\\"))).FullName;
            }
            else
            {
                Console.WriteLine("Нельзя подняться выше.");
            }
        }
        public void SetCommand(string input)
        {
            try
            {
                if (input.Length == 0)
                {
                    throw new Exception("");
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
                    case "help":
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
                    case "newfile":
                        CreateFile(parameters);
                        break;
                    case "sc":
                        ShowFile(parameters);
                        break;
                    case "copy":
                        Copy(parameters);
                        break;
                    case "paste":
                        Paste(parameters);
                        break;
                    case "move":
                        Move(parameters);
                        break;
                    case "concat":
                        Concatenate(parameters);
                        break;
                    case "graph":
                        break;
                    default:
                        throw new Exception("Не существует такой команды. \nhelp для справки");
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
            }
        }
        public void Move(string[] parameters)
        {
            try
            {
                if (parameters.Length == 1)
                {
                    switch (parameters[0])
                    {
                        case "--help":
                            Console.WriteLine("\t'move': \n\t'move <ПУТЬ_К_ФАЙЛУ>.txt <ПУТЬ_КУДА_КОПИРОВАТЬ>' - перемещает файл <ПУТЬ_К_ФАЙЛУ>.txt в <ПУТЬ_КУДА_КОПИРОВАТЬ>.");
                            break;
                        default:
                            break;
                    }
                    return;
                }
                else if(parameters.Length == 2)
                {
                    string pathToFile = Path.GetFullPath(Path.Combine(CurrentPath, parameters[0]));
                    string pathTo = Path.GetFullPath(Path.Combine(CurrentPath, parameters[1]));
                    if (!File.Exists(pathToFile))
                    {
                        throw new Exception($"Файл {pathToFile} не существует.");
                    }
                    if (!Directory.Exists(pathTo))
                    {
                        throw new Exception($"{pathTo} не существует 0_0.");
                    }
                    Console.WriteLine(pathToFile);
                    Console.WriteLine(Path.GetFullPath(Path.Combine(pathTo, StringToPath(pathToFile)[StringToPath(pathToFile).Length - 1])));
                    File.Move(pathToFile, Path.GetFullPath(Path.Combine(pathTo, StringToPath(pathToFile)[StringToPath(pathToFile).Length - 1])));
                    Console.WriteLine();
                }
                else
                {
                    throw new Exception("Неверное количество аргументов.");
                }
            }
            catch(Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message + "\nmove --help для справки.");
            }
            string path = Path.GetFullPath(Path.Combine(CurrentPath, parameters[1]));
        }
        public void Concatenate(string[] parameters)
        {
            try
            {
                if(parameters.Length == 1)
                {
                    switch (parameters[0])
                    {
                        case "--help":
                            Console.WriteLine("\t'concat': \n\t'concat add <ПУТЬ>.txt' - Складывает текстовую информацию из файла <ПУТЬ>.txt к предыдущим значениям concat \n\t'concat show' - Выводит текущее значение concat. \n\t'concat clear' - Очищает concat.");
                            break;
                        case "clear":
                            Concat.SetNull();
                            break;
                        case "show":
                            Console.WriteLine(Concat.ToString());
                            break;
                        default:
                            throw new Exception("Неизвестный аргумент для concat.");
                    }
                }
                else if(parameters.Length == 2)
                {
                    switch (parameters[0])
                    {
                        case "add":
                            string path = Path.GetFullPath(Path.Combine(CurrentPath, parameters[1]));
                            if (File.Exists(path))
                            {
                                if (path[(path.Length - 4)..].ToString().ToLower() != ".txt")
                                {
                                    throw new Exception("Укажите расширение .txt после имени файла.");
                                }
                                Concat.Concat(String.Join("\n", File.ReadAllLines(path))+"\n");
                            }
                            else
                            {
                                throw new Exception($"Файл {path} не существует.");
                            }
                            break;
                        default:
                            throw new Exception("Неизвестный аргумент для concat.");
                    }
                }
                else
                {
                    throw new Exception("Неверное количество аргументов.");
                }
            }
            catch(Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message + "\nconcat --help для справки. ");
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
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ex.Message);
                }
            }
        }
        public void Copy(string[] parameters)
        {
            try
            {
                if(parameters.Length == 1)
                {
                    switch (parameters[0])
                    {
                        case "--help":
                            Console.WriteLine("\t'copy': \n\t'copy <ПУТЬ>.txt' - Копирует файл <ПУТЬ>.txt в буфер.");
                            break;
                        default:
                            string path = Path.GetFullPath(Path.Combine(CurrentPath, parameters[0]));
                            if (!File.Exists(path))
                            {
                                throw new Exception($"Файла {path} не существует.");
                            }
                            TxtFile.Name = StringToPath(path)[StringToPath(path).Length - 1];
                            TxtFile.Content = String.Join("\n", File.ReadAllLines(path));
                            Console.WriteLine(TxtFile.Name);
                            Console.WriteLine(TxtFile.Content);
                            break;
                    }
                }
            }
            catch(Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message + "\ncopy --help для справки. ");
            }
        }
        public void Paste(string[] parameters)
        {
            try
            {
                if(parameters.Length == 0)
                {
                    string path = Path.GetFullPath(CurrentPath);
                    if (File.Exists(path))
                    {
                        throw new Exception($"Это не директория.");
                    }
                    if (Directory.Exists(path))
                    {
                        if (TxtFile.Name == null || TxtFile.Content == null)
                        {
                            throw new Exception("Не скопировали ничего");
                        }
                        TxtFile.Name = TxtFile.GenerateName(path);
                        string fullPath = Path.Combine(path, TxtFile.Name);
                        File.Create(fullPath).Close();
                        File.WriteAllText(fullPath, TxtFile.Content);
                        if (File.Exists(fullPath))
                        {
                            Console.WriteLine($"Файл {fullPath} создан.");
                        }
                        else
                        {
                            throw new Exception($"Файл {fullPath} не удалось создать.");
                        }
                    }
                    else
                    {
                        throw new Exception($"Директории нет");
                    }
                }
                else if (parameters.Length == 1)
                {
                    switch (parameters[0])
                    {
                        case "--help":
                            Console.WriteLine("\t'paste': \n\t'paste <ПУТЬ>' - Вставляет файл из буфера в <ПУТЬ>.");
                            break;
                        default:
                            string path = Path.GetFullPath(Path.Combine(CurrentPath, parameters[0]));
                            if (File.Exists(path))
                            {
                                throw new Exception($"Это не директория.");
                            }
                            if (Directory.Exists(path))
                            {
                                if (TxtFile.Name == null || TxtFile.Content == null)
                                {
                                    throw new Exception("Не скопировали ничего");
                                }
                                TxtFile.Name = TxtFile.GenerateName(path);
                                string fullPath = Path.Combine(path, TxtFile.Name);
                                File.Create(fullPath).Close();
                                File.WriteAllText(fullPath, TxtFile.Content);
                                if (File.Exists(fullPath))
                                {
                                    Console.WriteLine($"Файл {path} создан.");
                                }
                                else
                                {
                                    throw new Exception($"Файл {path} не удалось создать.");
                                }
                                break;
                            }
                            else
                            {
                                throw new Exception($"Директории нет");
                            }

                    }
                }
                else
                {
                    throw new Exception("Неверное количество аргументов.");
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message + "\npaste --help для справки. ");
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
            Console.WriteLine("\t'sc <ПУТЬ>'\n\tsc <ПУТЬ> - Открыть файл в <ПУТЬ>. Смотреть sc --help для просмтра справки.\n");
            Console.WriteLine("\t'newfile <ПУТЬ>.txt'\n\tnewfile <ПУТЬ>.txt - Создать .txt файл в <ПУТЬ>. Смотреть newfile --help для просмтра справки.\n");
            Console.WriteLine("\t'concat'\n\tСмотреть concat --help для просмтра справки.\n");
            Console.WriteLine("\t'copy'\n\tcopy <ПУТЬ>.txt - копировать\n\tСмотреть copy --help для просмтра справки.\n");
            Console.WriteLine("\t'paste'\n\tpaste - вставить\n\tСмотреть paste --help для просмтра справки.\n");
            Console.WriteLine("\t'move'\n\tmove <ПУТЬ_К_ФАЙЛУ>.txt <ПУТЬ_КУДА_КОПИРОВАТЬ>' - перемещает файл <ПУТЬ_К_ФАЙЛУ>.txt в <ПУТЬ_КУДА_КОПИРОВАТЬ>.");
            Console.WriteLine("\tНет возможности читать файлы и папки с пробельными символами.");
        }
        public void ChangeDirectory(string[] parameters)
        {
            try
            {
                if (parameters.Length == 1)
                {
                    parameters[0] = parameters[0].Replace("/", "\\");
                }
                else
                {
                    throw new Exception("Неверное число аргументов.");
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
            catch (Exception ex)
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
            catch (Exception ex)
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
        public void ShowFile(string[] parameters)
        {
            try
            {
                if (parameters.Length == 1)
                {
                    switch (parameters[0])
                    {
                        case "--help":
                            Console.WriteLine("\t'sc:' \n\t'sc <ПУТЬ>' - Открыть файл в <ПУТЬ>. \n\t'sc <ПУТЬ>' utf-8 - Открыть с кодировкой utf-8.\n\t'sc <ПУТЬ>' unicode - Открыть с кодировкой unicode.\n\t'sc <ПУТЬ>' ascii - Открыть с кодировкой ASCII.");
                            break;
                        default:
                            string path = Path.GetFullPath(Path.Combine(CurrentPath, parameters[0]));
                            if (File.Exists(path))
                            {
                                Editor.ShowContent(path, Encoding.UTF8, true);
                            }
                            else
                            {
                                throw new Exception($"Файла {path} не существует.");
                            }
                            break;
                    }
                }
                else if (parameters.Length == 2)
                {
                    Encoding encoding;
                    switch (parameters[1])
                    {
                        case "utf-8":
                            encoding = Encoding.UTF8;
                            break;
                        case "unicode":
                            encoding = Encoding.Unicode;
                            break;
                        case "ascii":
                            encoding = Encoding.ASCII;
                            break;
                        default:
                            throw new Exception($"Кодировка {parameters[1]} недоступна.");
                    }
                    string path = Path.GetFullPath(Path.Combine(CurrentPath, parameters[0]));
                    if (File.Exists(path))
                    {
                        Editor.ShowContent(path, encoding, true);
                    }
                    else
                    {
                        throw new Exception($"Файла {path} не существует.");
                    }
                }
                else
                {
                    throw new Exception("Неверное число аргументов.\nsc --help для справки.");
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message + "\nsc --help для справки.");
            }
        }
        //nuzhna zashita ot duraka
        public void DeleteFile(string[] parameters)
        {
            try
            {
                if (parameters.Length == 1)
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
        public void CreateFile(string[] parameters)
        {
            try
            {
                if (parameters.Length == 1)
                {
                    switch (parameters[0])
                    {
                        case "--help":
                            Console.WriteLine("\t'newfile:' \n\t'newfile <ПУТЬ>.txt' - Создать файл .txt в <ПУТЬ>. \n\t'newfile <ПУТЬ>.txt' utf-8 - Создать файл .txt в <ПУТЬ> с кодировкой utf-8.\n\t'newfile <ПУТЬ>.txt' unicode - Создать файл .txt в <ПУТЬ> с кодировкой unicode.\n\t'newfile <ПУТЬ>.txt' ascii - Создать файл .txt в <ПУТЬ> с кодировкой ascii.");
                            break;
                        default:
                            string path = Path.GetFullPath(Path.Combine(CurrentPath, parameters[0]));
                            if (File.Exists(path))
                            {
                                throw new Exception($"Файл {path} уже существует.");
                            }
                            if(path[(path.Length-4)..].ToString().ToLower() != ".txt")
                            {
                                throw new Exception("Укажите расширение .txt после имени файла.");
                            }
                            File.Create(path).Close();
                            Editor.EditContent(path, Encoding.UTF8);
                            if (File.Exists(path))
                            {
                                Console.WriteLine($"Файл {path} создан.");
                            }
                            else
                            {
                                throw new Exception($"Файл {path} не удалось создать.");
                            }
                            break;
                    }
                }
                else if (parameters.Length == 2)
                {
                    Encoding encoding;
                    switch (parameters[1])
                    {
                        case "utf-8":
                            encoding = Encoding.UTF8;
                            break;
                        case "unicode":
                            encoding = Encoding.Unicode;
                            break;
                        case "ascii":
                            encoding = Encoding.ASCII;
                            break;
                        default:
                            encoding = Encoding.UTF8;
                            break;
                    }
                    string path = Path.GetFullPath(Path.Combine(CurrentPath, parameters[0]));
                    if (File.Exists(path))
                    {
                        throw new Exception($"Файл {path} уже существует.");
                    }
                    if (path[(path.Length - 4)..].ToString().ToLower() != ".txt")
                    {
                        throw new Exception("Укажите расширение .txt после имени файла.");
                    }
                    File.Create(path).Close();
                    Editor.EditContent(path, encoding);
                    if (File.Exists(path))
                    {
                        Console.WriteLine($"Файл {path} создан.");
                    }
                    else
                    {
                        throw new Exception($"Файл {path} не удалось создать.");
                    }
                }
                else
                {
                    throw new Exception("Неверное число аргументов.\nnewfile --help для справки.");
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message + "\nnewfile --help для справки.");
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
                    if (StringToPath(value)[0] != CurrentDrive)
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
