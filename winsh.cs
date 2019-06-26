using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace CLI {
    class Program
    {
        static void Main(string[] args)
        {
            var shell = new Shell();
            shell.Run();
        }
    }

    public class Shell {

        const string BatchFileName = "run.bat";
        public void Run()
        {
			
            String input = null;
			String[] batchLines = File.ReadAllLines(BatchFileName); 
			// run the batch commands
			foreach (string line in batchLines) {
				Execute(line); 
			}				
			
			
            // keep looping until user enters "exit"
            do
            {
                Console.Write("winsh> ");
                input = Console.ReadLine();
                Execute(input);
            } while (input != "exit");


        }

        public int Execute(string input)
        {
            string[] inputs = input.Split(' ');

            if (inputs[0] == "ls")
            {
                DirectoryInfo dir = new DirectoryInfo(Directory.GetCurrentDirectory());

                foreach (DirectoryInfo d in dir.GetDirectories())
                {
                    Console.WriteLine("{0, -30}\t directory", d.Name);
                }

                foreach (FileInfo f in dir.GetFiles())
                {
                    Console.WriteLine("{0, -30}\t File", f.Name);
                }
                return 0;
            }

            if(inputs[0] == "man")
            {
                Console.WriteLine("winsh shell");
                Console.WriteLine("A Unix-like Shell written in C#");
                Console.WriteLine("_______COMMANDS___________________________________");
                Console.WriteLine("man     -> manual");
                Console.WriteLine("ls      -> list current directory and filetype");
				Console.WriteLine("more    -> display the file in terminal");
				Console.WriteLine("	          Ex: more file1");
				Console.WriteLine("cp      -> copy file to destination");
				Console.WriteLine("	          Ex: cp file1 file2");
				Console.WriteLine("grep    -> search a file for specific text");
				Console.WriteLine("	          Ex: grep text file1");
				Console.WriteLine("clear   -> clear the screen");
				Console.WriteLine("pwd     -> show the directory");
				Console.WriteLine("whoami  -> show the current user");
				Console.WriteLine("rm      -> delete a file");
				Console.WriteLine("	          Ex: rm file1");
				Console.WriteLine("ps      -> show running processes");
				Console.WriteLine("__________________________________________________");
                return 0;

            }

            if(inputs[0] == "more")
            {
                String[] lines;
                lines = File.ReadAllLines(inputs[1]);
                foreach (string line in lines)
                {
                    Console.WriteLine(line);
                }
                return 0; 
            }
                
            if(inputs[0] == "cp")
            {
                if (inputs.Length != 3)
                {
                    Console.WriteLine("The syntax of the command is incorrect.");
                }
                else
                {
                    if (File.Exists(inputs[1]) == false)
                    {
                        Console.WriteLine("Source file not found");
                    }
                    else if (File.Exists(inputs[2]) == false)
                    {
                        Console.WriteLine("Target file not found");
                    }
                    else
                    {
                        File.Copy(inputs[1], inputs[2] + "\\" + inputs[1]);
                    }
                }
                return 0;
            }

            if (inputs[0] == "grep")
            {
                if (inputs.Length != 3)
                {
                    Console.WriteLine("The syntax of the command is incorrect.");
                }
                else
                {
                    if (File.Exists(inputs[2]) == false)
                    {
                        Console.WriteLine("File not found");
                    }
                    else
                    {
                        string[] lines = File.ReadAllLines(inputs[2]);

                        foreach (string line in lines)
                        {
                            if (line.Contains(inputs[1]))
                            {
                                Console.WriteLine(line);
                            }
                        }
                    }

                }
                return 0;
            }

            if (inputs[0] == "clear") 
			{ 
                for(int i=0; i<30; ++i) {
                    Console.WriteLine("        ");
                }
                return 0;
            }

            if (inputs[0] == "pwd") 
			{ 
                Console.WriteLine(Directory.GetCurrentDirectory());
                return 0;
            }
			
            if (inputs[0] == "whoami") 
			{ 
                Console.WriteLine(Environment.UserName);
                return 0;
            }

            if (inputs[0] == "ps") 
			{ 
                Process[] mYProcs = Process.GetProcesses();

                Console.WriteLine("-----------------------------------------------------");
                Console.WriteLine("{0, -8} {1, -30} {2, -10}", "PID", "Process Name", "Status");
                Console.WriteLine("-----------------------------------------------------");

                foreach (Process p in mYProcs)
                {
                    try
                    {
                        Console.WriteLine("{0, -8} {1, -30} {2, -10}", p.Id, 
                        p.ProcessName, p.Responding ? "Running" : "IDLE");
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
                return 0;
            }    
 

            if (inputs[0] == "rm") 
			{ 
                if (inputs.Length != 2)
                {
                    Console.WriteLine("No file specified to remove.");
                }
                else
                {
                    if (File.Exists(inputs[1]) == false)
                    {
                        Console.WriteLine("File not found");
                    }
                    else
                    {
                        Console.WriteLine("Are you sure you want to delete {0} (y/n)", inputs[1]);
                        ConsoleKeyInfo key = Console.ReadKey();
                        if (key.Key == ConsoleKey.Y)
                        {
                            File.Delete(inputs[1]);
                            Console.WriteLine("\nFile Deleted: {0}", inputs[1]);
                        }
                    }
                }
				return 0;
            }
			
            return 1;
        }

    }
}