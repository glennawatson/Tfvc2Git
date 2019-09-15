using System.Text.Json.Serialization;

namespace Tfvc2Git.Model.Services.Model
{
    public class TfvcMergeSource
    {
        [JsonPropertyName("isRename")]
        public bool IsRename { get; set; }
        [JsonPropertyName("serverItem")]
        public string ServerItem { get; set; }
        [JsonPropertyName("versionFrom")]
        public int VersionFrom { get; set; }
        [JsonPropertyName("versionTo")]
        public int VersionTo { get; set; }
    }
}
