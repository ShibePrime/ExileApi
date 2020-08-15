using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExileCore.Shared.PluginAutoUpdate
{
    interface IPluginCompiler
    {
        void CompilePlugin(DirectoryInfo source, string outputDirectory);
    }
}
