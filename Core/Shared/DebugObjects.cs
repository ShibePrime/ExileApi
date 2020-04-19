using ExileCore.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExileCore.Shared
{
    public class DebugObjects : IDebugObjects
    {
        public DebugObjects()
        {
            Objects = new Dictionary<string, object>();
        }

        public Dictionary<string, object> Objects { get; }

        public void AddOrUpdate(string key, object value)
        {
            Objects[key] = value;
        }
    }
}
