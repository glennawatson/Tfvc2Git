using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Tfvc2Git.Model.Services.Model
{
    public class TfvcChange
    {
        [JsonPropertyName("changeType")]
        public VersionControlChangeType ChangeType { get; set; }
        [JsonPropertyName("item")]
        public TfvcItem Item { get; set; }
        [JsonPropertyName("mergeSources")]
        public List<TfvcMergeSource> MergeSources { get; set; }
        [JsonPropertyName("newContent")]
        public ItemContent NewContent { get; set; }
        [JsonPropertyName("pendingVersion")]
        public int PendingVersion { get; set; }
        [JsonPropertyName("sourceServerItem")]
        public string SourceServerItem { get; set; }
        [JsonPropertyName("url")]
        public string Url { get; set; }
    }
}
