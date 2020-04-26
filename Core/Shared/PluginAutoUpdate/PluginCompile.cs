using ExileCore.Shared.Helpers;
using JM.LinqFaster;
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
    public class PluginCompiler
    {
        DirectoryInfo RootDirectoryInfo { get; set; }
        private CodeDomProvider Provider { get; set; }
        private string[] DllFiles { get; set; }

        public PluginCompiler(DirectoryInfo rootDirectoryInfo)
        {
            RootDirectoryInfo = rootDirectoryInfo;
            (Provider, DllFiles) = PrepareCompilation(RootDirectoryInfo);
        }

        private (CodeDomProvider provider, string[] dllFiles) PrepareCompilation(DirectoryInfo rootDirectoryInfo)
        {
            using (CodeDomProvider provider =
                new CSharpCodeProvider())
            using (new PerformanceTimer("Compile source plugins"))
            {
                var _compilerSettings = provider.GetType()
                    .GetField("_compilerSettings", BindingFlags.Instance | BindingFlags.NonPublic)
                    .GetValue(provider);

                var _compilerFullPath = _compilerSettings
                    .GetType().GetField("_compilerFullPath", BindingFlags.Instance | BindingFlags.NonPublic);

                _compilerFullPath.SetValue(_compilerSettings,
                    ((string)_compilerFullPath.GetValue(_compilerSettings)).Replace(@"bin\roslyn\", @"roslyn\"));

                var dllFiles = rootDirectoryInfo.GetFiles("*.dll", SearchOption.TopDirectoryOnly)
                    .WhereF(x => !x.Name.Equals("cimgui.dll") && x.Name.Count(c => c == '-' || c == '_') != 5)
                    .SelectF(x => x.FullName).ToArray();

                return (provider, dllFiles);
            }
        }

        public Assembly CompilePlugin(DirectoryInfo source, string outputDirectory)
        {
            var csFiles = source.GetFiles("*.cs", SearchOption.AllDirectories).Select(x => x.FullName)
                .ToArray();

            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }
            var parameters = new CompilerParameters
            {
                GenerateExecutable = false,
                CompilerOptions = "/optimize /unsafe",
                OutputAssembly = Path.Combine(outputDirectory, $"{source.Name}.dll")
            };

            parameters.ReferencedAssemblies.AddRange(DllFiles);
            var csprojPath = source.GetFiles($"{source.Name}.csproj", SearchOption.AllDirectories).Select(x => x.FullName).FirstOrDefault();

            if (File.Exists(csprojPath))
            {
                var readAllLines = File.ReadAllLines(csprojPath);

                var refer = readAllLines
                    .WhereF(x =>
                        x.TrimStart().StartsWith("<Reference Include=") && x.TrimEnd().EndsWith("/>"));

                var refer2 = readAllLines.Where(x =>
                    x.TrimStart().StartsWith("<Reference Include=") && x.TrimEnd().EndsWith("\">") &&
                    x.Contains(","));

                foreach (var r in refer)
                {
                    var arr = new int[2] { 0, 0 };
                    var j = 0;

                    for (var i = 0; i < r.Length; i++)
                        if (r[i] == '"')
                        {
                            arr[j] = i;
                            j++;
                        }

                    if (arr[1] != 0)
                    {
                        var dll = $"{r.Substring(arr[0] + 1, arr[1] - arr[0] - 1)}.dll";
                        parameters.ReferencedAssemblies.Add(dll);
                    }
                }
            }

            var libDlls = FindDllsFromCompiledDirectory(outputDirectory, source.Name);
            parameters.ReferencedAssemblies.AddRange(libDlls);

            CompilerResults result = null;
            try
            {
                result = Provider.CompileAssemblyFromFile(parameters, csFiles);
            } 
            catch (Exception e)
            {
                DebugWindow.LogError($"{source.Name} -> Compiling from Assembly throw error.");
                DebugWindow.LogDebug($"{source.Name} -> {e.Message}");
            }

            if (result == null)
            {
                DebugWindow.LogError($"{source.Name} -> Compile failed! Assembly (result) is null.");
                return null;
            }

            if (result.Errors.HasErrors == true)
            {
                var AllErrors = "";

                foreach (CompilerError compilerError in result.Errors)
                {
                    AllErrors += compilerError + Environment.NewLine;
                    DebugWindow.LogError($"{source.Name} -> {compilerError}");
                }

                //File.WriteAllText(Path.Combine(info.FullName, "Errors.txt"), AllErrors);
            }
            else
            {
                return result.CompiledAssembly;
            }

            return null;
        }

        private string[] FindDllsFromCompiledDirectory(string compiledPath, string pluginName)
        {
            var dllFiles = Directory.GetFiles(compiledPath, "*.dll")
                .Where(f => new DirectoryInfo(f)?.Name?.Equals($"{pluginName}.dll") == false)
                .ToArray();
            return dllFiles;
        }

        // currently not in use, due to .dll's are placed in top level directory
        private string[] FindDllsFromDependencyDirectory(string compiledPath)
        {
            var dllFiles = new List<string>();
            foreach (var possibleDirectoryName in PluginCopyFiles.DependenciesDirectoryNames)
            {
                var path = Path.Combine(compiledPath, possibleDirectoryName);
                if (!Directory.Exists(path)) continue;
                var libsDll = Directory.GetFiles(path, "*.dll");
                dllFiles.AddRange(libsDll);
            }
            return dllFiles.ToArray();
        }
    }
}
