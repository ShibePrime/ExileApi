using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExileCore.Shared.VersionChecker
{
    public struct GithubReleaseResponse
    {
        [JsonProperty("name")]
        public string VersionString { get; set; }

        [JsonProperty("assests")]
        public IList<GithubReleaseAsset> Assets { get; set; }
    }

    public struct GithubReleaseAsset
    {
        [JsonProperty("name")]
        public string FileName { get; set; }
        [JsonProperty("browser_download_url")]
        public string BrowserDownloadUrl { get; set; }
    }
}
