using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Updater
{
    class Program
    {
        static void Main(string[] args)
        {
            string updateFolderPath;
            string releaseFileName;
            string mainExecuteablePath;
            if (args.Length < 3)
            {
                Console.WriteLine("To run the HUD, launch the Loader.exe");
                Console.WriteLine("Updater started manually without path arguments");
                string basePath = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
                var baseDirectory = Path.GetDirectoryName(basePath);
                updateFolderPath = Path.Combine(baseDirectory, "update");
                releaseFileName = "PoeHelper.zip";
                mainExecuteablePath = Path.Combine(baseDirectory, "Loader.exe");
            }
            else
            {
                updateFolderPath = args[0];
                releaseFileName = args[1];
                mainExecuteablePath = args[2];
            }
            Console.WriteLine($"Log -> updateFolderPath: {updateFolderPath}");
            Console.WriteLine($"Log -> releaseFileName: {releaseFileName}");
            Console.WriteLine($"Log -> mainExecuteablePath: {mainExecuteablePath}");

            var executeableName = Path.GetFileName(mainExecuteablePath);
            if (!WaitTillMainExeIsClosed(executeableName, 15))
            {
                Console.WriteLine($"Error -> Main Application was not closed in time, update failed");
                Console.Read();
                return;
            }

            try
            {
                var releaseFileFullPath = Path.Combine(updateFolderPath, releaseFileName);
                if (!File.Exists(releaseFileFullPath))
                {
                    Console.WriteLine($"Error -> Release (zip) file does not exists, expected location: {releaseFileFullPath}");
                    Console.Read();
                    return;
                }
                var resultName = Unzip(updateFolderPath, releaseFileName);
                ReplaceDirectory(resultName, Path.GetDirectoryName(mainExecuteablePath));

                Console.WriteLine($"Log -> Starting main executeable...");
                Process.Start(mainExecuteablePath);

                Console.WriteLine($"Log -> Cleaning update folder.");
                CleanUpdateFolder(updateFolderPath);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error -> Something during the update went wrong.");
                Console.WriteLine("Error -> When you are not able to start the program manually you may need to reinstall it.");
                Console.WriteLine("Error -> " + e.Message);
            }
            Console.Read();
        }

        private static bool WaitTillMainExeIsClosed(string executeableName, int maxSeconds)
        {
            for (var i = 0; i < maxSeconds; i++)
            {
                if (!IsProcessOpen(executeableName)) return true;
                Console.WriteLine($"Log -> Waiting till Main Application is closed... {i}s / {maxSeconds}s");
                Thread.Sleep(1000);
            }
            return false;
        }

        public static bool IsProcessOpen(string name)
        {
            var nameWithoutExtension = StringArrayToString(name.Split('.'), 1);
            foreach (Process clsProcess in Process.GetProcesses())
            {
                if (clsProcess.ProcessName.Contains(nameWithoutExtension))
                {
                    return true;
                }
            }

            return false;
        }

        private static string Unzip(string updateFolder, string fileName)
        {
            var fileNameWithoutExtension = StringArrayToString(fileName.Split('.'), 1);
            var resultFolder = Path.Combine(updateFolder, fileNameWithoutExtension);
            ZipFile.ExtractToDirectory(
                Path.Combine(updateFolder, fileName),
                resultFolder
            );
            return resultFolder;
        }

        private static string StringArrayToString(string[] array, int skipFromBehind)
        {
            string result = "";
            for (var i = 0; i < array.Length - skipFromBehind; i++)
            {
                result += array[i];
            }
            return result;
        }

        public static void ReplaceDirectory(string source, string target)
        {
            Random random = new Random();
            var sourcePath = source.TrimEnd('\\', ' ');
            var targetPath = target.TrimEnd('\\', ' ');
            var files = Directory.EnumerateFiles(sourcePath, "*", SearchOption.AllDirectories)
                                 .GroupBy(s => Path.GetDirectoryName(s));
            foreach (var folder in files)
            {
                var targetFolder = folder.Key.Replace(sourcePath, targetPath);
                Directory.CreateDirectory(targetFolder);
                foreach (var file in folder)
                {
                    //if (!file.Contains(".dll") && !file.Contains(".exe")) continue;

                    var targetFile = Path.Combine(targetFolder, Path.GetFileName(file));
                    if (File.Exists(targetFile))
                    {
                        try
                        {
                            File.Delete(targetFile);                            
                        }
                        catch
                        {
                            Console.WriteLine($"Log -> File can not be deleted, try copying: {targetFile}");
                            try
                            {
                                var fileName = ReplaceLastOccurrence(targetFile, ".", random.Next(1000, 9999) + ".");
                                File.Move(targetFile, fileName);
                            }
                            catch(Exception e)
                            {
                                Console.WriteLine("Error -> File copy failed. " + targetFile);
                                Console.WriteLine("Error -> " + e.Message);
                                continue;
                            }
                        }
                    }
                    File.Move(file, targetFile);
                }
            }
        }

        public static string ReplaceLastOccurrence(string source, string find, string replace)
        {
            int place = source.LastIndexOf(find);

            if (place == -1)
                return source;

            string result = source.Remove(place, find.Length).Insert(place, replace);
            return result;
        }

        private static void CleanUpdateFolder(string updateFolder)
        {
            var directory = new DirectoryInfo(updateFolder);

            foreach (FileInfo file in directory.EnumerateFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in directory.EnumerateDirectories())
            {
                dir.Delete(true);
            }
        }
    }
}
