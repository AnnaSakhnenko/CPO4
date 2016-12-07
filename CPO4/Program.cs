using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CPO4
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
            first:
                string type = "";
                Console.Write(">  ");
                string request = Console.ReadLine();
                request += ' ';
                if (request.IndexOf("-type f") != -1)
                {
                    type = "f";
                    request = request.Remove(request.IndexOf("-type f"), 8);
                }
                if (request.IndexOf("-type d") != -1)
                {
                    type = "d";
                    request = request.Remove(request.IndexOf("-type d"), 8);
                }
                request = Regex.Replace(request, "[ ]+", " ");
                if (request.IndexOf("find") != -1)
                {
                    string condition = "";
                    string folder = "";
                    string directory = request.Remove(0, 5);
                    string team = directory.Remove(0, directory.IndexOf(' ')).TrimStart();
                    directory = directory.Remove(directory.IndexOf(' '));
                    if (team.IndexOf(' ') != -1)
                    {                        
                        if (team.IndexOf('-') != 0)
                        {
                            string team1 = team.Remove(0, team.IndexOf(' ')).TrimStart();
                            team = team.Remove(team.IndexOf(' '));
                            folder = "\\" + team;
                            if (team1.IndexOf(' ') != -1)
                            {
                                condition = team1.Remove(0, team1.IndexOf(' ')).TrimStart(); 
                                team = team1.Remove(team1.IndexOf(' '));
                            }
                            else
                            {
                                team = "";
                            }
                        }
                        else
                        {
                            if (team.IndexOf(' ') != -1)
                            {
                                condition = team.Remove(0, team.IndexOf(' ')).TrimStart(); ;
                                team = team.Remove(team.IndexOf(' '));
                            }
                        }
                    }
                    switch (directory[0])
                    {
                        case '/':
                            if(folder != "")
                            {
                                folder = folder.Remove(0, 1);
                            }
                            if (team == "" || condition == "")
                            {
                                ShowWindowsDirectoryInfo(directory + folder, type);
                            }
                            else
                            {
                                switch (team)
                                {
                                    case "-name":
                                        ShowWindowsDirectoryInfo(directory + folder, condition, type);
                                        break;
                                    case "-size":
                                        condition = condition.Remove(condition.IndexOf(' '));
                                        ShowWindowsFileInfo(directory + folder, condition, type);
                                        break;
                                }
                            }
                            break;
                        case '.':
                            if (team == "" || condition == "")
                            {
                                ShowWindowsDirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + folder, type);
                            }
                            else
                            {
                                switch (team)
                                {
                                    case "-name":
                                        ShowWindowsDirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + folder, condition, type);
                                        break;
                                    case "-size":
                                        condition = condition.Remove(condition.IndexOf(' '));
                                        ShowWindowsFileInfo(AppDomain.CurrentDomain.BaseDirectory + folder, condition, type);
                                        break;
                                }
                            }
                            break;
                        case '~':
                            if (team == "" || condition == "")
                            {
                                ShowWindowsDirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + folder, type);
                            }
                            else
                            {
                                switch (team)
                                {
                                    case "-name":
                                        ShowWindowsDirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + folder, condition, type);
                                        break;
                                    case "-size":
                                        condition = condition.Remove(condition.IndexOf(' '));
                                        ShowWindowsFileInfo(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + folder, condition, type);
                                        break;
                                }
                            }
                            break;
                    }                    
                }
                else
                {
                    Console.WriteLine("Ошибка");
                }
                goto first;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.Read();
            }
        }

        static void ShowWindowsDirectoryInfo(string directory, string type)
        {
            string[] directories = Directory.GetDirectories(directory);
            for (int i = 0; i < directories.Length; i++)
            {
                try
                {
                    if(type == "d" || type == "")
                    {
                        Console.WriteLine(directories[i]);
                    }
                    ShowWindowsDirectoryInfo(directories[i], type);
                }
                catch
                {
                    i++;
                }
            }
            if (type == "f" || type == "")
            {
                string[] files = Directory.GetFiles(directory);
                for (int i = 0; i < files.Length; i++)
                {
                    Console.WriteLine(files[i]);
                }
            }
        }

        static void ShowWindowsDirectoryInfo(string directory, string condition, string type)
        {
            string[] directories = Directory.GetDirectories(directory);
            for (int i = 0; i < directories.Length; i++)
            {
                try
                {
                    if (type == "d" || type == "")
                    {
                        string[] director = Directory.GetDirectories(directory, condition);
                        for (int j = 0; j < director.Length; j++)
                        {
                        Console.WriteLine(director[j]);
                        }
                    }
                    ShowWindowsDirectoryInfo(directories[i], condition, type);
                }
                catch
                {
                    i++;
                }
            }
            if (type == "f" || type == "")
            {
                string[] files = Directory.GetFiles(directory, condition);
                for (int i = 0; i < files.Length; i++)
                {
                    Console.WriteLine(files[i]);
                }
            }
        }

        static void ShowWindowsFileInfo(string directory, string condition, string type)
        {
            bool signBool = condition[0].ToString().All(char.IsDigit);
            bool sizeBool = condition[condition.Length - 1].ToString().All(char.IsDigit);

            double length = 0;
            //Console.WriteLine(size);
            if (signBool)
            {
                string size = "";
                if (sizeBool)
                {
                    size = "b"; 
                }
                else
                {
                    size = condition[condition.Length - 1].ToString().ToLower();
                    condition = condition.Remove(condition.Length - 1);
                }                
                string[] files = Directory.GetFiles(directory);
                for (int i = 0; i < files.Length; i++)
                {
                    FileInfo file = new FileInfo(files[i]);                    
                    switch (size)
                    {
                        case "b":
                            if (file.Length == Convert.ToInt32(condition))
                            {
                                Console.WriteLine(file);
                            }
                            break;
                        case "k":
                            if (file.Length / 1024 == Convert.ToInt32(condition))
                            {
                                Console.WriteLine(file);
                            }
                            break;
                        case "m":
                            length = file.Length / 1024;
                            length = length / 1024;
                            length = Math.Round(length, 2);
                            if (length == Convert.ToDouble(condition))
                            {
                                Console.WriteLine(file);
                            }
                            break;
                    }
                    //Console.WriteLine(length.ToString());
                }
            }
            else
            {
                string sign = condition[0].ToString();
                condition = condition.Remove(0, 1);
                string size = "";
                if (sizeBool)
                {
                    size = "b";
                }
                else
                {
                    size = condition[condition.Length - 1].ToString().ToLower();
                    condition = condition.Remove(condition.Length - 1);
                }
                string[] files = Directory.GetFiles(directory);
                for (int i = 0; i < files.Length; i++)
                {
                    FileInfo file = new FileInfo(files[i]);
                    switch (sign)
                    {
                        case "-":
                            switch (size)
                            {
                                case "b":
                                    if (file.Length < Convert.ToInt32(condition))
                                    {
                                        Console.WriteLine(file);
                                    }
                                    break;
                                case "k":
                                    if (file.Length / 1024 < Convert.ToInt32(condition))
                                    {
                                        Console.WriteLine(file);
                                    }
                                    break;
                                case "m":
                                    length = file.Length / 1024;
                                    length = length / 1024;
                                    length = Math.Round(length, 2);
                                    if (length < Convert.ToDouble(condition))
                                    {
                                        Console.WriteLine(file);
                                    }
                                    break;
                            }
                            break;
                        case "+":
                            switch (size)
                            {
                                case "b":
                                    if (file.Length > Convert.ToInt32(condition))
                                    {
                                        Console.WriteLine(file);
                                    }
                                    break;
                                case "k":
                                    if (file.Length / 1024 > Convert.ToInt32(condition))
                                    {
                                        Console.WriteLine(file);
                                    }
                                    break;
                                case "m":
                                    length = file.Length / 1024;
                                    length = length / 1024;
                                    length = Math.Round(length, 2);
                                    if (length > Convert.ToDouble(condition))
                                    {
                                        Console.WriteLine(file);
                                    }
                                    break;
                            }
                            break;
                    }
                }
            }

            string[] directories = Directory.GetDirectories(directory);
            for (int i = 0; i < directories.Length; i++)
            {
                try
                {
                    ShowWindowsFileInfo(directories[i], condition, type);
                }
                catch
                {
                    i++;
                }
            }
        }
    }
}
