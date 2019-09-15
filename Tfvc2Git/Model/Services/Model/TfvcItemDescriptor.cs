using System.Text.Json.Serialization;

namespace Tfvc2Git.Model.Services.Model
{
    public class TfvcItemDescriptor
    {
        [JsonPropertyName("path")]
        public string Path { get; set; }
        
        [JsonPropertyName("recursionLevel")]
        public VersionControlRecursionType? RecursionLevel { get; set; }
        
        [JsonPropertyName("version")]
        public string Version { get; set; }
        
        [JsonPropertyName("versionOption")]
        public TfvcVersionOption? VersionOption { get; set; }
        
        [JsonPropertyName("versionType")]
        public TfvcVersionType? VersionType { get; set; }
    }
}
