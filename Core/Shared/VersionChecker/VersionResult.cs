using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExileCore.Shared.VersionChecker
{
    public enum VersionResult
    {
        Loading,
        UpToDate,
        MajorUpdate,
        MinorUpdate,
        PatchUpdate,
        Error
    }

    public static class VersionResultExtension
    {
        public static bool IsUpdate(this VersionResult vr)
        {
            return vr == VersionResult.MajorUpdate || vr == VersionResult.MinorUpdate || vr == VersionResult.PatchUpdate;
        }
    }
}
