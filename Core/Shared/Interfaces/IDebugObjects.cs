using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExileCore.Shared.Interfaces
{
    public interface IDebugObjects
    {
        Dictionary<string, object> Objects { get; }
        void AddOrUpdate(string key, object value);
    }
}
