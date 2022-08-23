using System;
using System.IO;
using static System.Console;
using System.Collections.Generic;
using static OS_Project.Virtual_Disk;
using System.Data;


namespace OS_Project
{
    class Program
    {
        static void CopyDirectory(string sourceDir, string destinationDir, bool recursive)
        {
            // Get information about the source directory
            var dir = new DirectoryInfo(sourceDir);

            // Check if the source directory exists
            if (!dir.Exists)
                throw new DirectoryNotFoundException($"Source directory not found: {dir.FullName}");

            // Cache directories before we start copying
            DirectoryInfo[] dirs = dir.GetDirectories();

            // Create the destination directory
            Directory.CreateDirectory(destinationDir);

            // Get the files in the source directory and copy to the destination directory
            foreach (FileInfo file in dir.GetFiles())
            {
                string targetFilePath = Path.Combine(destinationDir, file.Name);
                file.CopyTo(targetFilePath);
            }
            // If recursive and copying subdirectories, recursively call this method
            if (recursive)
            {
                foreach (DirectoryInfo subDir in dirs)
                {
                    string newDestinationDir = Path.Combine(destinationDir, subDir.Name);
                    CopyDirectory(subDir.FullName, newDestinationDir, true);
                }
            }
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to our Virtual Disk Shell\n");
            Console.WriteLine("Developed By Mohei & Mokhtar & Saied\n");
            String Dir = @"M:\>";
            string Help = " cd      -Change the current default directory to . If the argument is not present, report the current\n" +
                          " clr     -Clear the screen\n" +
                          " quit    -Quit the shell\n" +
                          " copy    -Copies one or more files to another location\n" +
                          " del     -Deletes one or more files\n" +
                          " md      -Creates a directory\n" +
                          " rd      -Removes a directory\n" +
                          " rename  -Renames a file\n" +
                          " type    -Displays the contents of a text file\n" +
                          " help    -Display the user manual using the more filter.\n" +
                          " import  -import text file(s) from your computer\n" +
                          " export  -export text file(s) to your computer\n";
            Virtual_Disk M = new Virtual_Disk();
            M.initialz(@"D:\M.txt");
            Mini_FAT Fat = new Mini_FAT();
            //Fat.Print_FAT();
            Fat.Prepare_FAT();
            Fat.Write_FAT();
            
            while (true)
            {
                Console.WriteLine(Dir); 
                string input = Console.ReadLine();
                string[] command = input.Split(" ");
                               
                if ( command[0]=="help")
                {       
                    if (input == command[0])
                    {
                        Console.WriteLine(Help);
                    }
                    else if (command[1] == "cd")
                    {
                        Console.WriteLine("Change the current default directory to . If the argument is not present, report the current");
                    }
                    else if (command[1] == "clr")
                    {
                        Console.WriteLine("Clear the screen");
                    }
                    else if (command[1] == "quit")
                    {
                        Console.WriteLine("Quit the shell");
                    }
                    else if (command[1] == "copy")
                    {
                        Console.WriteLine("Copies one or more files to another location");
                    }
                    else if (command[1] == "del")
                    {
                        Console.WriteLine("Deletes one or more file");
                    }
                    else if (command[1] == "md")
                    {
                        Console.WriteLine("Creates a directory");
                    }
                    else if (command[1] == "rd")
                    {
                        Console.WriteLine("Removes a directory");
                    }
                    else if (command[1] == "rename")
                    {
                        Console.WriteLine("Renames a file");
                    }
                    else if (command[1] == "type")
                    {
                        Console.WriteLine("Displays the contents of a text file");
                    }
                    else if (command[1] == "help")
                    {
                        Console.WriteLine("Display the user manual using the more filter.");
                    }
                    else if (command[1] == "import")
                    {
                        Console.WriteLine("import text file(s) from your computer");
                    }
                    else if (command[1] == "export")
                    {
                        Console.WriteLine("export text file(s) to your computer");
                    }
                    else
                    {
                        Console.WriteLine("This command is not supported by the help utility.");
                    }
                }
                else if (command[0] == "clr")
                {
                    Console.Clear();
                }
                else if (command[0] == "quit")
                {
                    System.Environment.Exit(0);
                }
                else if (command[0] == "cd")
                {
                    if (input == command[0]) //cd
                    {
                        Console.WriteLine(Dir);
                    }
                    else if (!File.Exists(command[1])) //cd 'not exists directory name'
                    {
                        Console.WriteLine("The system cannot find the path specified.");
                    }
                    
                }
                else if (command[0] == "dir")
                {
                    /*string dirName = command[0];
                    var files = Directory.GetFiles(dirName, @".", SearchOption.AllDirectories);
                    foreach (string file in files)
                    {
                        Console.WriteLine(file);
                    }*/
                }
                else if (command[0] == "copy")
                {
                    if (input == command[0]) //copy 
                    {
                        Console.WriteLine("The syntax of the command is incorrect.");
                    }
                    else // copy sourc target
                    {
                        string source = command[1];
                        string target = command[2];
                        CopyDirectory(source, target, true);
                    }
                }
                else if (command[0] =="rename")
                {
                    string filename = command[1];
                    System.IO.FileInfo file = new System.IO.FileInfo(filename);
                    if (file.Exists) // if the file exists
                    { 
                        file.MoveTo(command[2]);
                        Console.WriteLine("File Renamed.");
                    }
                    else // if the file not exists
                    {
                        File.Create(filename);
                        Console.WriteLine("File is Not Exict And Will be Created ");
                    }

                }
                else if (command[0] == "del")
                {
                    string filename = command[1];
                    System.IO.FileInfo file = new System.IO.FileInfo(filename);
                    if (input == command[0])
                    {
                        Console.WriteLine("The syntax of the command is incorrect.");
                    }
                    else if (file.Exists)
                    {
                        string myfile = command[1];
                        Console.WriteLine("Do You Want To Delete This File Only ? y/n");
                        string sure = Console.ReadLine();
                        if (sure == "y")
                        {
                            File.Delete(myfile);
                            Console.WriteLine("Specified file has been deleted");
                        }
                    }
                    else
                    {
                        Console.WriteLine("This File Does not Exist");
                    }

                }
                else if (command[0] == "md")
                {
                    string dir = command[1];
                    if (input == command[0])
                    {
                        Console.WriteLine("The syntax of the command is incorrect.");
                    }else if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                        Console.WriteLine($"Directory  has created sucssesfully in this path '{command[1]}' ");
                    }
                    else if (Directory.Exists(dir))
                    {
                        Console.WriteLine($"This directory is already exists in this path '{command[1]}'");
                    }
                }
                else if (command[0] == "rd")
                {
                    string dirname = command[1];
                    System.IO.FileInfo file = new System.IO.FileInfo(dirname);
                    if (input == command[0])
                    {
                        Console.WriteLine("The syntax of the command is incorrect.");
                    }
                    else if (Directory.Exists(dirname))
                    {
                        Console.WriteLine("Are You Sure You want to Delete This Folder ? y/n ");
                        string sure = Console.ReadLine();
                        if (sure == "y")
                        {
                            Directory.Delete(dirname);
                            Console.WriteLine("Specified directory has been deleted");
                        }
                        if (sure == "N")
                        {
                            break;
                        }
                    }
                    else if (!file.Exists)
                    {
                        Console.WriteLine("Directory is not found.");
                    }
                }
                else if (command[0] == "type")
                {
                    string filename = command[1];
                    System.IO.FileInfo file = new System.IO.FileInfo(filename);
                    if (input == command[0])
                    {
                        Console.WriteLine("The syntax of the command is incorrect.");
                    }
                    else if (file.Exists)
                    {    
                        // Read a text file line by line.
                        string[] lines = File.ReadAllLines(filename);

                        foreach (string line in lines)
                            Console.WriteLine(line);
                    }
                    else if (!file.Exists)
                    {
                        Console.WriteLine($"'{command[1]}' file is not exist.");
                    }
                }
                else if (command[0] == "import")
                {
                      
                }
                else if (command[0] == "export")
                {

                }
                else
                {
                    WriteLine($"'{input}' is not recognized as an internal or external command operable program or batch file..");
                }
            }
        }
    }
}
