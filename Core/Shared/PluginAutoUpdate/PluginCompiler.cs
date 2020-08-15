using Microsoft.Build.Evaluation;
using Microsoft.Build.Execution;
using Microsoft.Build.Framework;
using Microsoft.Build.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExileCore.Shared.PluginAutoUpdate
{
    public class PluginCompiler : IPluginCompiler
    {
        public void CompilePlugin(DirectoryInfo source, string outputDirectory)
        {
            var globalProperty = new Dictionary<string, string> { 
                { "Configuration", "Debug" }, 
                { "Platform", "AnyCPU" },
                { "OutputPath", Path.Combine(outputDirectory, $"{source.Name}.dll") },
            };

            var buildParameters = new BuildParameters(new ProjectCollection()) { Loggers = new List<ILogger> { new ConsoleLogger() } };
            var buildRequest = new BuildRequestData(source.FullName, globalProperty, null, new[] { "Build" }, null);
            BuildResult buildResult = BuildManager.DefaultBuildManager.Build(buildParameters, buildRequest);
        }
    }
}
