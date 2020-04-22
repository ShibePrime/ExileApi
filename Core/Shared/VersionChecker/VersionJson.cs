using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExileCore.Shared.VersionChecker
{
    public struct VersionJson
    {
        public int Major { get; set; }
        public int Minor { get; set; }
        public int Patch { get; set; }

        [JsonIgnore]
        public string VersionString => $"{Major}.{Minor}.{Patch}";
    }
}
