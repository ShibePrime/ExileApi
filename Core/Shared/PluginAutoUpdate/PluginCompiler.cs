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
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExileCore.Shared.PluginAutoUpdate
{
    public class PluginCompilerSettings : ICompilerSettings
    {
        public PluginCompilerSettings(DirectoryInfo exileApiRootDirectory)
        {
            if (exileApiRootDirectory == null) throw new ArgumentNullException(nameof(exileApiRootDirectory));
            var roslynDir = exileApiRootDirectory.GetDirectories().Single(d => d.Name == "roslyn");
            _roslynCscExe = roslynDir.GetFiles().Single(f => f.Name == "csc.exe");
        }

        private FileInfo _roslynCscExe { get; }
        public string CompilerFullPath => _roslynCscExe.FullName;
        public int CompilerServerTimeToLive => 900;
    }

    public class PluginCompiler : IPluginCompiler
    {
        public void CompilePlugin(FileInfo csProj, string outputDirectory, DirectoryInfo exileApiRootDirectory)
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
            foreach (var missingDependency in missingDependencies)
            {
                DebugWindow.LogError($"{pluginName} -> Dependency is missing: {missingDependency.name}");
            }

            var compilerSettings = new PluginCompilerSettings(exileApiRootDirectory);
            using (CodeDomProvider provider = new CSharpCodeProvider(compilerSettings))
            {
                try
                {
                    var result = provider.CompileAssemblyFromFile(parameters, csFiles);
                    if (result.Errors.Count > 0)
                    {
                        DebugWindow.LogError($"{pluginName} -> CompilePlugin failed");
                        foreach (var error in result.Errors)
                        {
                            DebugWindow.LogError($"{pluginName} -> {error}");
                        }
                    }
                }
                catch (Exception e)
                {
                    DebugWindow.LogError($"{pluginName} -> CompilePlugin failed");
                    DebugWindow.LogError($"{pluginName} -> {e}");
                }
            }
        }

        private void TryToAddReferencedAssemblies(
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

        private List<(string name, bool isAdded)> GetDependenciesFromCsProjLines(ICollection<string> lines)
        {
            /*
             * TODO consider using Regexp for better readability or a single loop checking every line for all possible cases.
             */
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

            var packageReferences = lines
                .Where(line => line.TrimStart().StartsWith("<PackageReference Include="))
                .Where(line => line.TrimEnd().EndsWith("\">"))
                .ToList();

            foreach (var packageReference in packageReferences)
            {
                var referenceName = packageReference
                    .Replace(" ", "")
                    .Replace("<PackageReferenceInclude=\"", "")
                    .Replace("\">", "");
                dependencies.Add((referenceName + ".dll", false));
            }

            var projectReferences = lines
                .Where(line => line.TrimStart().StartsWith("<ProjectReference Include="))
                .Where(line => line.TrimEnd().EndsWith("\">"))
                .ToList();

            foreach (var projectReference in projectReferences)
            {
                var referenceName = ParseProjectReferences(projectReference) + ".dll";
                if (dependencies.Any(d => d.name == referenceName)) continue;
                dependencies.Add((referenceName, false));
            }

            return dependencies;
        }

        private string ParseProjectReferences(string projectReferenceLine)
        {
            var referenceName = projectReferenceLine
                .Replace(" ", "")
                .Replace("\">", "")
                .Replace(".csproj", "")
                .Split('\\')
                .Last();

            if (referenceName == "Core") return "ExileCore";

            return referenceName;
        }
    }
}
