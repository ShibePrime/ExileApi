using Microsoft.Build.Evaluation;
using Microsoft.Build.Execution;
using Microsoft.Build.Framework;
using Microsoft.Build.Logging;
using Microsoft.CodeDom.Providers.DotNetCompilerPlatform;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginCompiler
{
    class Program
    {
        static void Main(string[] args)
        {
            var csProj = new FileInfo(@"E:\Git\PoeHUD\PoeHelper\Plugins\Source\GetChaosValue\Ninja Price\Ninja Price.csproj");
            var output = @"E:\Git\PoeHUD\PoeHelper\Plugins\Compiled\Ninja Price";
            var exileApiRoot = new DirectoryInfo(@"E:\Git\PoeHUD\ExileApi");
            CompilePlugin(csProj, output, exileApiRoot);
        }

        public static void CompilePlugin(FileInfo csProj, string outputDirectory, DirectoryInfo exileApiRootDirectory)
        {
            var sourceFolder = csProj.Directory;
            var csFiles = sourceFolder.GetFiles("*.cs", SearchOption.AllDirectories).Select(x => x.FullName)
                .ToArray();

            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }
            var pluginName = csProj.Name.Replace(csProj.Extension, "");
            var parameters = new CompilerParameters
            {
                GenerateExecutable = false,
                CompilerOptions = "/optimize /unsafe",
                OutputAssembly = Path.Combine(outputDirectory, $"{pluginName}.dll"),
            };

            var dependencies = GetDependenciesFromCsProjLines(File.ReadAllLines(csProj.FullName));
            var runtimeDirectory = new DirectoryInfo(System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory());

            TryToAddReferencedAssemblies(ref dependencies, ref parameters, sourceFolder, SearchOption.AllDirectories);
            TryToAddReferencedAssemblies(ref dependencies, ref parameters, exileApiRootDirectory, SearchOption.TopDirectoryOnly);
            TryToAddReferencedAssemblies(ref dependencies, ref parameters, runtimeDirectory, SearchOption.TopDirectoryOnly);

            var missingDependencies = dependencies.Where(d => d.isAdded == false).ToList();

            using (CodeDomProvider provider = new CSharpCodeProvider())
            {
                try
                {
                    var result = provider.CompileAssemblyFromFile(parameters, csFiles);
                }
                catch (Exception e)
                {

                }
            }
        }

        private static void TryToAddReferencedAssemblies(
            ref List<(string name, bool isAdded)> dependencies, 
            ref CompilerParameters parameters, 
            DirectoryInfo directoryToSearch, 
            SearchOption searchOption)
        {
            for (var i = 0; i < dependencies.Count; i++)
            {
                if (dependencies[i].isAdded) continue;

                var dllFiles = directoryToSearch.GetFiles(dependencies[i].name, searchOption);
                var dllFile = dllFiles.FirstOrDefault();
                if (dllFile == null) continue;

                parameters.ReferencedAssemblies.Add(dllFile.FullName);
                dependencies[i] = (dependencies[i].name, true);
            }
        }

        private static List<(string name, bool isAdded)> GetDependenciesFromCsProjLines(ICollection<string> lines)
        {
            var dependencies = new List<(string name, bool isAdded)>();

            var references = lines
                .Where(line => line.TrimStart().StartsWith("<Reference Include="))
                .Where(line => line.TrimEnd().EndsWith("/>"));

            foreach (var reference in references)
            {
                var referenceName = reference
                    .Replace(" ", "")
                    .Replace("<ReferenceInclude=\"", "")
                    .Replace("\"/>", "");
                dependencies.Add((referenceName + ".dll", false));
            }

            var referencesWithIdenty = lines
                .Where(line => line.TrimStart().StartsWith("<Reference Include="))
                .Where(line => line.TrimEnd().EndsWith("\">"))
                .ToList();

            foreach (var referenceWithIdenty in referencesWithIdenty)
            {
                var referenceName = referenceWithIdenty
                    .Replace(" ", "")
                    .Replace("<ReferenceInclude=\"", "")
                    .Replace("\">", "")
                    .Split(',')
                    .First();
                dependencies.Add((referenceName + ".dll", false));
            }

            return dependencies;
        }
    }
}
