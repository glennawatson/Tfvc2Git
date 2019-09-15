using System;
using System.Text.Json.Serialization;

namespace Tfvc2Git.Model.Services.Model
{
    public class TfvcItem
    {
        [JsonPropertyName("changeDate")]
        public DateTime ChangeDate { get; set; }
        [JsonPropertyName("content")]
        public string Content { get; set; }
        [JsonPropertyName("contentMetadata")]
        public FileContentMetadata ContentMetadata { get; set; }
        [JsonPropertyName("deletionId")]
        public int DeletionId { get; set; }
        [JsonPropertyName("encoding")]
        public int Encoding { get; set; }
        [JsonPropertyName("hashValue")]
        public string HashValue { get; set; }

        [JsonPropertyName("isBranch")]
        public bool IsBranch { get; set; }
        [JsonPropertyName("isFolder")]
        public bool IsFolder { get; set; }
        [JsonPropertyName("isPendingChange")]
        public bool IsPendingChange { get; set; }
        [JsonPropertyName("isSymLink")]
        public bool IsSymLink { get; set; }
        [JsonPropertyName("path")]
        public string Path { get; set; }
        [JsonPropertyName("size")]
        public int Size { get; set; }
        [JsonPropertyName("url")]
        public string Url { get; set; }
        [JsonPropertyName("version")]
        public int Version { get; set; }
    }
}
