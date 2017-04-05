using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * This Application:
 * - creates all directories in the Destinatin that are in the Source
 * - deletes directories in the Destination when the parent Source directory is later and they are not present in Source
 * - copies files from Source that are not existent in Destination
 * - copies the later of the files from Source or Destination
 * - deletes files in the Destination when the parent Source directory is later and they are not present in Source
 */

namespace SimpleSync
{
    class Program
    {
        static FileStream TreeFile = new FileStream(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ".tree"), FileMode.Create);

        static void RecursiveSync(string SourcePath, string DestPath)
        {
            string NextSourcePath, NextSourceFile, NextDestPath, NextDestFile;

            //Get all Directories in source
            string[] listSourceDirs = Directory.GetDirectories(SourcePath);
            //Get all Directories in destination
            string[] listDestDirs = Directory.GetDirectories(DestPath);
            //Get all Files in source
            string[] listSourceFiles = Directory.GetFiles(SourcePath);
            //Get all Files in destination
            string[] listDestFiles = Directory.GetFiles(DestPath);

            TreeFile.Write(Encoding.ASCII.GetBytes(SourcePath), 0, SourcePath.Length);
            TreeFile.Write(Encoding.ASCII.GetBytes("\n"), 0, 1);

            foreach (string currDestDir in listDestDirs)
            {
                NextSourcePath = Path.Combine(SourcePath, Path.GetFileName(currDestDir));
                NextDestPath = currDestDir;
                if (Directory.GetLastAccessTimeUtc(DestPath) < Directory.GetLastAccessTimeUtc(SourcePath))
                {
                    if (!Directory.Exists(NextSourcePath))
                        Directory.Delete(NextDestPath, true);
                }
            }

            foreach (string currDir in listSourceDirs)
            {
                //Update to the current Paths
                NextDestPath = Path.Combine(DestPath, Path.GetFileName(currDir));
                NextSourcePath = currDir;

                if (!Directory.Exists(NextDestPath))
                {
                    //Create missing directory
                    Directory.CreateDirectory(NextDestPath);
                }

                // Enter the Next Directory
                RecursiveSync(NextSourcePath, NextDestPath);
            }

            // For all the Files in the current destination directory
            foreach (string currDestFile in listDestFiles)
            {
                NextSourceFile = Path.Combine(SourcePath, Path.GetFileName(currDestFile));
                NextDestFile = currDestFile;
                if (Directory.GetLastAccessTimeUtc(DestPath) < Directory.GetLastAccessTimeUtc(SourcePath))
                {
                    if (!File.Exists(NextSourceFile))
                    {
                        File.Delete(NextDestFile);
                    }
                }
            }

            //For all Files in the current source directory
            foreach (string currFile in listSourceFiles)
            {
                NextDestFile = Path.Combine(DestPath, Path.GetFileName(currFile));
                //check if the file already exists in the destination
                if (File.Exists(NextDestFile))
                {
                    //Check which of the files is latest
                    if (File.GetLastWriteTimeUtc(NextDestFile) < File.GetLastWriteTimeUtc(currFile))
                    {
                        // Copy the Source File
                        File.Copy(currFile, NextDestFile, true);
                    }
                    else if (File.GetLastWriteTimeUtc(NextDestFile) > File.GetLastWriteTimeUtc(currFile))
                    {
                        // Copy the Destination File
                        File.Copy(NextDestFile, currFile, true);
                    }
                    else
                    {
                        // Files are equal so leave everything as it is :)
                    }
                }
                else
                {
                    // File not yet exixtent in Destination. Copy the Source file
                    File.Copy(currFile, NextDestFile, true);
                }
            }

        }

        static void Main(string[] args)
        {
            string sourceDirectory = @"Default";
            string destinationDirectory = @"Default";
            Uri tmpUri;
            
            //string currDir, currFile;
            if (args.GetLength(0) < 2)
            {
                //Console.WriteLine("To few arguments. Usage: command <Source> <Destination>\n");
                //return;
                // Use fixed paths instead:
                //tmpUri = new Uri(Path.GetDirectoryName( System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase)); // My Own Path
                //sourceDirectory = Path.Combine(tmpUri.LocalPath,"Thesis");
                sourceDirectory = System.IO.Path.Combine("F:\\", "Thesis");
                destinationDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),"Documents", "Thesis"); // Users Path on the PC
            }
            else
            {
                sourceDirectory = args[0];
                destinationDirectory = args[1];
            }
            if (!Directory.Exists(sourceDirectory))
            {
                Console.WriteLine("Source Path does not exist!\n");
                return;
            }
            if (!Directory.Exists(destinationDirectory))
            {
                Console.WriteLine("Destination Path does not exist!\n");
                return;
            }

            RecursiveSync(sourceDirectory, destinationDirectory);
        }
    }
}
