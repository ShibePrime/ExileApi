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
            try
            {
                string updateFolderPath;
                string unzippedFolderName;
                string mainExecuteablePath;
                if (args.Length < 3)
                {
                    Console.WriteLine("Updater started manually without path arguments");
                    string basePath = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
                    var baseDirectory = Path.GetDirectoryName(basePath);

                    updateFolderPath = Path.Combine(baseDirectory, "update");
                    unzippedFolderName = "PoeHelper";
                    mainExecuteablePath = Path.Combine(baseDirectory, "Loader.exe");

                    if (Directory.GetFiles(updateFolderPath).Length == 0)
                    {
                        Console.WriteLine("No update possible. To run the HUD, launch the Loader.exe");
                        Console.Read();
                        return;
                    }
                }
                else
                {
                    updateFolderPath = args[0];
                    unzippedFolderName = args[1];
                    mainExecuteablePath = args[2];
                }
                Console.WriteLine($"Log -> updateFolderPath: {updateFolderPath}");
                Console.WriteLine($"Log -> releaseFileName: {unzippedFolderName}");
                Console.WriteLine($"Log -> mainExecuteablePath: {mainExecuteablePath}");

                var exeFolderPath = Path.GetDirectoryName(mainExecuteablePath);
                var tempFolderPath = Path.Combine(exeFolderPath, "temp");
                Console.WriteLine($"Log -> Cleaning directory: {tempFolderPath}");
                CleanFolder(tempFolderPath);

                var executeableName = Path.GetFileName(mainExecuteablePath);
                var processesToKillTasks = new Task<bool>[]
                {
                    Task.Run(() => KillExecuteable(executeableName, 15)),
                    Task.Run(() => KillExecuteable("csc.exe", 15)),
                    Task.Run(() => KillExecuteable("VBCSCompiler.exe", 15))
                };

                Task.WaitAll(processesToKillTasks);


                var unzippedPath = Path.Combine(updateFolderPath, unzippedFolderName);

                Directory.CreateDirectory(tempFolderPath);
                ReplaceDirectory(unzippedPath, exeFolderPath, tempFolderPath);

                Console.WriteLine($"Log -> Starting main executeable...");
                Process.Start(mainExecuteablePath);

                Console.WriteLine($"Log -> Cleaning directory: {updateFolderPath}");
                CleanFolder(updateFolderPath);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error -> Something during the update went wrong.");
                Console.WriteLine("Error -> When you are not able to start the program manually you may need to reinstall it.");
                Console.WriteLine("Error -> " + e.Message);
                Console.Read();
            }
        }

        private static bool KillExecuteable(string executeableName, int maxSeconds)
        {
            var nameWithoutExtension = StringArrayToString(executeableName.Split('.'), 1);

            try
            {
                var timer = new Stopwatch();
                timer.Start();
                var maxMs = maxSeconds * 1000;

                while (IsProcessOpen(nameWithoutExtension) && timer.ElapsedMilliseconds < maxMs)
                {
                    Console.WriteLine($"Log -> Try to kill {executeableName}");
                    KillProcess(nameWithoutExtension);
                    if (!IsProcessOpen(nameWithoutExtension)) return true;

                    Console.WriteLine($"Log -> {executeableName} is still running... {timer.ElapsedMilliseconds} / {maxMs}");
                    Thread.Sleep(500);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Warning -> Unable to kill {executeableName}");
                Console.WriteLine($"Warning -> {e}");
            }


            return !IsProcessOpen(nameWithoutExtension);
        }

        public static bool IsProcessOpen(string name)
        {
            return Process.GetProcessesByName(name).Count() > 0;
        }

        public static void KillProcess(string name)
        {
            var processes = Process.GetProcessesByName(name);
            foreach (var process in processes)
            {
                process.Kill();
            }
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

        public static void ReplaceDirectory(string source, string target, string tempFolder)
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
                            Console.WriteLine($"Log -> File can not be deleted. Try to replace it by copying: {targetFile}");
                            try
                            {
                                var tempFileName = ReplaceLastOccurrence(Path.GetFileName(file), ".", random.Next(1000, 9999) + ".");
                                File.Move(targetFile, Path.Combine(tempFolder, tempFileName));
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

        private static void CleanFolder(string updateFolder)
        {
            var directory = new DirectoryInfo(updateFolder);
            if (!directory.Exists) return;

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
