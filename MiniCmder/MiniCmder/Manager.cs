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
            if (CurrentPath != CurrentDrive)
            {
                string pathWoLast = PathToString(StringToPath(CurrentPath).Where((i,j) => 
                j!= StringToPath(CurrentPath).Length-1).ToArray());
                CurrentPath = pathWoLast;
            }
            else
            {
                Console.WriteLine("Нельзя подняться выше.");
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
            catch (Exception ex)
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
